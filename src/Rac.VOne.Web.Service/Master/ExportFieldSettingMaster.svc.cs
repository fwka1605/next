using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class ExportFieldSettingMaster : IExportFieldSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IExportFieldSettingProcessor exportFieldSettingProcessor;
        private readonly ILogger logger;

        public ExportFieldSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IExportFieldSettingProcessor exportFieldSettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.exportFieldSettingProcessor = exportFieldSettingProcessor;
            logger = logManager.GetLogger(typeof(ExportFieldSettingMaster));
        }

        public async Task<ExportFieldSettingsResult> GetItemsByExportFileTypeAsync(string SessionKey, int CompanyId, int ExportFileType)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await exportFieldSettingProcessor.GetAsync(new ExportFieldSetting {
                    CompanyId = CompanyId,
                    ExportFileType = ExportFileType,
                })).ToList();

                return new ExportFieldSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ExportFieldSettings = result,
                };
            }, logger);
        }

        public async Task<ExportFieldSettingsResult> SaveAsync(string SessionKey, ExportFieldSetting[] ExportFieldSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await exportFieldSettingProcessor.SaveAsync(ExportFieldSetting, token)).ToList();
                return new ExportFieldSettingsResult
                {
                    ExportFieldSettings = result,
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);
        }


    }
}
