using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReminderQueryProcessor
    {
        Task<Reminder> AddAsync(Reminder Reminder, CancellationToken token = default(CancellationToken));
        Task<ReminderSummary> AddSummaryAsync(ReminderSummary ReminderSummary, CancellationToken token = default(CancellationToken));
    }
}
