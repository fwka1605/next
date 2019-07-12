using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReminderOutputedQueryProcessor
    {
        Task<int> AddAsync(ReminderOutputed ReminderOutputed, CancellationToken token = default(CancellationToken));
        Task<int> UpdateReminderAsync(int reminderId, DateTime outputAt, CancellationToken token = default(CancellationToken));
        Task<int> AddReminderHistoryAsync(int loginUserId, int ReminderId, DateTime outputAt, decimal reminderAmount, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReminderOutputed>> GetItemsAsync(ReminderOutputedSearch search, CancellationToken token = default(CancellationToken));
        Task<int> GetMaxOutputNoAsync(int companyId, CancellationToken token = default(CancellationToken));

    }
}
