using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReminderQueryProcessor
    {
        Task<IEnumerable<Reminder>> GetItemsAsync(ReminderSearch search, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderSummary>> GetSummaryItemsAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderSummary>> GetSummaryItemsByDestinationAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintAsync(int companyId, IEnumerable<int> reminderIds, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintByDestinationCodeAsync(Reminder reminder, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintAsync(int companyId, IEnumerable<int> customerIds, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(ReminderSummary reminderSummary, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintByDestinationAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderHistory>> GetReminderListAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Reminder>> GetCancelDecisionItemsAsync(ReminderSearch search, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken));
    }
}
