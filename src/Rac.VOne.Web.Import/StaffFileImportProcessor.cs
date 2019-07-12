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
    /// 営業担当者マスター インポート処理
    /// </summary>
    public class StaffFileImportProcessor : IStaffFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly IStaffProcessor staffProcessor;

        /// <summary>constructor</summary>
        public StaffFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IDepartmentProcessor departmentProcessor,
            IStaffProcessor staffProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.departmentProcessor = departmentProcessor;
            this.staffProcessor = staffProcessor;
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
            var loginUsersTask = loginUserProcessor.GetAsync(new LoginUserSearch{ CompanyId = source.CompanyId, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);
            var departmentTask = departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = source.CompanyId, }, token);
            var staffTask = staffProcessor.GetAsync(new StaffSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUsersTask, appConTask, departmentTask);

            var company = companyTask.Result.First();
            var loginUserDictionary = loginUsersTask.Result.ToDictionary(x => x.Code);
            var loginUser = loginUsersTask.Result.First(x => x.Id == source.LoginUserId);
            var appCon = appConTask.Result;
            var departmentDictionary = departmentTask.Result.ToDictionary(x => x.Code);

            var useDistribution = appCon.UseDistribution == 1;

            var definition = new StaffFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            definition.MailField.Required = useDistribution;

            definition.DepartmentIdField.GetModelsByCode = val => departmentDictionary;

            var importer = definition.CreateImporter(x => x.Code, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;

            if (mode == ImportMethod.Replace)
            {
                importer.AdditionalWorker = async worker => {
                    var codes = worker.Models.Values.Select(x => x.Code).Distinct().ToArray();
                    var userTask = staffProcessor.GetImportItemsLoginUserAsync(source.CompanyId, codes, token);
                    var custTask = staffProcessor.GetImportItemsCustomerAsync(source.CompanyId, codes, token);
                    var billTask = staffProcessor.GetImportItemsBillingAsync(source.CompanyId, codes, token);

                    await Task.WhenAll(userTask, custTask, billTask);

                    var def = worker.RowDef as StaffFileDefinition;

                    def.StaffCodeField.ValidateAdditional = (val, param) => {
                        var reports = new List<WorkingReport>();

                        reports.AddRange(userTask.Result.Select(x => new WorkingReport {
                            FieldNo     = def.StaffCodeField.FieldIndex,
                            FieldName   = def.StaffNameField.FieldName,
                            Message     = $"ログインユーザーマスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }));

                        reports.AddRange(custTask.Result.Select(x => new WorkingReport {
                            FieldNo     = def.StaffCodeField.FieldIndex,
                            FieldName   = def.StaffNameField.FieldName,
                            Message     = $"得意先マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }));

                        reports.AddRange(billTask.Result.Select(x => new WorkingReport {
                            FieldNo     = def.StaffCodeField.FieldIndex,
                            FieldName   = def.StaffNameField.FieldName,
                            Message     = $"請求データに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                        }));
                        return reports;
                    };

                };

            }

            importer.LoadAsync = () => staffProcessor.GetAsync(new StaffSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => staffProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}