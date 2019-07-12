using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteLoginUserPasswordQueryProcessor
    {
        Task<int> DeleteAsync(int LoginUserId, CancellationToken token = default(CancellationToken));
    }
}
