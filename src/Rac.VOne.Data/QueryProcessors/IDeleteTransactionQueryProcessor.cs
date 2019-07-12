using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteTransactionQueryProcessor<TEntity> where TEntity : ITransactional
    {
        Task<int> DeleteAsync(long id, CancellationToken token = default(CancellationToken));
    }
}
