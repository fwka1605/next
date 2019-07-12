using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddLoginUserQueryProcessor
    {
        Task<LoginUser> SaveAsync(LoginUser loginUser, CancellationToken token = default(CancellationToken));
    }
}
