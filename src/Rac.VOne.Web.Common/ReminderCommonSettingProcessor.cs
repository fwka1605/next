using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ReminderCommonSettingProcessor : IReminderCommonSettingProcessor
    {
        private readonly IByCompanyGetEntityQueryProcessor<ReminderCommonSetting> getReminderCommonSettingQueryProcessor;
        private readonly IAddReminderCommonSettingQueryProcessor addReminderCommonSettingQueryProcessor;

        public ReminderCommonSettingProcessor(
            IByCompanyGetEntityQueryProcessor<ReminderCommonSetting> getReminderCommonSettingQueryProcessor,
            IAddReminderCommonSettingQueryProcessor addReminderCommonSettingQueryProcessor)
        {
            this.getReminderCommonSettingQueryProcessor = getReminderCommonSettingQueryProcessor;
            this.addReminderCommonSettingQueryProcessor = addReminderCommonSettingQueryProcessor;
        }

        public Task<ReminderCommonSetting> GetItemAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => getReminderCommonSettingQueryProcessor.GetAsync(CompanyId, token);


        public Task<ReminderCommonSetting> SaveAsync(ReminderCommonSetting ReminderCommonSetting, CancellationToken token = default(CancellationToken))
            => addReminderCommonSettingQueryProcessor.SaveAsync(ReminderCommonSetting, token);

    }
}
