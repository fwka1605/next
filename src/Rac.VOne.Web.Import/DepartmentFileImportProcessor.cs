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
    /// 請求部門 インポートの実装
    /// </summary>
    public class DepartmentFileImportProcessor : IDepartmentFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IStaffProcessor staffProcessor;
        private readonly IDepartmentProcessor departmentProcessor;

        /// <summary>constructor</summary>
        public DepartmentFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IStaffProcessor staffProcessor,
            IDepartmentProcessor departmentProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.staffProcessor = staffProcessor;
            this.departmentProcessor = departmentProcessor;
        }

        /// <summary>
        /// インポート処理本体
        /// </summary>
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
            var staffsTask = staffProcessor.GetAsync(new StaffSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUserTask, appConTask, staffsTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var appCon = appConTask.Result;
            var staffDictionary = staffsTask.Result.ToDictionary(x => x.Code);

            var definition = new DepartmentFileDefinition(new DataExpression(appCon));
            definition.StaffIdField.GetModelsByCode = codes => staffDictionary;
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };
            var importer = definition.CreateImporter(x => x.Code, parser);
            importer.UserId         = source.LoginUserId;
            importer.UserCode       = loginUser.Code;
            importer.CompanyId      = source.CompanyId;
            importer.CompanyCode    = company.Code;
            if (mode == ImportMethod.Replace) importer.AdditionalWorker = async worker => {
                var codes = worker.Models.Values.Select(x => x.Code).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();

                var staffItemTask = departmentProcessor.GetImportItemsStaffAsync(source.CompanyId, codes, token);
                var swdItemTask = departmentProcessor.GetImportItemsSectionWithDepartmentAsync(source.CompanyId, codes, token);
                var billItemTask = departmentProcessor.GetImportItemsBillingAsync(source.CompanyId, codes, token);

                await Task.WhenAll(staffItemTask, swdItemTask, billItemTask);

                var staffResult = staffItemTask.Result.ToArray();
                var swdResult   = swdItemTask.Result.ToArray();
                var billResult  = billItemTask.Result.ToArray();

                (worker.RowDef as DepartmentFileDefinition).DepartmentCodeField.ValidateAdditional = (val, param) => {
                    var reports = new List<WorkingReport>();
                    reports.AddRange(staffResult.Select(x => new WorkingReport {
                        FieldNo     = definition.DepartmentCodeField.FieldIndex,
                        FieldName   = definition.DepartmentCodeField.FieldName,
                        Message     = $"営業担当者マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                    }).ToArray());
                    reports.AddRange(swdResult.Select(x => new WorkingReport {
                        FieldNo     = definition.DepartmentCodeField.FieldIndex,
                        FieldName   = definition.DepartmentCodeField.FieldName,
                        Message     = $"入金・請求部門名対応マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                    }).ToArray());
                    reports.AddRange(billResult.Select(x => new WorkingReport {
                        FieldNo     = definition.DepartmentCodeField.FieldIndex,
                        FieldName   = definition.DepartmentCodeField.FieldName,
                        Message     = $"請求データーに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                    }).ToArray());
                    return reports;
                };
            };
            importer.LoadAsync = () => departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => departmentProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}