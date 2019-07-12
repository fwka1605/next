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
    /// 得意先歩引きマスター インポート処理
    /// </summary>
    public class CustomerDiscountFileImportProcessor : ICustomerDiscountFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly IAccountTitleProcessor accountTitleProcessor;
        private readonly ICustomerDiscountProcessor customerDiscountProcessor;

        /// <summary>constructor</summary>
        public CustomerDiscountFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ICustomerProcessor customerProcessor,
            IDepartmentProcessor departmentProcessor,
            IAccountTitleProcessor accountTitleProcessor,
            ICustomerDiscountProcessor customerDiscountProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.customerProcessor = customerProcessor;
            this.departmentProcessor = departmentProcessor;
            this.accountTitleProcessor = accountTitleProcessor;
            this.customerDiscountProcessor = customerDiscountProcessor;
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
            var loginUsersTask = loginUserProcessor.GetAsync(new LoginUserSearch { CompanyId = source.CompanyId, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);
            var customerTask = customerProcessor.GetAsync(new CustomerSearch { CompanyId = source.CompanyId }, token);
            var departmentTask = departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = source.CompanyId, }, token);
            var accountTitleTask = accountTitleProcessor.GetAsync(new AccountTitleSearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUsersTask, appConTask, customerTask, departmentTask, accountTitleTask);

            var company = companyTask.Result.First();
            var loginUserDictionary = loginUsersTask.Result.ToDictionary(x => x.Code);
            var loginUser = loginUsersTask.Result.First(x => x.Id == source.LoginUserId);
            var appCon = appConTask.Result;
            var customerDictionary = customerTask.Result.ToDictionary(x => x.Code);
            var departmentDictionary = departmentTask.Result.ToDictionary(x => x.Code);
            var accountTitleDictionary = accountTitleTask.Result.ToDictionary(x => x.Code);

            var definition = new CustomerDiscountFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };

            definition.CustomerIdField.GetModelsByCode = val => customerDictionary;
            definition.DepartmentId1Field.GetModelsByCode = val => departmentDictionary;
            definition.DepartmentId2Field.GetModelsByCode = val => departmentDictionary;
            definition.DepartmentId3Field.GetModelsByCode = val => departmentDictionary;
            definition.DepartmentId4Field.GetModelsByCode = val => departmentDictionary;
            definition.DepartmentId5Field.GetModelsByCode = val => departmentDictionary;
            definition.AccountTitleId1Field.GetModelsByCode = val => accountTitleDictionary;
            definition.AccountTitleId2Field.GetModelsByCode = val => accountTitleDictionary;
            definition.AccountTitleId3Field.GetModelsByCode = val => accountTitleDictionary;
            definition.AccountTitleId4Field.GetModelsByCode = val => accountTitleDictionary;
            definition.AccountTitleId5Field.GetModelsByCode = val => accountTitleDictionary;
            definition.Rate1Field.ValidateAdditional = (val, param) => ValidateRate(val, param, definition.Rate1Field);
            definition.Rate2Field.ValidateAdditional = (val, param) => ValidateRate(val, param, definition.Rate2Field);
            definition.Rate3Field.ValidateAdditional = (val, param) => ValidateRate(val, param, definition.Rate3Field);
            definition.Rate4Field.ValidateAdditional = (val, param) => ValidateRate(val, param, definition.Rate4Field);
            definition.Rate5Field.ValidateAdditional = (val, param) => ValidateRate(val, param, definition.Rate5Field);

            var importer = definition.CreateImporter(x => x.CustomerCode, parser);
            importer.UserId = source.LoginUserId;
            importer.UserCode = loginUser.Code;
            importer.CompanyId = source.CompanyId;
            importer.CompanyCode = company.Code;

            importer.LoadAsync = () => customerDiscountProcessor.GetItemsAsync(new CustomerSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => customerDiscountProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }

        private List<WorkingReport> ValidateRate(Dictionary<int, CustomerDiscount> val, object param, FieldDefinition<CustomerDiscount, decimal> field)
        {
            var reports = new List<WorkingReport>();
            foreach (var pair in val)
            {
                var amount
                    = pair.Value.Rate1 ?? 0M
                    + pair.Value.Rate2 ?? 0M
                    + pair.Value.Rate3 ?? 0M
                    + pair.Value.Rate4 ?? 0M
                    + pair.Value.Rate5 ?? 0M
                    ;
                if (100M < amount) reports.Add(new WorkingReport {
                    LineNo      = pair.Key,
                    FieldNo     = field.FieldIndex,
                    FieldName   = field.FieldName,
                    Message     = "歩引率の合計が100を超えるためインポートできません。",
                });
            }
            return reports;
        }
    }
}