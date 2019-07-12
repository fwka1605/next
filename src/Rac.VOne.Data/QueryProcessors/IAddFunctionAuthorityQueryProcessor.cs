using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddFunctionAuthorityQueryProcessor
    {
        Task<FunctionAuthority> SaveAsync(FunctionAuthority authority, CancellationToken token = default(CancellationToken));
    }
}
