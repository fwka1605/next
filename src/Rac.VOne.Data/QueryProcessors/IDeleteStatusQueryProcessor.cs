using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteStatusQueryProcessor
    {
        Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken));
    }
}
