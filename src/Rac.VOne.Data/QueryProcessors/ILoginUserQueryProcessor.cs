using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ILoginUserQueryProcessor
    {
        Task<IEnumerable<LoginUser>> GetAsync(LoginUserSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExitStaffAsync(int StaffId, CancellationToken token = default(CancellationToken));
    }
}
