using NLog;
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

namespace Rac.VOne.Web.Service.Master
{
    public class BillingDivisionSettingMaster :
        IBillingDivisionSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBillingDivisionSettingProcessor billingDivisionSettingProcessor;
        private readonly ILogger logger;

        public BillingDivisionSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IBillingDivisionSettingProcessor billingDivisionSettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.billingDivisionSettingProcessor = billingDivisionSettingProcessor;
            logger = logManager.GetLogger(typeof(BillingDivisionSettingMaster));
        }

        public async Task<BillingDivisionSettingResult> GetAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingDivisionSettingProcessor.GetAsync(CompanyId, token);
                return new BillingDivisionSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingDivisionSetting = result,
                };
            }, logger);
        }
    }
}
