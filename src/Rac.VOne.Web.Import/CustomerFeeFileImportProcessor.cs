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
    /// 得意先手数料 インポート処理
    /// </summary>
    public class CustomerFeeFileImportProcessor : ICustomerFeeFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly ICurrencyProcessor currencyProcessor;
        private readonly ICustomerFeeProcessor customerFeeProcessor;

        /// <summary>constructor</summary>
        public CustomerFeeFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ICustomerProcessor customerProcessor,
            ICurrencyProcessor currencyProcessor,
            ICustomerFeeProcessor customerFeeProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.customerProcessor = customerProcessor;
            this.currencyProcessor = currencyProcessor;
            this.customerFeeProcessor = customerFeeProcessor;
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
            var currencyTask = currencyProcessor.GetAsync(new CurrencySearch { CompanyId = source.CompanyId, }, token);

            await Task.WhenAll(companyTask, loginUsersTask, appConTask, customerTask, currencyTask);

            var company = companyTask.Result.First();
            var loginUserDictionary = loginUsersTask.Result.ToDictionary(x => x.Code);
            var loginUser = loginUsersTask.Result.First(x => x.Id == source.LoginUserId);
            var appCon = appConTask.Result;
            var customerDictionary = customerTask.Result.ToDictionary(x => x.Code);
            var currencyDictionary = currencyTask.Result.ToDictionary(x => x.Code);

            var useForeignCurrency = appCon.UseForeignCurrency == 1;

            var definition = new CustomerRegistrationFeeFileDefinition(new DataExpression(appCon));
            var parser = new CsvParser {
                Encoding        = encoding,
                StreamCreator   = new PlainTextMemoryStreamCreator(),
            };


            definition.CurrencyCodeField.Ignored = !useForeignCurrency;
            definition.CustomerIdField.GetModelsByCode = val => customerDictionary;
            definition.CurrencyCodeField.GetModelsByCode = code => currencyDictionary;

            var importer = definition.CreateImporter(x => new { x.CustomerCode, x.CurrencyCode, x.Fee }, parser);
            importer.UserId = source.LoginUserId;
            importer.UserCode = loginUser.Code;
            importer.CompanyId = source.CompanyId;
            importer.CompanyCode = company.Code;

            importer.LoadAsync = () => customerFeeProcessor.GetAsync(new CustomerFeeSearch { CompanyId = source.CompanyId, }, token);
            importer.RegisterAsync = x => customerFeeProcessor.ImportAsync(x.New, x.Dirty, x.Removed, token);

            var result = await importer.ImportAsync(csv, mode, token, null);
            result.Logs = importer.GetErrorLogs();

            return result;
        }
    }
}