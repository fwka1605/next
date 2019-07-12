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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "ReminderSettingService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで ReminderSettingService.svc または ReminderSettingService.svc.cs を選択し、デバッグを開始してください。
    public class ReminderSettingService : IReminderSettingService
    {
        private readonly IAuthorizationProcessor authorizationProcess;
        private readonly IReminderCommonSettingProcessor reminderCommonSettingProcessor;
        private readonly IReminderTemplateSettingProcessor reminderTemplateSettingProcessor;
        private readonly IReminderLevelSettingProcessor reminderLevelSettingProcessor;
        private readonly IReminderSummarySettingProcessor reminderSummarySettingProcessor;
        private readonly ILogger logger;

        public ReminderSettingService(
            IAuthorizationProcessor authorizationProcess,
            IReminderCommonSettingProcessor reminderCommonSettingProcessor,
            IReminderTemplateSettingProcessor reminderTemplateSettingProcessor,
            IReminderLevelSettingProcessor reminderLevelSettingProcessor,
            IReminderSummarySettingProcessor reminderSummarySettingProcessor,
            ILogManager logManager)
        {
            this.authorizationProcess = authorizationProcess;
            this.reminderCommonSettingProcessor = reminderCommonSettingProcessor;
            this.reminderTemplateSettingProcessor = reminderTemplateSettingProcessor;
            this.reminderLevelSettingProcessor = reminderLevelSettingProcessor;
            this.reminderSummarySettingProcessor = reminderSummarySettingProcessor;
            logger = logManager.GetLogger(typeof(ReminderSettingService));
        }

        public async Task<ReminderCommonSettingResult> GetReminderCommonSettingAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderCommonSettingProcessor.GetItemAsync(CompanyId, token);
                return new ReminderCommonSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderCommonSetting = result,
                };
            }, logger);
        }

        public async Task<ReminderCommonSettingResult> SaveReminderCommonSettingAsync(string SessionKey, ReminderCommonSetting ReminderCommonSetting)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderCommonSettingProcessor.SaveAsync(ReminderCommonSetting, token);
                return new ReminderCommonSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderCommonSetting = result,
                };
            }, logger);
        }

        public async Task<ReminderTemplateSettingsResult> GetReminderTemplateSettingsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderTemplateSettingProcessor.GetItemsAsync(CompanyId, token)).ToList();
                return new ReminderTemplateSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderTemplateSettings = result,
                };
            }, logger);
        }

        public async Task<ReminderTemplateSettingResult> GetReminderTemplateSettingByCodeAsync(string SessionKey, int CompanyId, string Code)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderTemplateSettingProcessor.GetByCodeAsync(CompanyId, Code, token);
                return new ReminderTemplateSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderTemplateSetting = result,
                };
            }, logger);
        }

        public async Task<ReminderTemplateSettingResult> SaveReminderTemplateSettingAsync(string SessionKey, ReminderTemplateSetting ReminderTemplateSetting)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderTemplateSettingProcessor.SaveAsync(ReminderTemplateSetting, token);
                return new ReminderTemplateSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderTemplateSetting = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteReminderTemplateSettingAsync(string SessionKey, int Id)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderTemplateSettingProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistReminderTemplateSettingAsync(string SessionKey, int ReminderTemplateId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderTemplateSettingProcessor.ExistAsync(ReminderTemplateId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ReminderLevelSettingsResult> GetReminderLevelSettingsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderLevelSettingProcessor.GetItemsAsync(CompanyId, token)).ToList();
                return new ReminderLevelSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderLevelSettings = result,
                };
            }, logger);
        }

        public async Task<ReminderLevelSettingResult> GetReminderLevelSettingByLevelAsync(string SessionKey, int CompanyId, int ReminderLevel)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderLevelSettingProcessor.GetItemByLevelAsync(CompanyId, ReminderLevel, token);
                return new ReminderLevelSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderLevelSetting = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistTemplateAtReminderLevelAsync(string SessionKey, int ReminderTemplateId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderLevelSettingProcessor.ExistReminderTemplateAsync(ReminderTemplateId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<MaxReminderLevelResult> GetMaxReminderLevelAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderLevelSettingProcessor.GetMaxLevelAsync(CompanyId, token);
                return new MaxReminderLevelResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MaxReminderLevel = result,
                };
            }, logger);
        }

        public async Task<ReminderLevelSettingResult> SaveReminderLevelSettingAsync(string SessionKey, ReminderLevelSetting ReminderLevelSetting)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderLevelSettingProcessor.SaveAsync(ReminderLevelSetting, token);
                return new ReminderLevelSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderLevelSetting = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteReminderLevelSettingAsync(string SessionKey, int CompanyId, int ReminderLevel)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await reminderLevelSettingProcessor.DeleteAsync(CompanyId, ReminderLevel, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ReminderSummarySettingsResult> GetReminderSummarySettingsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderSummarySettingProcessor.GetAsync(CompanyId, token)).ToList();
                return new ReminderSummarySettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderSummarySettings = result,
                };
            }, logger);
        }

        public async Task<ReminderSummarySettingsResult> SaveReminderSummarySettingAsync(string SessionKey, ReminderSummarySetting[] ReminderSummarySettings)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await reminderSummarySettingProcessor.SaveAsync(ReminderSummarySettings, token)).ToList();
                return new ReminderSummarySettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReminderSummarySettings = result,
                };
            }, logger);
        }

    }
}
