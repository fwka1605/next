using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using Rac.VOne.Common.Logging;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class SettingMaster : ISettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ISettingProcessor settingProcessor;
        private readonly ILogger logger;

        public SettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            ISettingProcessor settingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.settingProcessor = settingProcessor;
            logger = logManager.GetLogger(typeof(SettingMaster));
        }

        public async Task<SettingsResult> GetItemsAsync(string SessionKey, string[] ItemId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await settingProcessor.GetAsync(ItemId, token)).ToList();

                return new SettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Settings = result,
                };
            }, logger);
        }
    }
}
