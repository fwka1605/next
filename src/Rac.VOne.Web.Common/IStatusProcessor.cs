using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IStatusProcessor
    {
        Task<IEnumerable<Status>> GetAsync(StatusSearch option, CancellationToken token = default(CancellationToken));
        Task<Status> SaveAsync(Status Status, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
        Task<bool> ExistReminderAsync(int StatusId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistReminderHistoryAsync(int StatusId, CancellationToken token = default(CancellationToken));
    }
}
