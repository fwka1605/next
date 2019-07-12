using NLog;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{

    public class AccountTransferService : IAccountTransferService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IAccountTransferProcessor accounttransferProcessor;
        private readonly ILogger logger;

        public AccountTransferService(
            IAuthorizationProcessor authorizationProcessor,
            IAccountTransferProcessor accounttransferProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.accounttransferProcessor = accounttransferProcessor;
            logger = logManager.GetLogger(typeof(AccountTransferService));
        }

        public async Task<CountResult> CancelAsync(string SessionKey, long[] AccountTransferLogIds, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await accounttransferProcessor.CancelAsync(
                    AccountTransferLogIds.Select(x => new AccountTransferLog {
                        Id          = x,
                        CreateBy    = LoginUserId,
                    }).ToArray(), token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<AccountTransferDetailsResult> ExtractAsync(string SessionKey,
            AccountTransferSearch SearchOption)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accounttransferProcessor.ExtractAsync(SearchOption, token)).ToList();
                return new AccountTransferDetailsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AccountTransferDetail = result,
                };
            }, logger);
        }

        public async Task<AccountTransferLogsResult> GetAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await accounttransferProcessor.GetAsync(CompanyId, token)).ToList();
                return new AccountTransferLogsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AccountTransferLog = result,
                };
            }, logger);
        }

        public async Task<AccountTransferDetailsResult> SaveAsync(string SessionKey,
            AccountTransferDetail[] AccountTransferDetails, bool Aggregate)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                foreach (var detail in AccountTransferDetails)
                    detail.Aggregate = Aggregate;
                var result = (await accounttransferProcessor.SaveAsync(AccountTransferDetails, token)).ToList();
                return new AccountTransferDetailsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    AccountTransferDetail = result,
                };
            }, logger);

        }
    }
}
