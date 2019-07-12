using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class BankBranchMaster : IBankBranchMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBankBranchProcessor bankBranchProcessor;
        private readonly ILogger logger;

        public BankBranchMaster(
            IAuthorizationProcessor authorizationProcessor,
            IBankBranchProcessor bankBranchProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.bankBranchProcessor = bankBranchProcessor;
            logger = logManager.GetLogger(typeof(BankBranchMaster));
        }

        public async Task<BankBranchResult> GetAsync(string Sessionkey, int CompanyId, string BankCode, string BranchCode)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                var result = (await  bankBranchProcessor.GetAsync(new BankBranchSearch {
                    CompanyId   = CompanyId,
                    BankCodes   = new[] { BankCode },
                    BranchCodes = new[] { BranchCode },
                }, token)).FirstOrDefault();
                return new BankBranchResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankBranch = result,
                };
            }, logger);
        }

        public async Task<BankBranchsResult> GetItemsAsync(string Sessionkey, int CompanyId, BankBranchSearch BankBranchSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                BankBranchSearch.CompanyId = CompanyId;
                var result = (await bankBranchProcessor.GetAsync(BankBranchSearch)).ToList();
                return new BankBranchsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankBranches = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string Sessionkey, int CompanyId, string BankCode, string BranchCode)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                var result = await bankBranchProcessor.DeleteAsync(CompanyId, BankCode, BranchCode, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<BankBranchResult> SaveAsync(string Sessionkey, BankBranch BankBranch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(Sessionkey, async token =>
            {
                var result = await bankBranchProcessor.SaveAsync(BankBranch, token);
                return new BankBranchResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BankBranch = result,
                };
            }, logger);
        }

        public async Task<ImportResult> ImportAsync(string SessionKey, BankBranch[] insertList, BankBranch[] updateList, BankBranch[] deleteList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await bankBranchProcessor.ImportAsync(insertList, updateList, deleteList, token);
                return result;
            }, logger);
        }
    }
}