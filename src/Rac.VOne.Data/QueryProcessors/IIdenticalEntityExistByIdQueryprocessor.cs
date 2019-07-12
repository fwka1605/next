using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IIdenticalEntityExistByIdQueryprocessor<TEntity> where TEntity : IIdentical
    {
        Task<bool> ExistAsync(int Id, CancellationToken token = default(CancellationToken));

    }
}
