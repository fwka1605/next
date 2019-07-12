using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// 督促状設定
    /// </summary>
    /// <remarks>controller を 分けるべき メソッド名が長すぎ 多すぎ</remarks>
    public class ReminderSettingController : ApiControllerAuthorized
    {
        private readonly IReminderCommonSettingProcessor reminderCommonSettingProcessor;
        private readonly IReminderTemplateSettingProcessor reminderTemplateSettingProcessor;
        private readonly IReminderLevelSettingProcessor reminderLevelSettingProcessor;
        private readonly IReminderSummarySettingProcessor reminderSummarySettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ReminderSettingController(
            IReminderCommonSettingProcessor reminderCommonSettingProcessor,
            IReminderTemplateSettingProcessor reminderTemplateSettingProcessor,
            IReminderLevelSettingProcessor reminderLevelSettingProcessor,
            IReminderSummarySettingProcessor reminderSummarySettingProcessor
            )
        {
            this.reminderCommonSettingProcessor = reminderCommonSettingProcessor;
            this.reminderTemplateSettingProcessor = reminderTemplateSettingProcessor;
            this.reminderLevelSettingProcessor = reminderLevelSettingProcessor;
            this.reminderSummarySettingProcessor = reminderSummarySettingProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReminderCommonSetting> GetReminderCommonSetting([FromBody] int CompanyId, CancellationToken token)
            => await reminderCommonSettingProcessor.GetItemAsync(CompanyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReminderCommonSetting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReminderCommonSetting> SaveReminderCommonSetting(ReminderCommonSetting ReminderCommonSetting, CancellationToken token)
            => await reminderCommonSettingProcessor.SaveAsync(ReminderCommonSetting, token);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderTemplateSetting>> GetReminderTemplateSettingByCode(MasterSearchOption option, CancellationToken token)
        {
            if (string.IsNullOrEmpty(option.Code))
                return (await reminderTemplateSettingProcessor.GetItemsAsync(option.CompanyId, token)).ToArray();
            else
                return new[] { await reminderTemplateSettingProcessor.GetByCodeAsync(option.CompanyId, option.Code, token) };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReminderTemplateSetting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReminderTemplateSetting> SaveReminderTemplateSetting(ReminderTemplateSetting ReminderTemplateSetting, CancellationToken token)
            => await reminderTemplateSettingProcessor.SaveAsync(ReminderTemplateSetting, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteReminderTemplateSetting([FromBody] int Id, CancellationToken token)
            => await reminderTemplateSettingProcessor.DeleteAsync(Id, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReminderTemplateId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistReminderTemplateSetting([FromBody] int ReminderTemplateId, CancellationToken token)
            => await reminderTemplateSettingProcessor.ExistAsync(ReminderTemplateId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderLevelSetting>> GetReminderLevelSettings([FromBody] int CompanyId, CancellationToken token)
            => (await reminderLevelSettingProcessor.GetItemsAsync(CompanyId, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting">Companyid, ReminderLevel を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReminderLevelSetting> GetReminderLevelSettingByLevel(ReminderLevelSetting setting, CancellationToken token)
            => await reminderLevelSettingProcessor.GetItemByLevelAsync(setting.CompanyId, setting.ReminderLevel, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReminderTemplateId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistTemplateAtReminderLevel([FromBody] int ReminderTemplateId, CancellationToken token)
            => await reminderLevelSettingProcessor.ExistReminderTemplateAsync(ReminderTemplateId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> GetMaxReminderLevel([FromBody] int CompanyId, CancellationToken token)
            => await reminderLevelSettingProcessor.GetMaxLevelAsync(CompanyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReminderLevelSetting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReminderLevelSetting> SaveReminderLevelSetting(ReminderLevelSetting ReminderLevelSetting, CancellationToken token)
            => await reminderLevelSettingProcessor.SaveAsync(ReminderLevelSetting, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting">Companyid, ReminderLevel を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteReminderLevelSetting(ReminderLevelSetting setting, CancellationToken token)
            => await reminderLevelSettingProcessor.DeleteAsync(setting.CompanyId, setting.ReminderLevel, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderSummarySetting>> GetReminderSummarySettings([FromBody] int CompanyId, CancellationToken token)
            => (await reminderSummarySettingProcessor.GetAsync(CompanyId, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReminderSummarySetting>> SaveReminderSummarySetting(IEnumerable<ReminderSummarySetting> settings, CancellationToken token)
            => (await reminderSummarySettingProcessor.SaveAsync(settings, token)).ToArray();

    }
}
