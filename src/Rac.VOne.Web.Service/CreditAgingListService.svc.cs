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
using NLog;
using Microsoft.AspNet.SignalR;

namespace Rac.VOne.Web.Service
{
    public class CreditAgingListService : ICreditAgingListService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICreditAgingListProcessor creditAgingListProcessor;
        private readonly ILogger logger;
        private readonly IHubContext hubContext;

        public CreditAgingListService(IAuthorizationProcessor authorizationProcessor,
            ICreditAgingListProcessor creditAgingListProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.creditAgingListProcessor = creditAgingListProcessor;
            logger = logManager.GetLogger(typeof(CreditAgingListSearch));
            hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        public async Task<CreditAgingListsResult> GetAsync(string SessionKey, CreditAgingListSearch searchOption, string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = await creditAgingListProcessor.GetAsync(searchOption, notifier, token);
                return new CreditAgingListsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    CreditAgingLists = new List<CreditAgingList>(result),
                };
            }, logger, connectionId);
        }
    }
}
