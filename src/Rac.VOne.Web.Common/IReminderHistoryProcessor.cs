using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReminderHistoryProcessor
    {
        Task<IEnumerable<ReminderHistory>> GetItemsByReminderIdAsync(int reminderId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderSummaryHistory>> GetSummaryItemsByReminderSummaryIdAsync(int reminderSummaryId, CancellationToken token = default(CancellationToken));
        Task<ReminderHistory> UpdateReminderHistoryAsync(ReminderHistory reminderHistory, CancellationToken token = default(CancellationToken));
        Task<ReminderSummaryHistory> UpdateReminderSummaryHistoryAsync(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token = default(CancellationToken));
        Task<int> DeleteHistoryAsync(ReminderHistory reminderHistory, CancellationToken token = default(CancellationToken));
        Task<int> DeleteSummaryHistoryAsync(ReminderSummaryHistory reminderSummaryHistory, CancellationToken token = default(CancellationToken));
    }
}
