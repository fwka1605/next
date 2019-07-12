using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateReminderQueryProcessor
    {
        Task<Reminder> UpdateStatusAsync(Reminder Reminder, CancellationToken token = default(CancellationToken));
        Task<ReminderSummary> UpdateSummaryStatusAsync(ReminderSummary ReminderSummary, CancellationToken token = default(CancellationToken));
    }
}
