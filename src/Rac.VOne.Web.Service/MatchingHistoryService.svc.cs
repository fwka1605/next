using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using Microsoft.AspNet.SignalR;
using NLog;

namespace Rac.VOne.Web.Service
{
    public class MatchingHistoryService : IMatchingHistoryService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IMatchingHistorySearchProcessor matchingHistorySearchProcessor;
        private readonly IMatchingOutputedProcessor matchingOutputedProcessor;
        private readonly ILogger logger;
        private readonly IHubContext hubContext;

        public MatchingHistoryService(
            IAuthorizationProcessor authorizationProcessor,
            IMatchingHistorySearchProcessor matchingHistorySearchProcessor,
            IMatchingOutputedProcessor matchingOutputedProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.matchingHistorySearchProcessor = matchingHistorySearchProcessor;
            this.matchingOutputedProcessor = matchingOutputedProcessor;
            logger = logManager.GetLogger(typeof(MatchingHistoryService));
            hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        public async Task<MatchingHistorysResult> GetAsync(string SessionKey, MatchingHistorySearch MatchingHistorySearch, string connectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = (await matchingHistorySearchProcessor.GetAsync(MatchingHistorySearch, token, notifier)).ToList();
                return new MatchingHistorysResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingHistorys = result,
                };
            }, logger, connectionId);
        }

        public async  Task<MatchingHistoryResult> SaveOutputAtAsync(string SessionKey , long[] MatchingHeaderId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await matchingOutputedProcessor.SaveOutputAtAsync(MatchingHeaderId, token);
                return new MatchingHistoryResult
                {
                    ProcessResult = new ProcessResult { Result = true }
                };
            }, logger);
        }

    }
}
