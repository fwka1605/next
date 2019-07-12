using Rac.VOne.Common.DataHandling;
using Rac.VOne.Import;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Import
{
    /// <summary>
    /// 入金部門 インポート処理
    /// </summary>
    public class SectionFileImportProcessor : ISectionFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ISectionProcessor sectionProcessor;

        /// <summary>constructor</summary>
        public SectionFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ISectionProcessor sectionProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.sectionProcessor = sectionProcessor;
        }

        /// <summary>インポート処理</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ImportResult> ImportAsync(MasterImportSource source, CancellationToken token = default(CancellationToken))
        {
            var mode = (ImportMethod)source.ImportMethod;
            var encoding = Encoding.GetEncoding(source.EncodingCodePage);
            var csv = encoding.GetString(source.Data);

            var companyTask = companyProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var loginUserTask = loginUserProcessor.GetAsync(new LoginUserSearch { Ids = new[] { source.LoginUserId }, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);
            var sectionTask = sectionProcessor.GetAsync(new SectionSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var sectionDictionary = sectionTask.Result.ToDictionary(x => x.Code);

            var definition = new SectionFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            var importer = definition.CreateImporter(x => x.Code, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;

            importer.AdditionalWorker = async worker => {
                var codes = worker.Models.Values.Select(x => x.Code).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();

                if (mode == ImportMethod.Replace)
                {
                    var bankAccountTask = sectionProcessor.GetImportItemsBankAccountAsync(source.CompanyId, codes, token);
                    var swdTask = sectionProcessor.GetImportItemsSectionWithDepartmentAsync(source.CompanyId, codes, token);
                    var swlTask = sectionProcessor.GetImportItemsSectionWithLoginUserAsync(source.CompanyId, codes, token);
                    var receiptTask = sectionProcessor.GetImportItemsReceiptAsync(source.CompanyId, codes, token);
                    var nettingTask = sectionProcessor.GetImportItemsNettingAsync(source.CompanyId, codes, token);

                    await Task.WhenAll(bankAccountTask, swdTask, swlTask, receiptTask, nettingTask);

                    (worker.RowDef as SectionFileDefinition).SectionCodeField.ValidateAdditional = (val, param) => {
                        var reports = new List<WorkingReport>();
                        reports.AddRange(bankAccountTask.Result.Select(x => new WorkingReport {
                            FieldNo     = definition.SectionCodeField.FieldIndex,
                            FieldName   = definition.SectionCodeField.FieldName,
                            Message     = $"銀行口座マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }).ToArray());
                        reports.AddRange(swdTask.Result.Select(x => new WorkingReport {
                            FieldNo     = definition.SectionCodeField.FieldIndex,
                            FieldName   = definition.SectionCodeField.FieldName,
                            Message     = $"入金・請求部門対応マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }).ToArray());
                        reports.AddRange(swlTask.Result.Select(x => new WorkingReport {
                            FieldNo     = definition.SectionCodeField.FieldIndex,
                            FieldName   = definition.SectionCodeField.FieldName,
                            Message     = $"入金部門・担当者対応マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }).ToArray());
                        reports.AddRange(receiptTask.Result.Select(x => new WorkingReport {
                            FieldNo     = definition.SectionCodeField.FieldIndex,
                            FieldName   = definition.SectionCodeField.FieldName,
                            Message     = $"入金データに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }).ToArray());
                        reports.AddRange(nettingTask.Result.Select(x => new WorkingReport {
                            FieldNo     = definition.SectionCodeField.FieldIndex,
                            FieldName   = definition.SectionCodeField.FieldName,
                            Message     = $"相殺データに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }).ToArray());
                        return reports;
                    };
                }

                var payerCodes = worker.Models.Values.Where(x => !string.IsNullOrEmpty(x.PayerCodeLeft) && !string.IsNullOrEmpty(x.PayerCodeRight))
                    .Select(x => x.PayerCodeLeft + x.PayerCodeRight).Distinct().ToArray();

                var sections = (await sectionProcessor.GetAsync(new SectionSearch { CompanyId = source.CompanyId, PayerCodes = payerCodes, }, token)).ToList();

                (worker.RowDef as SectionFileDefinition).PayerCodeLeftField.ValidateAdditional = (val, param) => {
                    var reports = new List<WorkingReport>();
                    var def = (worker.RowDef as SectionFileDefinition);
                    var uniqueKeys = new Dictionary<string, int>();
                    var duplicatedLines = new List<int>();

                    foreach (var pair in val)
                    {
                        var leftIsEmpty = string.IsNullOrEmpty(pair.Value.PayerCodeLeft);
                        var rightIsEmpty = string.IsNullOrEmpty(pair.Value.PayerCodeRight);
                        if (leftIsEmpty && rightIsEmpty) continue;

                        if (leftIsEmpty ^ rightIsEmpty)
                        {
                            var field = leftIsEmpty ? def.PayerCodeLeftField : def.PayerCodeRightField;
                            reports.Add(new WorkingReport {
                                LineNo      = pair.Key,
                                FieldNo     = field.FieldIndex,
                                FieldName   = field.FieldName,
                                Message     = "仮想支店コード・仮想口座番号のどちらかが未入力のため、インポートできません。",
                            });
                            continue;
                        }

                        var payerCode = pair.Value.PayerCodeLeft + pair.Value.PayerCodeRight;
                        if (mode == ImportMethod.InsertOnly)
                        {
                            if (sections.Any(x => x.PayerCode == payerCode))
                                reports.Add(new WorkingReport {
                                    LineNo      = pair.Key,
                                    FieldNo     = def.PayerCodeLeftField.FieldIndex,
                                    FieldName   = "仮想支店コード、仮想口座番号",
                                    Message     = "既に登録されている仮想支店コード、仮想口座番号のため、インポートできません。",
                                    Value       = payerCode,
                                });
                        }
                        else if (mode == ImportMethod.InsertAndUpdate)
                        {
                            // bug : 更新処理で、振込依頼人番号 の シャッフルができない
                            if (sections.Any(x => x.PayerCode == payerCode && x.Code != pair.Value.Code))
                                reports.Add(new WorkingReport {
                                    LineNo      = pair.Key,
                                    FieldNo     = def.PayerCodeLeftField.FieldIndex,
                                    FieldName   = "仮想支店コード、仮想口座番号",
                                    Message     = "既に登録されている仮想支店コード、仮想口座番号のため、インポートできません。",
                                    Value       = payerCode,
                                });
                        }
                        if (uniqueKeys.ContainsKey(payerCode))
                        {
                            var line = uniqueKeys[payerCode];
                            if (!duplicatedLines.Contains(line)) duplicatedLines.Add(line);
                            duplicatedLines.Add(pair.Key);
                        }
                        else
                            uniqueKeys.Add(payerCode, pair.Key);

                    }
                    foreach (var line in duplicatedLines)
                        reports.Add(new WorkingReport {
                            LineNo      = line,
                            FieldNo     = def.PayerCodeLeftField.FieldIndex,
                            FieldName   = "仮想支店コード、仮想口座番号",
                            Message     = "仮想支店コード、仮想口座番号が重複しているため、インポートできません。",
                            Value       = val[line].PayerCodeLeft + val[line].PayerCodeRight,
                        });
                    return reports;
                };
            };

            importer.LoadAsync = () => sectionProcessor.GetAsync(new SectionSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => sectionProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}