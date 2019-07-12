using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class ImportSettingMaster : IImportSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IImportSettingProcessor importsettingProcessor;
        private readonly ILogger logger;


        public ImportSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IImportSettingProcessor importsettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.importsettingProcessor = importsettingProcessor;
            logger = logManager.GetLogger(typeof(ImportSettingMaster));
        }

        public async Task<ImportSettingResult> GetAsync(string SessionKey, int CompanyId, int ImportFileType)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await importsettingProcessor.GetAsync(new ImportSettingSearch {
                    CompanyId       = CompanyId,
                    ImportFileType  = ImportFileType,
                }, token)).FirstOrDefault();
                return new ImportSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImportSetting = result,
                };
            }, logger);
        }

        public async Task<ImportSettingResults> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await importsettingProcessor.GetAsync(new ImportSettingSearch { CompanyId = CompanyId, }, token)).ToList();
                return new ImportSettingResults
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImportSettings = result,
                };
            }, logger);
        }

        public async Task<ImportSettingResult> SaveAsync(string SessionKey, ImportSetting[] ImportSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await importsettingProcessor.SaveAsync(ImportSetting, token);
                return new ImportSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true }
                };
            }, logger);
        }
    }
}
