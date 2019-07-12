using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ReminderTemplateSettingProcessor : IReminderTemplateSettingProcessor
    {
        private readonly IByCompanyGetEntitiesQueryProcessor<ReminderTemplateSetting> getReminderTemplateSettingQueryProcessor;
        private readonly IMasterGetByCodeQueryProcessor<ReminderTemplateSetting> getReminderTemplateSettingGetByCodeQueryProcessor;
        private readonly IAddReminderTemplateSettingQueryProcessor addReminderTemplateSettingQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<ReminderTemplateSetting> deleteReminderTemplateSettingQueryProcessor;
        private readonly IIdenticalEntityExistByIdQueryprocessor<ReminderTemplateSetting> templateExistByIdQueryProcessor;

        public ReminderTemplateSettingProcessor(
            IByCompanyGetEntitiesQueryProcessor<ReminderTemplateSetting> getReminderTemplateSettingQueryProcessor,
            IMasterGetByCodeQueryProcessor<ReminderTemplateSetting> getReminderTemplateSettingGetByCodeQueryProcessor,
            IAddReminderTemplateSettingQueryProcessor addReminderTemplateSettingQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<ReminderTemplateSetting> deleteReminderTemplateSettingQueryProcessor,
            IIdenticalEntityExistByIdQueryprocessor<ReminderTemplateSetting> templateExistByIdQueryProcessor
            )
        {
            this.getReminderTemplateSettingQueryProcessor = getReminderTemplateSettingQueryProcessor;
            this.getReminderTemplateSettingGetByCodeQueryProcessor = getReminderTemplateSettingGetByCodeQueryProcessor;
            this.addReminderTemplateSettingQueryProcessor = addReminderTemplateSettingQueryProcessor;
            this.deleteReminderTemplateSettingQueryProcessor = deleteReminderTemplateSettingQueryProcessor;
            this.templateExistByIdQueryProcessor = templateExistByIdQueryProcessor;
        }

        public async Task<IEnumerable<ReminderTemplateSetting>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await getReminderTemplateSettingQueryProcessor.GetItemsAsync(CompanyId);

        public async Task<ReminderTemplateSetting> GetByCodeAsync(int CompanyId, string Code, CancellationToken token = default(CancellationToken))
            => await getReminderTemplateSettingGetByCodeQueryProcessor.GetByCodeAsync(CompanyId, Code);

        public async Task<ReminderTemplateSetting> SaveAsync(ReminderTemplateSetting ReminderTemplateSetting, CancellationToken token = default(CancellationToken))
            => await addReminderTemplateSettingQueryProcessor.SaveAsync(ReminderTemplateSetting, token);

        public async Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await deleteReminderTemplateSettingQueryProcessor.DeleteAsync(CompanyId, token);

        public async Task<bool> ExistAsync(int Id, CancellationToken token = default(CancellationToken))
            => await templateExistByIdQueryProcessor.ExistAsync(Id, token);


    }
}
