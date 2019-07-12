using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Microsoft.AspNet.SignalR;
using NLog;

namespace Rac.VOne.Web.Service
{
    public class CustomerLedgerService : ICustomerLedgerService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICustomerLedgerProcessor customerLedgerProcessor;
        private readonly ILogger logger;
        private readonly IHubContext hubContext;

        public CustomerLedgerService(
            IAuthorizationProcessor authorizationProcessor,
            ICustomerLedgerProcessor customerLedgerProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.customerLedgerProcessor = customerLedgerProcessor;
            logger = logManager.GetLogger(typeof(CustomerLedgerService));
            hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        public async Task<CustomerLedgersResult> GetAsync(string SessionKey, CustomerLedgerSearch SearchOption, string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = await customerLedgerProcessor.GetAsync(SearchOption, token, notifier);
                return new CustomerLedgersResult
                {
                    ProcessResult = new ProcessResult { Result = true, },
                    CustomerLedgers = new List<CustomerLedger>(result),
                };
            }, logger, connectionId);
        }
    }
}
