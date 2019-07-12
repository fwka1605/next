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
    public class CollectionScheduleService : ICollectionScheduleService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICollectionScheduleProcessor collectionScheduleProcessor;
        private readonly ILogger logger;
        private readonly IHubContext hubContext;

        public CollectionScheduleService(IAuthorizationProcessor authorizationProcessor,
            ICollectionScheduleProcessor collectionScheduleProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.collectionScheduleProcessor = collectionScheduleProcessor;
            logger = logManager.GetLogger(typeof(CollectionScheduleService));
            hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        public async Task< CollectionSchedulesResult> GetAsync(string SessionKey, CollectionScheduleSearch SearchOption, string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = await collectionScheduleProcessor.GetAsync(SearchOption, token, notifier);
                return new CollectionSchedulesResult
                {
                    ProcessResult = new ProcessResult { Result = true, },
                    CollectionSchedules = new List<CollectionSchedule>(result),
                };
            }, logger, connectionId);
        }
    }
}
