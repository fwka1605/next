using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteReminderHistoryQueryProcessor
    {
        Task<int> DeleteByReminderIdAsync(int reminderId, CancellationToken token = default(CancellationToken));
        Task<int> DeleteReminderSummaryHistoryAsync(Reminder reminder, CancellationToken token = default(CancellationToken));
    }
}
