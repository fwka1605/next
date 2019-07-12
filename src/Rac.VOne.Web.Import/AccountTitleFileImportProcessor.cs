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
using System.Web;

namespace Rac.VOne.Web.Import
{
    /// <summary>
    /// 科目マスターインポート処理
    /// </summary>
    public class AccountTitleFileImportProcessor : IAccountTitleFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IAccountTitleProcessor accountTitleProcessor;
        /// <summary>constructor</summary>
        public AccountTitleFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IAccountTitleProcessor accountTitleProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.accountTitleProcessor = accountTitleProcessor;
        }

        /// <summary>
        /// インポート処理
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

            await Task.WhenAll(companyTask, loginUserTask, appConTask);

            var company = companyTask.Result.First();
            var loginUser = loginUserTask.Result.First();
            var applicationControl = appConTask.Result;
            var definition = new AccountTitleFileDefinition(new DataExpression(applicationControl));

            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            var importer = definition.CreateImporter(m => m.Code, parser);
            importer.UserId = source.LoginUserId;
            importer.UserCode = loginUser.Code;
            importer.CompanyId = source.CompanyId;
            importer.CompanyCode = company.Code;
            if (mode == ImportMethod.Replace) importer.AdditionalWorker = async worker => {
                var codes = worker.Models.Values.Select(x => x.Code).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
                var categoryItemTask = accountTitleProcessor.GetImportItemsCategoryAsync(source.CompanyId, codes, token);
                var customerItemTask = accountTitleProcessor.GetImportItemsCustomerDiscountAsync(source.CompanyId, codes, token);
                var debitBillingItemTask = accountTitleProcessor.GetImportItemsDebitBillingAsync(source.CompanyId, codes, token);
                var creditBillingItemTask = accountTitleProcessor.GetImportItemsCreditBillingAsync(source.CompanyId, codes, token);

                await Task.WhenAll(categoryItemTask, customerItemTask, debitBillingItemTask, creditBillingItemTask);


                (worker.RowDef as AccountTitleFileDefinition).AccountTitleCodeField.ValidateAdditional = (val, param) => {
                    var reports = new List<WorkingReport>();
                    reports.AddRange(categoryItemTask.Result.Select(x => new WorkingReport
                    {
                        FieldNo = definition.AccountTitleCodeField.FieldIndex,
                        FieldName = definition.AccountTitleCodeField.FieldName,
                        Message = $"区分マスターに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                    }).ToArray());

                    reports.AddRange(customerItemTask.Result.Select(x => new WorkingReport
                    {
                        FieldNo = definition.AccountTitleCodeField.FieldIndex,
                        FieldName = definition.AccountTitleCodeField.FieldName,
                        Message = $"得意先マスター歩引設定に存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                    }).ToArray());

                    reports.AddRange(debitBillingItemTask.Result.Select(x => new WorkingReport
                    {
                        FieldNo = definition.AccountTitleCodeField.FieldIndex,
                        FieldName = definition.AccountTitleCodeField.FieldName,
                        Message = $"請求データに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                    }).ToArray());

                    reports.AddRange(creditBillingItemTask.Result.Select(x => new WorkingReport
                    {
                        FieldNo = definition.AccountTitleCodeField.FieldIndex,
                        FieldName = definition.AccountTitleCodeField.FieldName,
                        Message = $"請求データに存在する{x.Code}：{x.Name}が存在しないため、インポートできません。",
                    }).ToArray());

                    return reports;
                };
            };

            importer.LoadAsync = () => accountTitleProcessor.GetAsync(new AccountTitleSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => accountTitleProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}