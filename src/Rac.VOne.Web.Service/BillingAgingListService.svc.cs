using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Microsoft.AspNet.SignalR;
using NLog;

namespace Rac.VOne.Web.Service
{
    public class BillingAgingListService : IBillingAgingListService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBillingAgingListProcessor billingAgingListProcessor;
        private readonly ILogger logger;
        private readonly IHubContext hubContext;

        public BillingAgingListService(IAuthorizationProcessor authorizationProcessor,
            IBillingAgingListProcessor billingAgingListProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.billingAgingListProcessor = billingAgingListProcessor;

            logger = logManager.GetLogger(typeof(BillingAgingListService));
            hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        public async Task< BillingAgingListsResult > GetAsync(string SessionKey, BillingAgingListSearch searchOption, string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = await billingAgingListProcessor.GetAsync(searchOption, notifier, token);
                return new BillingAgingListsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingAgingLists = new List<BillingAgingList>(result),
                };
            }, logger, connectionId);
        }

        public async Task<BillingAgingListDetailsResult> GetDetailsAsync(string SessionKey, BillingAgingListSearch searchOption)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingAgingListProcessor.GetDetailsAsync(searchOption, token);
                return new BillingAgingListDetailsResult
                {
                    ProcessResult = new ProcessResult { Result = true, },
                    BillingAgingListDetails = new List<BillingAgingListDetail>(result),
                };
            }, logger);
        }
    }
}
