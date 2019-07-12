using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IStatusExistsQueryProcessor
    {
        Task<bool> ExistReminderAsync(int StatusId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistReminderHistoryAsync(int StatusId, CancellationToken token = default(CancellationToken));
    }
}
