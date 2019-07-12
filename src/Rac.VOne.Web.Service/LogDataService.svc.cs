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
    public class LogDataService : ILogDataService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ILogDataProcessor logDataProcessor;
        private readonly ILogger logger;

        public LogDataService(
            IAuthorizationProcessor authorizationProcessor,
            ILogDataProcessor logDataProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.logDataProcessor = logDataProcessor;
            logger = logManager.GetLogger(typeof(LogDataService));
        }

        public async Task<LogDataResult> GetItemsAsync(string SessionKey, int CompanyId, DateTime? LoggedAtFrom, DateTime? LoggedAtTo, string LoginUserCode)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await logDataProcessor.GetItemsAsync(new LogDataSearch {
                    CompanyId       = CompanyId,
                    LoggedAtFrom    = LoggedAtFrom,
                    LoggedAtTo      = LoggedAtTo,
                    LoginUserCode   = LoginUserCode,
                }, token)).ToList();

                return new LogDataResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    LogData = result,
                };
            }, logger);
        }

        public async Task<CountResult> LogAsync(string SessionKey, LogData LogData)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await logDataProcessor.LogAsync(LogData, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<LogDatasResult> GetStatsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await logDataProcessor.GetStatsAsync(CompanyId, token);
                return new LogDatasResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Datas = new[] { result },
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAllAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await logDataProcessor.DeleteAllAsync(CompanyId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }
     }
}
