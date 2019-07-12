using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteReminderSummaryQueryProcessor
    {
        Task<int> DeleteReminderSummaryAsync(Reminder reminder, CancellationToken token = default(CancellationToken));
    }
}
