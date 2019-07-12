using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReminderHistoryQueryProcessor
    {
        Task<ReminderHistory> AddAsync(ReminderHistory ReminderHistory, CancellationToken token = default(CancellationToken));
        Task<ReminderSummaryHistory> AddSummaryAsync(ReminderSummaryHistory ReminderSummaryHistory, CancellationToken token = default(CancellationToken));
    }
}
