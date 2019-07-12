using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IIdenticalEntityGetByIdsQueryProcessor<TEntity> where TEntity : IIdentical
    {
        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<int> Ids, CancellationToken token = default(CancellationToken));
    }
}
