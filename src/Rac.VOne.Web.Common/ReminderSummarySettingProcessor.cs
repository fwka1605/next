using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ReminderSummarySettingProcessor : IReminderSummarySettingProcessor
    {
        private readonly IAddReminderSummarySettingQueryProcessor addReminderSummarySettingQueryProcessor;
        private readonly IReminderSummarySettingQueryProcessor reminderSummarySettingQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyIdQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReminderSummarySettingProcessor(
            IAddReminderSummarySettingQueryProcessor addReminderSummarySettingQueryProcessor,
            IReminderSummarySettingQueryProcessor reminderSummarySettingQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyIdQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.addReminderSummarySettingQueryProcessor = addReminderSummarySettingQueryProcessor;
            this.reminderSummarySettingQueryProcessor = reminderSummarySettingQueryProcessor;
            this.applicationControlByCompanyIdQueryProcessor = applicationControlByCompanyIdQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<ReminderSummarySetting>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            var app = await applicationControlByCompanyIdQueryProcessor.GetAsync(CompanyId, token);
            return await reminderSummarySettingQueryProcessor.GetAsync(CompanyId, app.UseForeignCurrency, token);
        }


        public async Task<IEnumerable<ReminderSummarySetting>> SaveAsync(IEnumerable<ReminderSummarySetting> settings, CancellationToken token = default(CancellationToken))
        {
            var result = new List<ReminderSummarySetting>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var setting in settings)
                    result.Add(await addReminderSummarySettingQueryProcessor.SaveAsync(setting, token));
                scope.Complete();
            }
            return result;
        }
    }
}
