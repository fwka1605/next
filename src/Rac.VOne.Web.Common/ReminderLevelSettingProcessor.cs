using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ReminderLevelSettingProcessor : IReminderLevelSettingProcessor
    {
        private readonly IByCompanyGetEntitiesQueryProcessor<ReminderLevelSetting> getReminderLevelSettingQueryProcessor;
        private readonly IReminderLevelSettingQueryProcessor reminderLevelSettingQueryProcessor;
        private readonly IAddReminderLevelSettingQueryProcessor addReminderLevelSettingQueryProcessor;
        private readonly IDeleteReminderLevelSettingQueryProcessor deleteReminderLevelSettingQueryProcessor;

        public ReminderLevelSettingProcessor(
            IByCompanyGetEntitiesQueryProcessor<ReminderLevelSetting> getReminderLevelSettingQueryProcessor,
            IReminderLevelSettingQueryProcessor reminderLevelSettingQueryProcessor,
            IAddReminderLevelSettingQueryProcessor addReminderLevelSettingQueryProcessor,
            IDeleteReminderLevelSettingQueryProcessor deleteReminderLevelSettingQueryProcessor
            )
        {
            this.getReminderLevelSettingQueryProcessor = getReminderLevelSettingQueryProcessor;
            this.reminderLevelSettingQueryProcessor = reminderLevelSettingQueryProcessor;
            this.addReminderLevelSettingQueryProcessor = addReminderLevelSettingQueryProcessor;
            this.deleteReminderLevelSettingQueryProcessor = deleteReminderLevelSettingQueryProcessor;
        }

        public async Task<IEnumerable<ReminderLevelSetting>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await getReminderLevelSettingQueryProcessor.GetItemsAsync(CompanyId, token);

        public async Task<bool> ExistReminderTemplateAsync(int ReminderTemplateId, CancellationToken token = default(CancellationToken))
            => await reminderLevelSettingQueryProcessor.ExistReminderTemplateAsync(ReminderTemplateId, token);

        public async Task<int> GetMaxLevelAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await reminderLevelSettingQueryProcessor.GetMaxLevelAsync(CompanyId, token);

        public async Task<ReminderLevelSetting> GetItemByLevelAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken))
            => await reminderLevelSettingQueryProcessor.GetItemByLevelAsync(CompanyId, ReminderLevel, token);

        public async Task<ReminderLevelSetting> SaveAsync(ReminderLevelSetting ReminderLevelSetting, CancellationToken token = default(CancellationToken))
            => await addReminderLevelSettingQueryProcessor.SaveAsync(ReminderLevelSetting, token);

        public async Task<int> DeleteAsync(int CompanyId, int ReminderLevel, CancellationToken token = default(CancellationToken))
            => await deleteReminderLevelSettingQueryProcessor.DeleteAsync(CompanyId, ReminderLevel, token);

    }
}
