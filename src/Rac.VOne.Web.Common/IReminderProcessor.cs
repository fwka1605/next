using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReminderProcessor
    {
        Task<bool> ExistAsync(int companyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Reminder>> GetItemsAsync(ReminderSearch search, ReminderCommonSetting setting, ReminderSummarySetting[] summary, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderSummary>> GetSummaryItemsAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderSummary>> GetSummaryItemsByDestinationAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintAsync(int companyId, int[] reminderIds, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForPrintByDestinationCodeAsync(IEnumerable<Reminder> reminders, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintAsync(int companyId, int[] customerIds, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForSummaryPrintByDestinationCodeAsync(IEnumerable<ReminderSummary> reminderSummaries, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderBilling>> GetReminderBillingItemsForReprintByDestinationAsync(int companyId, ReminderOutputed reminderOutputed, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Reminder>> CreateAsync(int companyId, int loginUserId, int useForeignCurrency, Reminder[] Reminder, ReminderCommonSetting setting, ReminderSummarySetting[] summary, CancellationToken token = default(CancellationToken));
        Task<int> CancelAsync(IEnumerable<Reminder> Reminder, CancellationToken token = default(CancellationToken));
        Task<int> UpdateStatusAsync(int loginUserId, Reminder[] Reminder, CancellationToken token = default(CancellationToken));
        Task<int> UpdateSummaryStatusAsync(int loginUserId, ReminderSummary[] ReminderSummary, CancellationToken token = default(CancellationToken));
        Task<int> UpdateReminderOutputedAsync(int loginUserId, ReminderOutputed[] ReminderOutputed, CancellationToken token = default(CancellationToken));
        Task<int> UpdateReminderSummaryOutputedAsync(int loginUserId, ReminderOutputed[] reminderOutputed, ReminderSummary[] reminderSummary, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderOutputed>> GetOutputedItemsAsync(ReminderOutputedSearch search, CancellationToken token = default(CancellationToken));
        Task<int> GetMaxOutputNoAsync(int companyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderHistory>> GetReminderListAsync(ReminderSearch search, ReminderCommonSetting setting, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Reminder>> GetCancelDecisionItemsAsync(ReminderSearch search, ReminderCommonSetting setting, IEnumerable<ReminderSummarySetting> summary, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken));
    }
}
