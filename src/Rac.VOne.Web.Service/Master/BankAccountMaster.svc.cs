using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class BankAccountMaster : IBankAccountMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBankAccountProcessor bankAccountProcessor;
        private readonly ILogger logger;

        public BankAccountMaster(
            IAuthorizationProcessor authorizationProcessor,
            IBankAccountProcessor bankAccountProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.bankAccountProcessor = bankAccountProcessor;
            logger = logManager.GetLogger(typeof(BankAccountMaster));
        }

        public async Task<BankAccountsResult> GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await bankAccountProcessor.GetAsync(new BankAccountSearch {
                    Ids     = Id,
                })).ToList();

                return new BankAccountsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankAccounts = result,
                };
            }, logger);
        }

        public async Task<BankAccountResult>  GetByCodeAsync(string SessionKey, int CompanyId, string BankCode,
            string BranchCode, int AccountTypeId, string AccountNumber)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await bankAccountProcessor.GetAsync(new BankAccountSearch {
                    CompanyId       = CompanyId,
                    BankCodes       = new[] { BankCode },
                    BranchCodes     = new[] { BranchCode },
                    AccountTypeId   = AccountTypeId,
                    AccountNumber   = AccountNumber,
                }, token)).FirstOrDefault();

                return new BankAccountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankAccount = result,
                };
            }, logger);
        }

        public async Task<BankAccountResult> GetByBranchNameAsync(string SessionKey, int CompanyId, string BankCode,
            string BranchName, int AccountTypeId, string AccountNumber)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await bankAccountProcessor.GetAsync(new BankAccountSearch {
                    CompanyId       = CompanyId,
                    BankCodes       = new[] { BankCode },
                    BranchName      = BranchName,
                    AccountTypeId   = AccountTypeId,
                    AccountNumber   = AccountNumber,
                }, token)).FirstOrDefault();

                return new BankAccountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankAccount = result,
                };
            }, logger);
        }

        public async Task<BankAccountsResult> GetItemsAsync(string SessionKey, int CompanyId, BankAccountSearch BankAccountSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await bankAccountProcessor.GetAsync(BankAccountSearch, token)).ToList();

                return new BankAccountsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankAccounts = result,
                };
            }, logger);
        }

        public async Task<BankAccountResult> SaveAsync(string SessionKey, BankAccount BankAccount)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await bankAccountProcessor.SaveAsync(BankAccount, token);
                return new BankAccountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankAccount = result,
                };

            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await bankAccountProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await bankAccountProcessor.ExistCategoryAsync(CategoryId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await bankAccountProcessor.ExistSectionAsync(SectionId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey,
              BankAccount[] InsertList, BankAccount[] UpdateList, BankAccount[] DeleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await bankAccountProcessor.ImportAsync(InsertList, UpdateList, DeleteList, token);
                return result;
            }, logger);
        }
    }
}
