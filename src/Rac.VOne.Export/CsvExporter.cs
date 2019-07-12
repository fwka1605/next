using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;

namespace Rac.VOne.Export
{
    public class CsvExporter<TModel> where TModel : class, new()
    {
        // ログイン情報
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public Exception Exception { get; set; }

        // 取込データ関連
        public RowDefinition<TModel> RowDef { get; protected set; }

        /// <summary>
        ///  エクスポーターのエンコーディング
        /// </summary>
        public Encoding CsvEncoding { get; set; } = Encoding.GetEncoding(932);

        // Webサービス関連
        public Func<Task<IEnumerable<TModel>>> LoadAsync { get; set; }

        public CsvExporter(
                RowDefinition<TModel> rowDef)
        {
            RowDef = rowDef;
        }

        public async Task<int> ExportAsync(string csvPath, List<TModel> original,
                CancellationToken cancel, IProgress<int> progress,
                Action<StringBuilder> specialHeaderHandler = null)
        {
            if (cancel.IsCancellationRequested) return 0;
            progress.Report(20); // 進捗：取込対象データ読込完了

            var export = new ExportWorker<TModel>(RowDef.DataExpression);
            export.LoginUserId = UserId;
            export.LoginUserCode = UserCode;
            export.LoginCompanyId = CompanyId;
            export.LoginCompanyCode = CompanyCode;

            RowDef.SetupFields();
            var sb = new StringBuilder();

            specialHeaderHandler?.Invoke(sb);
            // header
            var header = RowDef.GetHeaderAry();
            if (header != null)
            {
                sb.AppendLine(string.Join(RowDef.Delimiter, header));
            }

            await Task.Run(() =>
            {
                foreach (TModel m in original)
                {
                    export.NewRecord(m);
                    RowDef.Do(export);
                }
            }, cancel);

            // DB関連チェック
            var validate = new IdToCodeWorker<TModel>(export); // Exportの結果を引き渡す
            RowDef.Do(validate); // DBチェック＆エラー情報収集

            // TODO: 文字列項目は強制でダブルクォート という要件
            await Task.Run(() =>
            {
                foreach (var record in validate.Records.Values)
                {
                    for (int i = 0; i < record.Count; i++)
                    {
                        if (record[i] == null) continue;
                        if (record[i].IndexOf("\"") >= 0
                         || record[i].IndexOf(RowDef.Delimiter) >= 0
                         || record[i].IndexOf("\r") >= 0
                         || record[i].IndexOf("\n") >= 0)
                        {
                            record[i] = "\"" + record[i].Replace("\"", "\"\"") + "\"";
                        }
                    }
                    sb.AppendLine(string.Join(RowDef.Delimiter, record.ToArray()));
                }
            });

            try
            {
                File.WriteAllText(csvPath, sb.ToString(), CsvEncoding);
            }
            catch (Exception e)
            {
                Exception = e;
                return 0;
            }

            return validate.Records.Count;
        }
    }

    public static class CsvExporterExtension
    {
        public static CsvExporter<TModel> CreateExporter<TModel>(this RowDefinition<TModel> def)
                where TModel : class, new()
        {
            return new CsvExporter<TModel>(def);
        }
    }
}
