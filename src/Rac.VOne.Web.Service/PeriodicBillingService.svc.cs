using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class PeriodicBillingService : IPeriodicBillingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IPeriodicBillingProcesssor periodicBillingProcesssor;
        private readonly ILogger logger;

        public PeriodicBillingService(
            IAuthorizationProcessor authorizationProcessor,
            IPeriodicBillingProcesssor periodicBillingProcesssor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.periodicBillingProcesssor = periodicBillingProcesssor;
            logger = logManager.GetLogger(typeof(PeriodicBillingService));
        }

        public async Task<BillingsResult> CreateAsync(string sessionKey, IEnumerable<PeriodicBillingSetting> settings)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var result = (await periodicBillingProcesssor.CreateAsync(settings, token)).ToList();
                return new BillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = result,
                };
            }, logger);
    }
}
