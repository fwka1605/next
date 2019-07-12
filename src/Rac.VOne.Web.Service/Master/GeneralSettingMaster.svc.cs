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
    public class GeneralSettingMaster : IGeneralSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IGeneralSettingProcessor generalSettingProcessor;
        private readonly ILogger logger;

        public GeneralSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IGeneralSettingProcessor generalSettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.generalSettingProcessor = generalSettingProcessor;
            logger = logManager.GetLogger(typeof(GeneralSettingMaster));
        }

        public async Task<GeneralSettingsResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await generalSettingProcessor.GetAsync(new GeneralSetting { CompanyId = CompanyId, }, token)).ToList();
                return new GeneralSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GeneralSettings = result,
                };
            }, logger);
        }

        public async Task<GeneralSettingResult> SaveAsync(string SessionKey, GeneralSetting Generalsetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await generalSettingProcessor.SaveAsync(Generalsetting, token);

                return new GeneralSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GeneralSetting = result,
                };

            }, logger);
        }

        public async Task<GeneralSettingResult> GetByCodeAsync(string SessionKey, int CompanyId, string Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await generalSettingProcessor.GetAsync(new GeneralSetting {
                    CompanyId   = CompanyId,
                    Code        = Code,
                }, token)).First();
                return new GeneralSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GeneralSetting = result,
                };
            }, logger);
        }

    }
}
