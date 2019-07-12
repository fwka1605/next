using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class ImporterSettingService : IImporterSettingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IImporterSettingProcessor importSettingProcessor;
        private readonly IImporterSettingDetailProcessor importSettingDetailProcessor;
        private readonly ILogger logger;

        public ImporterSettingService(IAuthorizationProcessor authorizationProcessor,
            IImporterSettingProcessor importSettingProcessor,
            IImporterSettingDetailProcessor importSettingDetailProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.importSettingProcessor = importSettingProcessor;
            this.importSettingDetailProcessor = importSettingDetailProcessor;
            logger = logManager.GetLogger(typeof(ImporterSettingService));
        }

        public async Task<ImporterSettingsResult> GetHeaderAsync(string SessionKey, int CompanyId, int FormatId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await importSettingProcessor.GetAsync(new ImporterSetting {
                    CompanyId   = CompanyId,
                    FormatId    = FormatId,
                }, token)).ToList();
                return new ImporterSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImporterSettings = result,
                };
            }, logger);
        }

        public async Task<ImporterSettingResult> GetHeaderByCodeAsync(string SessionKey, int CompanyId, int FormatId, string Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await importSettingProcessor.GetAsync(new ImporterSetting {
                    CompanyId   = CompanyId,
                    FormatId    = FormatId,
                    Code        = Code,
                },token)).FirstOrDefault();

                return new ImporterSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImporterSetting =  result,
                };
            }, logger);
        }

        public async Task<ImporterSettingAndDetailResult> SaveAsync(string SessionKey, ImporterSetting ImpSetting, ImporterSettingDetail[] ImpSettingDetail)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                ImpSetting.Details = new List<ImporterSettingDetail>(ImpSettingDetail);

                var result = await importSettingProcessor.SaveAsync(ImpSetting, token);
                return new ImporterSettingAndDetailResult
                {
                    ProcessResult           = new ProcessResult { Result = true },
                    ImporterSetting         = result,
                    ImporterSettingDetail   = result.Details.ToArray(),
                };

            }, logger);
        }

        public async Task<ImporterSettingDetailsResult> GetDetailByCodeAsync(string SessionKey, int CompanyId, int FormatId, string Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await importSettingDetailProcessor.GetAsync(new ImporterSetting {
                    CompanyId   = CompanyId,
                    FormatId    = FormatId,
                    Code        = Code,
                }, token)).ToList();
                return new ImporterSettingDetailsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImporterSettingDetails = result,
                };
            }, logger);
        }

        public async Task<ImporterSettingDetailsResult> GetDetailByIdAsync(string SessionKey, int ImporterSettingId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await importSettingDetailProcessor.GetAsync(new ImporterSetting { Id = ImporterSettingId }, token)).ToList();
                return new ImporterSettingDetailsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ImporterSettingDetails = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int ImporterSettingId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var  result = await importSettingProcessor.DeleteAsync(ImporterSettingId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result
                };
            }, logger);
        }

    }
}
