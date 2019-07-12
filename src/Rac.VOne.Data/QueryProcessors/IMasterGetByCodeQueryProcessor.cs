using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IMasterGetByCodeQueryProcessor<TEntity> where TEntity : IMaster
    {
        Task<TEntity> GetByCodeAsync(int CompnayId, string Code, CancellationToken token = default(CancellationToken));

    }
}
