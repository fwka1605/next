using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddApplicationControlQueryProcessor
    {
        Task<ApplicationControl> AddAsync(ApplicationControl applicationControl, CancellationToken token = default(CancellationToken));
    }
}
