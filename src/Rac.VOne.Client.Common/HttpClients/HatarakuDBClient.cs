using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Client.Common.HttpClients
{
    public class HatarakuDBClient : IDisposable
    {
        public WebApiSetting WebApiSetting { private get; set; }

        private readonly HttpClient client;
        private const string UriFormat = "{base_uri}api/{function}/version/{api_version}";
        private const string UriCsvExport = "csvexport";
        private const string UriCsvDataImport = "csvdataimport";
        //private const string UriFileUpload = "fileupload";
        //private const string UriCsvImport = "csvimport";
        //private const string UriCsvImportCheck = "checkcsvipmortprocess";

        /// <summary>Web API 呼び出し制限 1分間 20 回まで
        /// 3秒に 1回 / 秒間(qps) 0.33
        /// </summary>
        private const int RequestIntervalSeconds = 3;

        public HatarakuDBClient()
        {
            client = new HttpClient();
        }

        /// <summary>テスト接続実施</summary>
        public async Task<HttpResponseMessage> ConnectAsnyc()
        {
            var request = GetRequestMessage(GetRequestUri(UriCsvExport));
            var limit = 200;
            var offset = 1;
            var extractSetting = WebApiSetting.ExtractSetting.ConvertToModel<WebApiHatarakuDBExtractSetting>();
            extractSetting.PatternCode = null;
            extractSetting.limit = limit.ToString();
            extractSetting.offset = offset.ToString();
            request.Content = new StringContent(extractSetting.ConvertToJson(), Encoding.UTF8, "application/json");
            return await client.SendAsync(request);
        }

        /// <summary>エクスポート処理の実施</summary>
        /// <param name="readHandler"></param>
        /// <returns></returns>
        /// <remarks>
        /// ・ユニークなデータの取得（連携/未連携の設定 働くDB側 view の設定？）
        /// ・一回のリクエストでの制限 200
        ///   請求明細を持つビューを連携する場合、ユニークキーの列番号を保持し、HashSet{string} にキーを入れてカウントする
        ///   200 丁度の場合は offset を 加算して再度呼び出しを行っている
        /// ・1アカウント 1分で 最大 20 リクエスト まで(0.33qps) Task.Delay で ウェイトをかけている
        /// </remarks>
        public async Task<string>  ExportAsync()
        {
            var parser = new CsvParser {
                Encoding = Encoding.UTF8,
                StreamCreator = new PlainTextMemoryStreamCreator()
            };
            var extractSetting = WebApiSetting.ExtractSetting.ConvertToModel<WebApiHatarakuDBExtractSetting>();
            extractSetting.PatternCode = null;
            var buffer = new StringBuilder();

            var limit = 200;
            var offset = 0;
            var uniqueKeyCount = 0;
            do
            {
                extractSetting.limit = limit.ToString();
                extractSetting.offset = offset.ToString();

                var request = GetRequestMessage(GetRequestUri(UriCsvExport));
                request.Content = new StringContent(extractSetting.ConvertToJson(), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new System.Net.WebException(string.Concat(response.ToString(), content));
                }

                var skip = offset == 0 ? 0 : 1;
                var lines = parser.Parse(content).ToList();
                var csv = string.Join(
                    Environment.NewLine,
                    lines.Skip(skip).Select(line =>
                        string.Join(",",
                        line.Select(field => EscapeField(field))
                        )
                    ));
                uniqueKeyCount = lines.Count - 1;
                if (uniqueKeyCount > 0)
                    buffer.AppendLine(csv);
                offset += limit;
                if (uniqueKeyCount != limit) break;
                await Task.Delay(TimeSpan.FromSeconds(RequestIntervalSeconds));
            } while (uniqueKeyCount == limit);
            return buffer.ToString();
        }
        private string EscapeField(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (!(value.IndexOf(",")  >= 0
               || value.IndexOf("\"") >= 0
               || value.IndexOf("\n") >= 0)) return value;
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        /// <summary>インポート処理の実施</summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<List<string>> OutputAsync(List<HatarakuDBData> source)
        {
            var processIds = new List<string>();
            var outputSetting = WebApiSetting.OutputSetting.ConvertToModel<WebApiHatarakuDBOutputSetting>();

            source = source.Where(x => !string.IsNullOrEmpty(x.InvoiceCode)).ToList();

            var offset = 0;
            var limit = 200;
            var count = 0;
            do
            {
                var request = GetRequestMessage(GetRequestUri(UriCsvDataImport), "multipart/form-data");
                var contents = new MultipartFormDataContent();
                var jsonParameter = new StringContent(outputSetting.ConvertToJson(), Encoding.UTF8, "application/json");
                jsonParameter.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "json" };
                contents.Add(jsonParameter);

                count = Math.Min(source.Count - count, offset + limit);
                var buffer = source.Skip(offset).Take(count).ToArray();
                var csv = string.Join(Environment.NewLine,
                    buffer.Select(x => string.Join(",",
                        ConvertToLine(x).Select(y => EscapeField(y))))
                    );
                var csvParameter = new StringContent(csv, Encoding.UTF8, "text/csv");
                csvParameter.Headers.ContentDisposition = new ContentDispositionHeaderValue($"form-data")
                {
                    Name = "uploadFile",
                    FileName = "uploadFile.csv",
                };
                contents.Add(csvParameter, "uploadFile", "uploadFile.csv");
                request.Content = contents;

                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new System.Net.WebException(string.Concat(response.ToString(), content));
                }

                var result = content.ConverToDynamic();
                var id = Convert.ToString(result.processId);
                processIds.Add(id);

                if (count < limit
                    || (offset + count) == source.Count) break;
                offset += count;
                await Task.Delay(TimeSpan.FromSeconds(RequestIntervalSeconds));
            }
            while (count == limit);

            return processIds;
        }

        private IEnumerable<string> ConvertToLine(HatarakuDBData data)
        {
            yield return data.InvoiceCode;
            yield return data.RecordedAt.ToString("yyyy-MM-dd");
            yield return data.AssignmentAmount.ToString("0");
            yield return data.VOneTransferedFlag;
        }

        private HttpRequestMessage GetRequestMessage(string uri, string mediatype = "application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediatype));
            request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            request.Headers.Add("X-HD-apitoken", WebApiSetting.AccessToken);
            return request;
        }

        private string GetRequestUri(string function)
            => UriFormat
            .Replace("{base_uri}", WebApiSetting.BaseUri)
            .Replace("{function}", function)
            .Replace("{api_version}", WebApiSetting.ApiVersion);

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
