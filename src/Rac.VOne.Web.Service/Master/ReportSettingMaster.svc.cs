using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class ReportSettingMaster : IReportSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IReportSettingProcessor reportSettingProcessor;
        private readonly ILogger logger;

        public ReportSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IReportSettingProcessor reportSettingProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.reportSettingProcessor = reportSettingProcessor;
            logger = logManager.GetLogger(typeof(ReportSettingMaster));
        }

        public async Task<ReportSettingsResult> GetItemsAsync(string SessionKey, int CompanyId, string ReportId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reportSettingProcessor.GetAsync(CompanyId, ReportId, token)).ToList();
                return new ReportSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReportSettings = result,
                };
            }, logger);
        }

        public async Task< ReportSettingsResult> SaveAsync(string SessionKey, ReportSetting[] ReportSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reportSettingProcessor.SaveAsync(ReportSetting, token)).ToList();
                return new ReportSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReportSettings = result,
                };
            }, logger);
        }

    }
}
