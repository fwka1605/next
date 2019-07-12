using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReminderHistoryQueryProcessor
    {
        Task<IEnumerable<ReminderHistory>> GetItemsByReminderIdAsync(int reminderId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderSummaryHistory>> GetSummaryItemsByReminderSummaryIdAsync(int reminderSummaryId, CancellationToken token = default(CancellationToken));
    }
}
