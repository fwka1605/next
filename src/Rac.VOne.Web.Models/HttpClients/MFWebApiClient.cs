using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Rac.VOne.Web.Models.HttpClients
{
    /// <summary>MFクラウド請求書 Web API 連携設定 の wrapper</summary>
    /// <remarks>
    /// MF Web API では、OAuth 2.0 の認証を行う
    /// access_token / refresh_token を一度取得すれば、認証 (Auth/Authorize)を行う必要はない
    /// refresh_token が失効してしまった場合は、再度認証処理が必要となる
    /// ilfe time
    /// access_token          : 30 day
    /// refresh_token         : 
    /// authorization_code    : 10 min
    /// 
    /// MF Web API の処理手順は下記
    /// 1. OAuth による 認証 (access_token / refresh_token取得済の場合はスキップ可能)
    /// 2. 各種業務 API の呼び出し 取得/作成
    /// 
    /// async/await で複数同時実行したりするため、将来的には内部にそれぞれcounter を所持して
    /// 自動的に await Task.Delay などが行えるのが理想
    /// 現状はリニア（線形）に実施した場合を想定して、loop内に1件あたりの秒数でウェイトをかけている
    /// 実施するには、 API 毎の ConccurentDictionary を用意し、TimeStamp などで判別が必要か...
    /// ※同一のapi設定を 複数ユーザーが同時に操作することは想定していない
    /// </remarks>
    public class MFWebApiClient : IDisposable
    {
        #region メンバー
        private readonly HttpClient client;

        /// <summary> BaseUri </summary>
        public string BaseUri { get { return @"https://invoice.moneyforward.com"; } }
        /// <summary> Callback Url </summary>
        public string RedirectUri { get { return @"https://www.r-ac.co.jp"; } }
        /// <summary> Scope </summary>
        public string Scope { get { return "write"; } }

        private string GetUriNoVersion(string method) => $"{BaseUri}/{method}";
        private string GetUri(string method) => GetUriNoVersion($"api/{WebApiSetting.ApiVersion}/{method}");

        public WebApiSetting WebApiSetting { get; set; }
        private const string MediaTypeJson = "application/json";
        private const string MediaTypeFormUrlEncoded = "application/x-www-form-urlencoded";
        private string GetFormUrlEncodedEntity(Dictionary<string, string> parameters)
          => string.Join("&", parameters.Select(x => $"{x.Key}={UrlEncode(x.Value)}"));
        private string UrlEncode(string value)
            => System.Net.WebUtility.UrlEncode(value);
        public Action<string> ErrorListener { get; set; }
        public bool TokenRefreshed { get; set; } = false;
        /// <summary>
        /// 通常 Web API 呼出し制限 1分あたり 60回
        /// 1秒に 1回 / 秒間 1 qps
        /// ※ 通信に掛かる時間を考慮すると、ほぼ ウェイトなしでOKだが、安全策で1秒待機
        /// </summary>
        private const int RequestIntervalSecondsForDefaultApi = 1;

        /// <summary> 認証コード </summary>
        public string AuthorizationCode { get; set; }

        #endregion

        #region 初期化
        public MFWebApiClient()
        {
            client = new HttpClient();
        }

        #endregion

        #region common

        /// <summary>問合せ用オブジェクトの生成</summary>
        /// <param name="uri">クエリパラメータを含めた完全なURIを指定</param>
        /// <param name="mediaType">mediatype default値は application/json</param>
        /// <param name="initializer"><see cref="HttpRequestMessage"/>の初期化処理ハンドラ
        /// RequestHeaderや、Contentを指定したい場合に利用可能
        /// </param>
        /// <returns></returns>
        private HttpRequestMessage GetRequestMessage(string uri,
         HttpMethod method = null,
         string mediaType = MediaTypeJson,
         Action<HttpRequestMessage> initializer = null)
        {
            var request = new HttpRequestMessage(method ?? HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            if (!string.IsNullOrEmpty(WebApiSetting.AccessToken))
                request.Headers.Add("Authorization", $"Bearer {WebApiSetting.AccessToken}");
            initializer?.Invoke(request);
            return request;
        }

        private async Task<List<TModel>> GetModelsInnerAsync<TModel>(
           string modelName,
           Func<string, List<TModel>> getItems,
           Func<string, int> getTotalCount,
           DateTime? from,
           DateTime? to,
           Action<HttpRequestMessage> requestInitializer = null)
        {
            var pageNo = 0;
            int? totalCount = null;
            var perPage = 100;
            var list = new List<TModel>();
            do
            {
                pageNo++;
                var response = await SendInnerAsync(() => {
                    var uri = GetUri($"billings/search.json?page={pageNo}&per_page={perPage}&range_key=billing_date");
                    if (from.HasValue)
                        uri += $"&from={from.Value:yyy-MM-dd}";
                    if(to.HasValue)
                        uri += $"&to={to.Value:yyy-MM-dd}";
                    return GetRequestMessage(uri, HttpMethod.Get, initializer: requestInitializer);
                });
                if (!response.IsSuccessStatusCode) break;

                var content = await response.Content.ReadAsStringAsync();

                if (!totalCount.HasValue)
                {
                    totalCount = getTotalCount(content);
                }
                var items = getItems(content);

                list.AddRange(items);
                if (totalCount == list.Count) break;

                // rate limiting
                await Task.Delay(TimeSpan.FromSeconds(RequestIntervalSecondsForDefaultApi));
            } while (true);
            return list;
        }

        private async Task<HttpResponseMessage> SendInnerAsync(Func<HttpRequestMessage> request)
        {
            do
            {
                var response = await client.SendAsync(request());
                if (response.IsSuccessStatusCode) return response;

                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var res = await RefreshTokenAsync();
                    if (!res) return response;
                    TokenRefreshed = true;
                }
                else
                {
                    ErrorListener?.Invoke(response.ReasonPhrase);
                    return response;
                }
            } while (true);
        }

        #endregion

        #region authorization

        /// <summary>
        /// 認証処理 認証コード取得 authroization_code (Browser で ユーザーが ID/Password を入力)
        /// トークンの取得 access_token / refresh_token
        /// → 取得した access_token / refresh_token を WebApiSetting プロパティに保持
        /// → DBに登録するなどして保持しておくこと
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AuthorizeAsync()
        {
            if (string.IsNullOrEmpty(AuthorizationCode)) return false;
            WebApiSetting.BaseUri = BaseUri;
            var tokenResult = await GetTokenAsync(AuthorizationCode);
            return tokenResult;
        }

        /// <summary>
        /// 認証コードより、access_token / refresh_token を取得する処理
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        private async Task<bool> GetTokenAsync(string authorizationCode)
        {
            var request = GetRequestMessage(GetUriNoVersion("oauth/token"), mediaType : MediaTypeFormUrlEncoded);
            var parameters = new Dictionary<string, string> {
                { "grant_type"   , "authorization_code" },
                { "client_id"    , WebApiSetting.ClientId },
                { "client_secret", WebApiSetting.ClientSecret },
                { "redirect_uri" , RedirectUri },
                { "code"         , authorizationCode },
            };
            request.Content = new StringContent(GetFormUrlEncodedEntity(parameters),
                Encoding.UTF8, MediaTypeFormUrlEncoded);
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                ErrorListener?.Invoke($"トークンの取得に失敗しました。{Environment.NewLine}{response}{Environment.NewLine}{content}");
                return false;
            }
            return ParseTokenContent(content);
        }

        private async Task<bool> RefreshTokenAsync()
        {
            var request = GetRequestMessage(GetUriNoVersion("oauth/token"), mediaType: MediaTypeFormUrlEncoded);
            var parameters = new Dictionary<string, string> {
                { "client_id"       , WebApiSetting.ClientId },
                { "redirect_uri" , RedirectUri },
                { "grant_type"      , "refresh_token"},
                { "refresh_token"   , WebApiSetting.RefreshToken },
            };
            request.Content = new StringContent(GetFormUrlEncodedEntity(parameters),
                Encoding.UTF8, MediaTypeFormUrlEncoded);
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                ErrorListener?.Invoke($"トークンのリフレッシュに失敗しました。{Environment.NewLine}{response}{Environment.NewLine}{content}");
                return false;
            }
            return ParseTokenContent(content);
        }

        private bool ParseTokenContent(string content)
        {
            var result = content.ConverToDynamic();
            var parseResult = false;
            try
            {
                WebApiSetting.AccessToken = Convert.ToString(result.access_token);
                WebApiSetting.RefreshToken = Convert.ToString(result.refresh_token);
                parseResult = true;
            }
            catch (Exception) { }
            return parseResult;
        }

        public async Task<bool> ValidateToken()
        {
            var uri = GetUri("office.json");
            var response = await SendInnerAsync(() => GetRequestMessage(uri, HttpMethod.Get));
            return response.IsSuccessStatusCode;
        }

        #endregion


        /// <summary>事務所情報 取得</summary>
        public async Task<MFModels.Office> GetOfficeAsync()
        {
            var uri = GetUri("office.json");
            var response = await SendInnerAsync(() => GetRequestMessage(uri, HttpMethod.Get));
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return content.ConvertToModel<MFModels.Office>();
        }

        /// <summary>請求データ 取得</summary>
        public async Task<List<MFModels.billing>> GetBillingAsync(DateTime? from, DateTime? to)
            => await GetModelsInnerAsync(nameof(MFModels.PagedBillings.billings),
           json => json.ConvertToModel<MFModels.PagedBillings>().billings,
           json => json.ConvertToModel<MFModels.PagedBillings>().meta.total_count,
           from,
           to);

        /// <summary>取引先情報 取得</summary>
        public async Task<MFModels.partner> GetPartnersAsync(string id)
        {
            var uri = GetUri($"partners/{id}.json");
            var response = await SendInnerAsync(() => GetRequestMessage(uri, HttpMethod.Get));
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return content.ConvertToModel<MFModels.partner>();
        }

        /// <summary>連携された 請求ID に対しての 入金ステータス更新</summary>
        /// <param name="ids">idの配列</param>
        /// <param name="isMatched">true:2:消込済, false:0:未設定</param>
        /// <returns>一件でもエラーがあればエラー</returns>
        public async Task<bool> PatchPaymentAsync(IEnumerable<string> ids, bool isMatched)
        {
            var value = new MFModels.UpdateItem(isMatched);
            var jsonContent = new StringContent(value.ConvertToJson(), Encoding.UTF8, MediaTypeJson);
            Func<string, HttpRequestMessage> getRequestMessage = (id)
                => GetRequestMessage(GetUri($"billings/{id}/billing_status/payment"),
                    new HttpMethod("PATCH"),
                    initializer: request => request.Content = jsonContent);
            var result = true;
            try
            {
                foreach (var id in ids)
                {
                    var response = await SendInnerAsync(() => getRequestMessage(id));
                    if (!response.IsSuccessStatusCode) {
                        var content = await response.Content.ReadAsStringAsync();
                        ErrorListener?.Invoke(content);
                        result = false;
                        break;
                    }
                    if (id != ids.Last())
                        await Task.Delay(TimeSpan.FromSeconds(RequestIntervalSecondsForDefaultApi));
                }
            }
            catch (Exception ex)
            {
                ErrorListener?.Invoke(ex.ToString());
            }
            return result;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
