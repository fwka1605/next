using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using Rac.VOne.Data.QueryProcessors;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Service.Master
{
    public class ClosingSettingMaster : IClosingSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ILogger logger;
        private readonly IClosingSettingProcessor closingSettingProcessor;

        public ClosingSettingMaster(IAuthorizationProcessor authorizationProcessor,
            IClosingSettingProcessor closingSettingProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.closingSettingProcessor = closingSettingProcessor;
            logger = logManager.GetLogger(typeof(CollectionScheduleService));
        }

        public async Task<ClosingSettingResult> GetAsync(string sessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await closingSettingProcessor.GetAsync(companyId, token);
                return new ClosingSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ClosingSetting = result
                };
            }, logger);
        }

        public async Task<ClosingSettingResult> SaveAsync(string sessionKey, ClosingSetting setting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await closingSettingProcessor.SaveAsync(setting, token);
                return new ClosingSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ClosingSetting = result,
                };
            }, logger);
        }

    }
}
