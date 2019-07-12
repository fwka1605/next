using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IControlColorProcessor
    {
        Task<ControlColor> GetAsync(int CompanyId, int LoginUserId, CancellationToken token = default(CancellationToken));
        Task<ControlColor> SaveAsync(ControlColor ControlColor, CancellationToken token = default(CancellationToken));
    }
}
