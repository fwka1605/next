using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class CurrencyMaster : ICurrencyMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICurrencyProcessor currencyProcessor;
        private readonly ILogger logger;

        public CurrencyMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICurrencyProcessor currencyProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.currencyProcessor = currencyProcessor;
            logger = logManager.GetLogger(typeof(CurrencyMaster));
        }

        public async Task<CurrenciesResult> GetAsync(string SessionKey, int[] CurrencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await currencyProcessor.GetAsync(new CurrencySearch { Ids = CurrencyId }, token)).ToList();
                return new CurrenciesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Currencies = result,
                };
            }, logger);
        }

        public async Task<CurrenciesResult> GetItemsAsync(string SessionKey, int CompanyId, CurrencySearch CurrencySearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                CurrencySearch.CompanyId = CompanyId;
                var result = (await currencyProcessor.GetAsync(CurrencySearch, token)).ToList();
                return new CurrenciesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Currencies = result,
                };
            }, logger);
        }

        public async Task<CurrenciesResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await currencyProcessor.GetAsync(new CurrencySearch {
                    CompanyId   = CompanyId,
                    Codes       = Code,
                }, token)).ToList();

                return new CurrenciesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Currencies = result,
                };
            }, logger);
        }

        public async Task<CurrencyResult> SaveAsync(string SessionKey, Currency Currency)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await currencyProcessor.SaveAsync(Currency, token);
                return new CurrencyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Currency = result ,
                };

            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await currencyProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey,
              Currency[] InsertList, Currency[] UpdateList, Currency[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await currencyProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
                return result;
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsBillingAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await currencyProcessor.GetImportItemsBillingAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsReceiptAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await currencyProcessor.GetImportItemsReceiptAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsNettingAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await currencyProcessor.GetImportItemsNettingAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };

            }, logger);
        }
    }
}
