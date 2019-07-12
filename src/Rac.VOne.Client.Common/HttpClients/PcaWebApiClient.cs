using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.PcaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rac.VOne.Client.Common.HttpClients
{
    /// <summary>PCA クラウド会計 DX Web API の wrapper</summary>
    /// <remarks>
    /// PCA Web API では、OAuth 2.0 の認証を行う
    /// access_token / refresh_token を一度取得すれば、認証 (Auth/Authorize)を行う必要はない
    /// refresh_token が失効してしまった場合は、再度認証処理が必要となる
    /// ilfe time
    /// access_token        : 60 min
    /// refresh_token       : 30 day
    /// authorization_code  :  5 min
    /// 
    /// PCA Web API の処理手順は下記
    /// 1. OAuth による 認証 (access_token / refresh_token取得済の場合はスキップ可能)
    /// 2. データ領域の選択 (access_token と データ領域の紐付け)
    /// 3. 各種業務 API の呼び出し 取得/作成
    /// 4. ログアウト (CALなど同時接続数制限のため、業務終了後呼ばれるようにすること)
    /// 
    /// Web API の Rate Limiting が 処理毎に詳細に分かれている
    /// OAuth 系 10 count / min
    /// 書込み系 20 count / min
    /// SQLテンプレート系 100 count / min
    /// それ以外 60 count / min
    /// async/await で複数同時実行したりするため、将来的には内部にそれぞれcounter を所持して
    /// 自動的に await Task.Delay などが行えるのが理想
    /// 現状はリニア（線形）に実施した場合を想定して、loop内に1件あたりの秒数でウェイトをかけている
    /// 実施するには、 API 毎の ConccurentDictionary を用意し、TimeStamp などで判別が必要か...
    /// ※同一のapi設定を 複数ユーザーが同時に操作することは想定していない
    /// </remarks>
    public class PcaWebApiClient : IDisposable
    {
        private const string RedirectUri             = "urn:pca:oauth20:desktop";
        private const string MediaTypeFormUrlEncoded = "application/x-www-form-urlencoded";
        private const string MediaTypeJson           = "application/json";
        private const string InputModuleName         = "Victory ONE G4";
        private const string AppName                 = "Acc20";
        private readonly HttpClient client;

        public WebApiSetting WebApiSetting { get; set; }

        public Action<string> ErrorListener { get; set; }

        public bool TokenRefreshed { get; set; } = false;

        /// <summary>
        /// 通常 Web API 呼出し制限 1分あたり 60回
        /// 1秒に 1回 / 秒間 1 qps
        /// ※ 通信に掛かる時間を考慮すると、ほぼ ウェイトなしでOKだが、安全策で1秒待機
        /// </summary>
        private const int RequestIntervalSecondsForDefaultApi = 1;
        /// <summary>
        /// 書込み系API 呼出し制限 1分あたり 20回
        /// 3秒に 1回 / 秒間 0.33 qps
        /// </summary>
        private const int RequestInterfalSecondsForCreateApi = 3;

        public PcaWebApiClient()
        {
            client = new HttpClient();
        }

        #region common

        /// <summary>
        /// PCA Web API の 各種 Uri を 生成するメソッド
        /// <see cref="WebApiSetting.BaseUri"/>と、メソッドを組み合わせて問合せを行う
        /// URL Query を利用する場合は、<see cref="method"/>に値を追加する必要がある
        /// Query parameter は 事前に URLエンコードしておくこと
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private string GetUri(string method) => "{base_uri}{version}/{app_name}/{method}"
            .Replace("{base_uri}"   , WebApiSetting.BaseUri)
            .Replace("{version}"    , WebApiSetting.ApiVersion)
            .Replace("{app_name}"   , AppName)
            .Replace("{method}"     , method);

        /// <summary>問合せ用オブジェクトの生成 POST メソッドのみ</summary>
        /// <param name="uri">クエリパラメータを含めた完全なURIを指定</param>
        /// <param name="mediaType">mediatype default値は application/json</param>
        /// <param name="initializer"><see cref="HttpRequestMessage"/>の初期化処理ハンドラ
        /// RequestHeaderや、Contentを指定したい場合に利用可能
        /// </param>
        /// <returns></returns>
        private HttpRequestMessage GetRequestMessage(string uri,
            string mediaType = MediaTypeJson,
            Action<HttpRequestMessage> initializer = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            if (!string.IsNullOrEmpty(WebApiSetting.AccessToken))
                request.Headers.Add("Authorization", $"Bearer {WebApiSetting.AccessToken}");
            initializer?.Invoke(request);
            return request;
        }

        /// <summary>
        /// <see cref="HttpUtility.UrlEncode(string)"/>の ラッパーメソッド
        /// 文字数短縮
        /// </summary>
        /// <param name="value"></param>
        private string UrlEncode(string value)
            => HttpUtility.UrlEncode(value);

        /// <summary>
        /// Dictionary の Value を URLエンコードして、すべて & で連結した文字列を返す処理
        /// 認証処理(OAuth)で利用、他はほぼ json パラメータを利用する
        /// </summary>
        /// <param name="parameters"></param>
        private string GetFormUrlEncodedEntity(Dictionary<string, string> parameters)
            => string.Join("&", parameters.Select(x => $"{x.Key}={UrlEncode(x.Value)}"));

        /// <summary>clinet.SendAsync の wrapper
        /// 認証系処理失敗時に、内部で oauth 系の処理を実施する
        /// refresh_token 失効時の認証処理が確認とれていない
        /// </summary>
        /// <param name="request"><see cref="HttpClient"/> では、一度送信した <see cref="HttpRequestMessage"/> の再利用が行えない
        /// ※async/awaitなどの関係上？ <see cref="HttpRequestMessage"/>を生成する Function を引数にとり、
        /// 正常に疎通できるまで、 <see cref="HttpRequestMessage"/> を生成する</param>
        /// <returns>正常系 200 系の場合の <see cref="HttpResponseMessage"/></returns>
        private async Task<HttpResponseMessage> SendInnerAsync(Func<HttpRequestMessage> request)
        {
            do
            {
                var response = await client.SendAsync(request());
                if (response.IsSuccessStatusCode) return response;

                // error 発生時に 内容を解析して refresh / 再認証を行う
                var content = await response.Content.ReadAsStringAsync();
                var apierror = content.ConvertToModel<ApiError>();

                if (IsAccessTokenExpired(apierror))
                {
                    var res = await RefreshTokenAsync();
                    if (!res) return response;
                    TokenRefreshed = true;
                }
                else if (IsRefreshTokenExpired(apierror))
                {
                    var res = await AuthorizeAsync();
                    if (!res) return response;
                    TokenRefreshed = true;
                }
                else
                {
                    ErrorListener?.Invoke(apierror.Message);
                    return response;
                }
            } while (true);
        }

        /// <summary>モデルを取得(Find)する処理
        /// limit 200 件を考慮し、不足時は、メソッドを継続して実施する
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelName">検索する モデル名</param>
        /// <param name="parser">json(string) から IEnumerable{TModel} へ変換する処理</param>
        /// <param name="requestInitializer">リクエストメッセージの初期化処理</param>
        /// <returns></returns>
        private async Task<IEnumerable<TModel>> GetModelsInnerAsync<TModel>(
            string modelName,
            Func<string, IEnumerable<TModel>> parser,
            Action<HttpRequestMessage> requestInitializer = null)
        {
            var baseUri = GetUri($"Find/{modelName}");
            var offset = 0;
            var totalCount = 0;
            var limit = 200;
            var list = new List<TModel>();
            var snapshotId = string.Empty;
            do
            {
                var response = await SendInnerAsync(()=> {
                    var uri = baseUri;
                    if (offset != 0 && !string.IsNullOrEmpty(snapshotId))
                        uri += $"?SnapshotId={snapshotId}&Offset={offset}&Limit={limit}";
                    else
                        uri += $"?Limit={limit}";
                    return GetRequestMessage(uri, initializer: requestInitializer);
                });
                if (!response.IsSuccessStatusCode) break;

                if (totalCount == 0)
                {
                    IEnumerable<string> values;
                    if (response.Headers.TryGetValues("X-PCA-Pagination-SnapshotId", out values))
                        snapshotId = values.FirstOrDefault();
                    if (response.Headers.TryGetValues("X-PCA-Pagination-TotalListCount", out values))
                        int.TryParse(values.FirstOrDefault(), out totalCount);
                }
                var content = await response.Content.ReadAsStringAsync();
                var items = parser.Invoke(content).ToList();

                list.AddRange(items);
                if (totalCount == list.Count) break;
                offset = list.Count;

                // rate limiting
                await Task.Delay(TimeSpan.FromSeconds(RequestIntervalSecondsForDefaultApi));
            } while (true);
            return list;
        }

        /// <summary>
        /// Create メソッドの呼び出し
        /// modelName に url encodeした query parameterを追加すること
        /// Create メソッドは 10件ずつ 20 count / min なので、10件処理するごとに 3 sec wait を掛けている
        /// </summary>
        /// <typeparam name="TModel">登録したい Entity の型</typeparam>
        /// <param name="list">登録したい すべての Entity</param>
        /// <param name="modelName"></param>
        /// <param name="converter">1..10件ずつの Entity を json形式の文字列へ変換する処理
        /// 型情報と密接に結びつくため、Function とした</param>
        /// <param name="errorHandler">エラー発生時のハンドラ 戻り値を<see cref="ApiError"/>に変換し、<see cref="ApiError.Message"/> を引数にとる<see cref="Action"/></param>
        /// <returns></returns>
        private async Task<bool> CreateAsync<TModel>(List<TModel> list, string modelName,
            Func<IEnumerable<TModel>, string> converter,
            Action<string> errorHandler)
        {
            var take = 10;
            for (var skip = 0; skip < list.Count; skip += take)
            {
                var items = list.Skip(skip).Take(take).ToList();
                var response = await SendInnerAsync(() => GetRequestMessage(GetUri($"Create/{modelName}"),
                    initializer: request => request.Content = new StringContent(converter(items), Encoding.UTF8, MediaTypeJson)));
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apierror = content.ConvertToModel<ApiError>();
                    errorHandler(apierror.Message);
                    return false;
                }
                await Task.Delay(TimeSpan.FromSeconds(RequestInterfalSecondsForCreateApi));
            }
            return true;
        }

        #endregion

        #region oauth

        /// <summary>
        /// 認証処理 認証コード取得 authroization_code (Browser で ユーザーが ID/Password を入力)
        /// トークンの取得 access_token / refresh_token
        /// → 取得した access_token / refresh_token を WebApiSetting プロパティに保持
        /// → DBに登録するなどして保持しておくこと
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AuthorizeAsync()
        {
            var authorizationCode = await GetAuthorizationCodeAsync();
            if (string.IsNullOrEmpty(authorizationCode)) return false;
            var tokenResult = await GetTokenAsync(authorizationCode);
            return tokenResult;
        }

        /// <summary>
        /// IE を起動し、認証コード (authorization_code) を取得する処理
        /// PCA クラウド の 認証画面を表示し、ブラウザ上で、
        /// サービスID、パスワード
        /// ログインユーザーID, パスワード を入力し、アプリケーション連携の認証を OAuth2.0 で実施する処理
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAuthorizationCodeAsync()
        {
            var parameters = new Dictionary<string, string> {
                { "response_type", "code" },
                { "client_id"    , WebApiSetting.ClientId },
                { "redirect_uri" , RedirectUri },
            };
            var uri = GetUri($"Auth/Authorize?{GetFormUrlEncodedEntity(parameters)}");
            var wrapper = new IEWrapper();
            return await wrapper.GetAuthorizationCodeAsync(uri);
        }

        /// <summary>
        /// 認証コードより、access_token / refresh_token を取得する処理
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>

        private async Task<bool> GetTokenAsync(string authorizationCode)
        {
            var request = GetRequestMessage(GetUri("Auth/Token"), MediaTypeFormUrlEncoded);
            var parameters = new Dictionary<string, string> {
                { "grant_type"   , "authorization_code" },
                { "code"         , authorizationCode },
                { "client_id"    , WebApiSetting.ClientId },
                { "client_secret", WebApiSetting.ClientSecret },
                { "redirect_uri" , RedirectUri }
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

        /// <summary>
        /// access_token の refresh 処理
        /// </summary>
        /// <returns></returns>
        private async Task<bool> RefreshTokenAsync()
        {
            var request = GetRequestMessage(GetUri("Auth/Token"), MediaTypeFormUrlEncoded);
            var parameters = new Dictionary<string, string> {
                { "grant_type"      , "refresh_token"},
                { "refresh_token"   , WebApiSetting.RefreshToken },
                { "client_id"       , WebApiSetting.ClientId },
                { "client_secret"   , WebApiSetting.ClientSecret }
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

        /// <summary>oauth の結果 パース</summary>
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

        /// <summary>access_token の失効確認</summary>
        private bool IsAccessTokenExpired(ApiError apierror)
        {
            if (apierror == null) return false;
            return (apierror.Code == "WEBSV600100"
                 || apierror.Code == "WEBSV600101"
                 || apierror.Code == "WEBSV600102"
             );
        }

        /// <summary>refresh_token の失効確認</summary>
        private bool IsRefreshTokenExpired(ApiError apierror)
        {
            //  WEBSV600102 は refresh_token のエラーではないか？？
            return false;
        }

        #endregion

        #region data area

        /// <summary>データ領域 選択処理</summary>
        /// <param name="selector">データ領域選択用 delegate</param>
        /// <returns></returns>
        public async Task<bool> SelectDataAreaAsync(
            Func<IEnumerable<BECommonDataArea>, BECommonDataArea> selector)
        {
            var list = (await FindDataAreaAsync()).ToList();
            BECommonDataArea area = null;
            if (list.Count > 1 && selector != null)
                area = selector(list);
            else
                area = list.FirstOrDefault();
            if (area == null) return false;
            var selectResult = await SelectDataAreaAsync(area.Name);
            return true;
        }

        /// <summary>
        /// データ領域の取得
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<BECommonDataArea>> FindDataAreaAsync()
        {
            var response = await SendInnerAsync(() => GetRequestMessage(GetUri("FindDataArea")));
            if (!response.IsSuccessStatusCode) return Enumerable.Empty<BECommonDataArea>();
            var content = await response.Content.ReadAsStringAsync();
            var item = content.ConvertToModel<BECommonDataAreaSet>();
            return item?.ArrayOfBECommonDataArea.BECommonDataArea ?? Enumerable.Empty<BECommonDataArea>();
        }

        /// <summary>指定されたデータ領域の選択</summary>
        /// <param name="name"><see cref="BECommonDataArea.Name"/> データ領域名</param>
        /// <returns></returns>
        private async Task<bool> SelectDataAreaAsync(string name)
        {
            var keys = new Dictionary<string, string> { { "DataArea", name } };
            var response = await SendInnerAsync(() =>
                GetRequestMessage(GetUri("SelectDataArea"),
                    initializer: request => request.Content = new StringContent(keys.ConvertToJson(), Encoding.UTF8, MediaTypeJson)));
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        #endregion

        #region find data

        /// <summary>基本会社情報 取得</summary>
        public async Task<BEComp> GetCompAsync()
        {
            var response = await SendInnerAsync(() => GetRequestMessage(GetUri("Find/Comp")));
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return content.ConvertToModel<BECompSet>()?.BEComp;
        }

        /// <summary>勘定科目 取得</summary>
        public async Task<IEnumerable<BEKmk>> GetKmkAsync() => await GetModelsInnerAsync("Kmk",
            json => json.ConvertToModel<BEKmkSet>().ArrayOfBEKmk.BEKmk);

        /// <summary>補助科目 取得</summary>
        public async Task<IEnumerable<BEHojo>> GetHojoAsync() => await GetModelsInnerAsync("Hojo",
            json => json.ConvertToModel<BEHojoSet>().ArrayOfBEHojo?.BEHojo ?? Enumerable.Empty<BEHojo>());

        /// <summary>部門 取得</summary>
        public async Task<IEnumerable<BEBu>> GetBuAsync() => await GetModelsInnerAsync("Bu",
            json => json.ConvertToModel<BEBuSet>().ArrayOfBEBu.BEBu);

        /// <summary>税区分 取得</summary>
        public async Task<IEnumerable<BETaxClass>> GetTaxClassAsync() => await GetModelsInnerAsync("TaxClass",
            json => json.ConvertToModel<BETaxClassSet>().ArrayOfBETaxClass.BETaxClass);

        public async Task<string> FindSlipAsync()
        {
            var response = await SendInnerAsync(() => GetRequestMessage(GetUri($"Find/InputSlip")));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadAsStringAsync();
        }

        #endregion

        #region create input slip

        /// <summary>伝票入力 連携 10件以上の伝票が連携された場合、
        /// 内部で10件ずつ Web API をコールする
        /// web api rate limiting への対応として、do loop 内で wait を掛けている
        /// （本当は、1分間以内のコール数をカウントしておき、連続で実施した方がパフォーマンスは良い）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="errorHandler"></param>
        /// <returns></returns>
        public async Task<bool> CreateSlipAsync(List<BEInputSlip> list, Action<string> errorHandler)
        {
            var parameters = new Dictionary<string, string> {
                { "InputModuleName" , InputModuleName },
                { "FlagFormat"      , "Number" },
            };
            var modelName = $"InputSlip?{GetFormUrlEncodedEntity(parameters)}";
            var result = await CreateAsync(list, modelName,
                items => {
                    if (items.Count() == 1)
                        return (new BEInputSlipSet { BEInputSlip = items.First() }).ConvertToJson();
                    else
                        return (new BEInputSlipsSet {
                            ArrayOfBEInputSlip = new ArrayOfBEInputSlip {
                                BEInputSlip = items.ToList()
                            }
                        }).ConvertToJson();
                },
                errorHandler);

            return result;
        }

        #endregion

        #region logout

        /// <summary>ログアウト</summary>
        public async Task<bool> LogoutAsync()
        {
            var response = await SendInnerAsync(() => GetRequestMessage(GetUri("Logout")));
            return response.IsSuccessStatusCode;
        }

        #endregion

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
