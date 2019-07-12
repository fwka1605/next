using Rac.VOne.Common.DataHandling;
using Rac.VOne.Import;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Import
{
    /// <summary>
    /// ログインユーザー インポート処理
    /// </summary>
    public class LoginUserFileImportProcessor : ILoginUserFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly IStaffProcessor staffProcessor;
        private readonly IPasswordPolicyProcessor passwordPolicyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;


        /// <summary>constructor</summary>
        public LoginUserFileImportProcessor(
            ICompanyProcessor companyProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IDepartmentProcessor departmentProcessor,
            IStaffProcessor staffProcessor,
            IPasswordPolicyProcessor passwordPolicyProcessor,
            ILoginUserProcessor loginUserProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.departmentProcessor = departmentProcessor;
            this.staffProcessor = staffProcessor;
            this.passwordPolicyProcessor = passwordPolicyProcessor;
            this.loginUserProcessor = loginUserProcessor;
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
            var departmentTask = departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = source.CompanyId, }, token);
            var staffTask = staffProcessor.GetAsync(new StaffSearch { CompanyId = source.CompanyId, }, token);
            var policyTask = passwordPolicyProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask, departmentTask, staffTask, policyTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var deptDic = departmentTask.Result.ToDictionary(x => x.Code);
            var stffDic = staffTask.Result.ToDictionary(x => x.Code);
            var policy = policyTask.Result;

            var definition = new LoginUserFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            var useDistribution = appCon.UseDistribution == 1;

            definition.MailField.Required =  useDistribution;
            definition.UseClientField.Ignored = !useDistribution;
            definition.UseWebViewerField.Ignored = !useDistribution;

            definition.DepartmentCodeField.GetModelsByCode = val => deptDic;
            definition.StaffCodeField.GetModelsByCode = val => stffDic;

            var importer = definition.CreateImporter(x => x.Code, parser);

            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;
            importer.LoadAsync = () => loginUserProcessor.GetAsync(new LoginUserSearch { CompanyId = source.CompanyId, }, token);

            importer.AdditionalWorker = async worker => {
                var codes = worker.Models.Values.Select(x => x.Code).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
                var def = worker.RowDef as LoginUserFileDefinition;

                if (mode == ImportMethod.Replace)
                {
                    var sectionResult = await loginUserProcessor.GetAsync(new LoginUserSearch { CompanyId = source.CompanyId, ExcludeCodes = codes, }, token);
                    def.LoginUserCodeField.ValidateAdditional = (val, param) => {
                        var reports = new List<WorkingReport>();
                        reports.AddRange(sectionResult.Select(x => new WorkingReport {
                            FieldNo     = definition.DepartmentCodeField.FieldIndex,
                            FieldName   = definition.DepartmentCodeField.FieldName,
                            Message     = $"入金部門・担当者対応マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }).ToArray());

                        return reports;
                    };
                }

                var dbCodes = (await loginUserProcessor.GetAsync(new LoginUserSearch { CompanyId = source.CompanyId, Codes = codes, }, token)).Select(x => x.Code);
                def.InitialPasswordField.ValidateAdditional = (val, param) => {
                    var reports = new List<WorkingReport>();

                    var csvCodes = val.Values.Select(u => u.Code).ToArray();

                    var targetCodes = csvCodes.Except(dbCodes); // 検証対象(＝新規登録ユーザ)

                    var addError = new Action<int, string, string>((lineNo, value, message) =>
                    {
                        reports.Add(new WorkingReport {
                            LineNo      = lineNo,
                            FieldNo     = definition.InitialPasswordField.FieldIndex,
                            FieldName   = definition.InitialPasswordField.FieldName,
                            Value       = value,
                            Message     = message,
                        });
                    });

                    foreach (var lineNo in val.Keys.Where(lineNo => targetCodes.Contains(val[lineNo].Code)))
                    {
                        var password = val[lineNo].InitialPassword;

                        if (string.IsNullOrEmpty(password))
                        {
                            addError(lineNo, password, $"新規登録ユーザの初回パスワードが空白のため、インポートできません。");
                            continue;
                        }

                        var validationResult = policy.Validate(password);
                        if (validationResult != PasswordValidateResult.Valid)
                        {
                            switch (validationResult)
                            {
                                case PasswordValidateResult.ProhibitionAlphabetChar:
                                    addError(lineNo, password, $"アルファベットが使用されているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ProhibitionNumberChar:
                                    addError(lineNo, password, $"数字が使用されているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ProhibitionSymbolChar:
                                    addError(lineNo, password, $"記号が使用されているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ProhibitionNotAllowedSymbolChar:
                                    addError(lineNo, password, $"使用できない文字が含まれているため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortageAlphabetCharCount:
                                    addError(lineNo, password, $"アルファベットが最低{policy.MinAlphabetUseCount}文字含まれていないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortageNumberCharCount:
                                    addError(lineNo, password, $"数字が最低{policy.MinNumberUseCount}文字含まれていないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortageSymbolCharCount:
                                    addError(lineNo, password, $"記号が最低{policy.MinSymbolUseCount}文字含まれていないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ShortagePasswordLength:
                                    addError(lineNo, password, $"{policy.MinLength}文字以上でないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ExceedPasswordLength:
                                    addError(lineNo, password, $"{policy.MaxLength}文字以下でないため、インポートできません。");
                                    break;
                                case PasswordValidateResult.ExceedSameRepeatedChar:
                                    addError(lineNo, password, $"同じ文字が{policy.MinSameCharacterRepeat}文字以上続いているため、インポートできません。");
                                    break;
                                default:
                                    throw new NotImplementedException($"PasswordValidateResult = {validationResult.ToString()}");
                            }
                        }
                    }

                    return reports;
                };
            };

            importer.RegisterAsync = x => loginUserProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}