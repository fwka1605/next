using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IApplicationControlProcessor
    {
        Task<ApplicationControl> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<int> UpdateUseOperationLogAsync(ApplicationControl AppData, CancellationToken token = default(CancellationToken));
        Task<ApplicationControl> AddAsync(ApplicationControl ApplicationControl, CancellationToken token = default(CancellationToken));
    }
}
