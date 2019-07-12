using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddStatusQueryProcessor
    {
        Task<Status> SaveAsync(Status status, CancellationToken token = default(CancellationToken));
        Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken));
    }
}
