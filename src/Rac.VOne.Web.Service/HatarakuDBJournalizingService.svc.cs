using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using NLog;

namespace Rac.VOne.Web.Service
{
    public class HatarakuDBJournalizingService :
        IHatarakuDBJournalizingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IHatarakuDBJournalizingProcessor hatarakuDBJournalizingProcessor;
        private readonly ILogger logger;
        public HatarakuDBJournalizingService(
            IAuthorizationProcessor authorizationProcessor,
            IHatarakuDBJournalizingProcessor hatarakuDBJournalizingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.hatarakuDBJournalizingProcessor = hatarakuDBJournalizingProcessor;
            logger = logManager.GetLogger(typeof(HatarakuDBJournalizingService));
        }

        public async Task<JournalizingSummariesResult> GetSummaryAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await hatarakuDBJournalizingProcessor.GetSummaryAsync(option, token);
                return new JournalizingSummariesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    JournalizingsSummaries = result.ToList(),
                };
            }, logger);

        public async Task<HatarakuDBDataResult> ExtractAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await hatarakuDBJournalizingProcessor.ExtractAsync(option, token);
                return new HatarakuDBDataResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    HatarakuDBData = result.ToList(),
                };
            }, logger);

        public async Task<CountResult> UpdateAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await hatarakuDBJournalizingProcessor.UpdateAsync(option, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        public async Task<CountResult> CancelAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await hatarakuDBJournalizingProcessor.CancelAsync(option, token);
                return new CountResult {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
    }
}
