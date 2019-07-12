using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ITransactionalGetByIdsQueryProcessor<TEntity> where TEntity : ITransactional
    {
        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken));
    }
}
