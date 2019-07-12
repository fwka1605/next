using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Common.Logging;
using NLog;
using Rac.VOne.Web.Service.Extensions;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class PeriodicBillingSettingMaster : IPeriodicBillingSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IPeriodicBillingSettingProcessor periodicBillingSettingProcessor;
        private readonly ILogger logger;

        public PeriodicBillingSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IPeriodicBillingSettingProcessor periodicBillingSettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.periodicBillingSettingProcessor = periodicBillingSettingProcessor;
            logger = logManager.GetLogger(typeof(PeriodicBillingSettingMaster));
        }

        public async Task<CountResult> DeleteAsync(string sessionKey, long id)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var result = await periodicBillingSettingProcessor.DeleteAsync(id, token);
                return new CountResult {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);

        public async Task<PeriodicBillingSettingsResult> GetItemsAsync(string sessionKey, PeriodicBillingSettingSearch option)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var result = (await periodicBillingSettingProcessor.GetAsync(option, token)).ToList();
                return new PeriodicBillingSettingsResult {
                    ProcessResult = new ProcessResult { Result = true },
                    PeriodicBillingSettings = result,
                };
            }, logger);

        public async Task<PeriodicBillingSettingResult> SaveAsync(string sessionKey, PeriodicBillingSetting setting)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var result = await periodicBillingSettingProcessor.SaveAsync(setting, token);
                return new PeriodicBillingSettingResult {
                    ProcessResult = new ProcessResult { Result = true },
                    PeriodicBillingSetting = result,
                };
            }, logger);
    }
}
