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

namespace Rac.VOne.Web.Service.Master
{
    public class WebApiSettingMaster : IWebApiSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IWebApiSettingProcessor webApiSettingProcessor;
        private readonly ILogger logger;

        public WebApiSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IWebApiSettingProcessor webApiSettingProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.webApiSettingProcessor = webApiSettingProcessor;
            logger = logManager.GetLogger(typeof(WebApiSettingMaster));
        }


        public async Task<CountResult> SaveAsync(string SessionKey, WebApiSetting setting)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await webApiSettingProcessor.SaveAsync(setting, token);
                return new CountResult {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);

        public async Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int? ApiTypeId)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await webApiSettingProcessor.DeleteAsync(CompanyId, ApiTypeId);
                return new CountResult {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);

        public async Task<WebApiSettingResult> GetByIdAsync(string SessionKey, int CompanyId, int ApiTypeId)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await webApiSettingProcessor.GetByIdAsync(CompanyId, ApiTypeId);
                return new WebApiSettingResult {
                    ProcessResult = new ProcessResult { Result = true },
                    WebApiSetting = result,
                };
            }, logger);

    }
}
