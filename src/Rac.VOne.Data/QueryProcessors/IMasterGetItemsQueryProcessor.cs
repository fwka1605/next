using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IMasterGetItemsQueryProcessor<TEntity> where TEntity : IMaster
    {
        Task<IEnumerable<TEntity>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken));
    }
}
