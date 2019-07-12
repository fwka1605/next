using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ITransactionalGetByIdQueryProcessor<TEntity> where TEntity : ITransactional
    {
        Task<TEntity> GetByIdAsync(long Id, CancellationToken token = default(CancellationToken));
    }
}
