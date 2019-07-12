using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Import
{
    /// <summary>CSV取込処理</summary>
    /// <typeparam name="TModel">取り込んだデータを格納するためのモデル</typeparam>
    /// <typeparam name="TIdentity">モデルの一意性を表す型。基本型、匿名クラス、ExpandoObjectを想定。</typeparam>
    public class CsvImporter<TModel, TIdentity> : IImporter
        where TModel : class, new()
    {
        // ログイン情報
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public int UserId { get; set; }
        public string UserCode { get; set; }

        // 取込データ関連
        public RowDefinition<TModel> RowDef { get; protected set; }
        private readonly Func<TModel, TIdentity> createIdentity;

        public Encoding CsvEncoding { get; set; } = Encoding.GetEncoding(932);
        public string ErrorLogPath { get; set; }
        public Encoding ErrorLogEncoding { get; set; } = Encoding.GetEncoding(932);

        // 結果
        public UnitOfWork<TModel> UnitOfWork { get; private set; }
        public List<WorkingReport> Errors { get; } = new List<WorkingReport>();

        // Webサービス関連
        public Func<Task<IEnumerable<TModel>>> LoadAsync { get; set; }

        public Func<UnitOfWork<TModel>, Task<ImportResult>> RegisterAsync { get; set; }

        public Func<ImportWorker<TModel>, Task> InitializeWorker { get; set; } = worker => Task.CompletedTask;

        public Func<ImportWorker<TModel>, Task> AdditionalWorker { get; set; } = worker => Task.CompletedTask;

        public Action<ImportWorker<TModel>, ImportResult> PostImportHanlder { get; set; }

        public ICsvParser Parser { get; set; } = new CsvParser();

        public CsvImporter(
                RowDefinition<TModel> rowDef,
                Func<TModel, TIdentity> identitySelector)
        {
            RowDef = rowDef;
            createIdentity = identitySelector;
        }

        public async Task<ImportResult> ImportAsync(string csvPath,
            ImportMethod method,
            CancellationToken? token,
            IProgress<int> progress)
        {
            IEnumerable<TModel> original = (LoadAsync == null) ? Enumerable.Empty<TModel>() : await LoadAsync();
            if (token?.IsCancellationRequested ?? false) return null;
            progress?.Report(20); // 進捗：取込対象データ読込完了

            var unitOfWork = new UnitOfWorkWithReference<TModel, TIdentity>(
                original, createIdentity);

            var worker = new ImportWorker<TModel>(RowDef);
            worker.LoginUserId = UserId;
            worker.LoginUserCode = UserCode;
            worker.LoginCompanyId = CompanyId;
            worker.LoginCompanyCode = CompanyCode;

            await InitializeWorker?.Invoke(worker);

            var valid = true;
            var csv = Parser.Parse(csvPath);

            RowDef.SetupFields();
            var fieldDefs = RowDef.Fields
                .Where(f => !f.Ignored)
                .OrderBy(f => f.FieldNumber)
                .ToArray();
            foreach (var fields in csv.Skip(RowDef.StartLineNumber))
            {
                worker.NewRecord(fields);
                if (fields.Length < fieldDefs.Length)
                {
                    foreach (var field in fieldDefs.Skip(fields.Length))
                    {
                        Errors.Add(new WorkingReport
                        {
                            LineNo = worker.RecordCount,
                            FieldNo = field.FieldIndex,
                            FieldName = field.FieldName,
                            Value = string.Empty,
                            Message = $"{field.FieldName}がありません。",
                        });
                    }
                }
                valid &= RowDef.Do(worker); // 書式チェック＆エラー情報収集

                if (worker.Reports.Any())
                {
                    Errors.AddRange(worker.Reports);
                    worker.Reports.Clear();
                }
            }
            progress?.Report(40); // 進捗：CSVファイル読込完了

            // DB関連チェック
            var validate = new CodeToIdWorker<TModel>(worker); // importの結果を引き渡す
            valid &= RowDef.Do(validate); // DBチェック＆エラー情報収集
            Errors.AddRange(validate.Reports);

            await AdditionalWorker?.Invoke(worker);

            var additionalValidate = new AdditionalValidationWorker<TModel>(worker)
            {
                ImportMethod = method,
            };
            valid &= RowDef.Do(additionalValidate); // DBチェック＆エラー情報収集
            Errors.AddRange(additionalValidate.Reports);

            foreach (var lineNo in Errors.Where(x => x.LineNo.HasValue).Select(x => x.LineNo.Value).Distinct())
            {
                worker.Models.Remove(lineNo);
            }

            // DB重複チェック
            if (!method.ValidateDuplicated(unitOfWork, worker))
            {
                worker.Reports.ForEach(r => worker.Models.Remove(r.LineNo.Value));
                Errors.AddRange(worker.Reports);
                worker.Reports.Clear();
            }

            // ファイル内キー重複チェック
            var uniqueKeys = new Dictionary<TIdentity, int>();

            var duplicatedLines = new List<int>();
            foreach (var pair in validate.Models)
            {
                // 必須項目が空欄の時は、Nullまたはゼロになっている。
                // キー項目が部分的にでも空欄の場合、重複チェックをパスする。
                var identity = createIdentity(pair.Value);
                if (ContainsNull(identity))
                {
                    identity = default(TIdentity);
                }
                if (identity == null || identity.Equals(default(TIdentity))) continue;

                if (uniqueKeys.ContainsKey(identity))
                {
                    switch (RowDef.DuplicateAdoption)
                    {
                        case DuplicateAdoption.BothAreErrors:
                            var duplicated = uniqueKeys[identity];
                            if (!duplicatedLines.Contains(duplicated))
                            {
                                duplicatedLines.Add(duplicated);
                            }
                            duplicatedLines.Add(pair.Key);
                            break;
                        case DuplicateAdoption.First:
                            duplicatedLines.Add(pair.Key);
                            break;
                    }
                }
                else
                {
                    uniqueKeys.Add(identity, pair.Key);
                }
                if (token.HasValue && token.Value.IsCancellationRequested) return null;
            }
            progress?.Report(60); // 進捗：データベース関連チェック完了

            switch (RowDef.TreatDuplicateAs)
            {
                case TreatDuplicateAs.Error:
                    duplicatedLines.ForEach(lineNo =>
                    {
                        Errors.Add(new WorkingReport() // キー重複
                        {
                            LineNo = lineNo,
                            FieldName = RowDef.KeyFields?.FirstOrDefault().FieldName ?? string.Empty,
                            Message = "重複しているため、インポートできません。",
                            Value = createIdentity(validate.Models[lineNo]).ToString(),
                        });
                    });
                    break;
                case TreatDuplicateAs.Ignore:
                    // エラーにはせず、ここで取込対象から取り除く。
                    duplicatedLines.ForEach(lineNo => worker.Ignore(lineNo));
                    break;
            }
            if (token.HasValue && token.Value.IsCancellationRequested) return null;

            // エラーデータを更新対象から取り除く
            var errorLines = Errors
                    .GroupBy(report => report.LineNo)
                    .Select(g => g.Key)
                    .ToList();
            if (errorLines.Any(lineNo => !lineNo.HasValue))
            {
                validate.Models.Clear();
            }
            else
            {
                errorLines.ForEach(lineNo => validate.Models.Remove(lineNo.Value));
            }
            if (token.HasValue && token.Value.IsCancellationRequested) return null;
            progress?.Report(80); // 進捗：すべてのデータチェック完了

            ImportResult result = null;
            if (method.Import(unitOfWork, worker) && RegisterAsync != null)
            {
                result = await RegisterAsync(unitOfWork);

                PostImportHanlder?.Invoke(worker, result);
                Debug.Assert(result != null,
                        $"{nameof(RegisterAsync)}()が{nameof(ImportResult)}を返しませんでした。");
            }
            else // 登録処理をしなかった、または取込可能件数がゼロ
            {
                result = new ImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InsertCount = 0,
                    UpdateCount = 0,
                    DeleteCount = 0,
                };
            }
            Errors.AddRange(worker.Reports);

            if (Errors.Any() && !string.IsNullOrWhiteSpace(ErrorLogPath))
            {
                OutputErrorLog(ErrorLogPath, Errors, csvPath);
            }
            result.ValidItemCount = worker.Models.Count;
            result.InvalidItemCount = worker.RecordCount - result.ValidItemCount;

            UnitOfWork = unitOfWork;
            progress?.Report(100); // 進捗：取込完了

            return result;
        }

        private bool ContainsNull(TIdentity identity)
        {
            Type idType = typeof(TIdentity);
            // 匿名クラスのNamespaceはNull
            if ((idType.Namespace ?? string.Empty).StartsWith(typeof(string).Namespace))
            {
                return identity == null || identity.Equals(default(TIdentity));
            }

            foreach (PropertyInfo prop in idType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                var propType = prop.PropertyType;

                if (propType.IsValueType
                        && !(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    // プロパティは非Nullableの値型
                    if (prop.GetValue(identity).Equals(Activator.CreateInstance(propType))) return true;
                }
                else
                {
                    // プロパティは参照型またはNullable
                    if (prop.GetValue(identity) == null) return true;
                }
            }
            return false;
        }

        private void OutputErrorLog(string path, List<WorkingReport> reports, string sourceFilePath)
        {
            var exists = File.Exists(path);
            using (var stream = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream, ErrorLogEncoding))
            {
                if (exists) writer.WriteLine();
                var now = DateTime.Now;

                var logs = GetErrorLogs(Path.GetFileName(sourceFilePath));
                foreach (var log in logs)
                    writer.WriteLine(log);
            }
        }

        public List<string> GetErrorLogs(string fileName = null)
        {
            var logs = new List<string>();
            var now = DateTime.Now;

            foreach (var report in Errors.Where(x => x.LineNo.HasValue).OrderBy(x => x.LineNo).ThenBy(x => x.FieldNo))
            {
                var fieldName = PadRightMultiByte(ErrorLogEncoding, report.FieldName, 30);
                logs.Add($"{report.LineNo + RowDef.StartLineNumber:D8}行目  {fieldName}{report.Message}");
            }
            foreach (var report in Errors.Where(x => !x.LineNo.HasValue))
                logs.Add(report.Message);
            if (logs.Any() && !string.IsNullOrEmpty(fileName))
            {
                logs.Insert(0, $"{RowDef.DataTypeToken}データ：{fileName}");
                logs.Insert(0, $"{now:yyyy年MM月dd日 HH時mm分ss秒}");
            }
            return logs;
        }

        public string FileName { get; set; }

        // 固定長マルチバイトのPadding
        private string PadRightMultiByte(Encoding encoding, string value, int length)
        {
            value = value ?? string.Empty;
            var byteCount = encoding.GetByteCount(value);
            if (length < byteCount)
            {
                value = new string(value
                        .TakeWhile((c, i) =>
                            encoding.GetByteCount(value.Substring(0, i + 1)) <= length)
                        .ToArray());
                byteCount = encoding.GetByteCount(value);
            }

            return value.PadRight(length - (byteCount - value.Length));
        }
    }

    public static class CsvImporterExtension
    {
        public static CsvImporter<TModel, TIdentity> CreateImporter<TModel, TIdentity>(this RowDefinition<TModel> def,
            Func<TModel, TIdentity> identitySelector, ICsvParser parser = null)
            where TModel : class, new()
        {
            var importer = new CsvImporter<TModel, TIdentity>(def, identitySelector);
            if (parser != null) importer.Parser = parser;
            return importer;
        }

    }
}
