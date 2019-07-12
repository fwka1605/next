using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class AccountTitleMaster : IAccountTitleMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IAccountTitleProcessor accountTitleProcessor;
        private readonly ILogger logger;

        public AccountTitleMaster(
            IAuthorizationProcessor authorizationProcessor,
            IAccountTitleProcessor accountTitleProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.accountTitleProcessor = accountTitleProcessor;
            logger = logManager.GetLogger(typeof(AccountTitleMaster));
        }

        public async Task<AccountTitlesResult> GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accountTitleProcessor.GetAsync(new AccountTitleSearch { Ids = Id }, token)).ToList();
                return new AccountTitlesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AccountTitles = result,
                };
            }, logger);
        }

        public async Task<AccountTitlesResult> GetItemsAsync(string SessionKey, AccountTitleSearch searchOption)
        {
            return await  authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accountTitleProcessor.GetAsync(searchOption, token)).ToList();
                return new AccountTitlesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AccountTitles = result,
                };
            }, logger);
        }

        public async Task<AccountTitlesResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accountTitleProcessor.GetAsync(new AccountTitleSearch {
                    CompanyId   = CompanyId,
                    Codes       = Code,
                }, token)).ToList();
                return new AccountTitlesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AccountTitles = result,
                };
            }, logger);
        }

        public async Task<AccountTitleResult> SaveAsync(string SessionKey, AccountTitle AccountTitle)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await accountTitleProcessor.SaveAsync(AccountTitle, token);
                return new AccountTitleResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AccountTitle = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int AccountTitleId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await accountTitleProcessor.DeleteAsync(AccountTitleId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ImportResultAccountTitle> ImportAsync(string SessionKey,
                AccountTitle[] InsertList, AccountTitle[] UpdateList, AccountTitle[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return (await accountTitleProcessor.ImportAsync(InsertList, UpdateList, DeleteList)) as ImportResultAccountTitle;
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForCategoryAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accountTitleProcessor.GetImportItemsCategoryAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForCustomerDiscountAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accountTitleProcessor.GetImportItemsCustomerDiscountAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForDebitBillingAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accountTitleProcessor.GetImportItemsDebitBillingAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }

        public async Task<MasterDatasResult> GetImportItemsForCreditBillingAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accountTitleProcessor.GetImportItemsCreditBillingAsync(CompanyId, Code, token)).ToList();
                return new MasterDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MasterDatas = result,
                };
            }, logger);
        }
    }
}
