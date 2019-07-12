using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class BankAccountTypeMaster : IBankAccountTypeMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBankAccountTypeProcessor bankAccountTypeProcessor;
        private readonly ILogger logger;

        public BankAccountTypeMaster(
            IAuthorizationProcessor authorizationProcessor,
            IBankAccountTypeProcessor bankAccountTypeProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.bankAccountTypeProcessor = bankAccountTypeProcessor;
            logger = logManager.GetLogger(typeof(BankAccountTypeMaster));
        }

        public async Task<BankAccountTypesResult> GetItemsAsync(string SessionKey)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async (token) => {
                var result = (await bankAccountTypeProcessor.GetAsync(token)).ToList();
                return new BankAccountTypesResult {
                    ProcessResult = new ProcessResult { Result = true },
                    BankAccountTypes = result,
                };
            }, logger);
    }
}
