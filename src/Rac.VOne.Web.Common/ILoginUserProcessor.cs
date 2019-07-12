using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ILoginUserProcessor
    {
        Task<IEnumerable<LoginUser>> GetAsync(LoginUserSearch option, CancellationToken token = default(CancellationToken));

        Task<LoginUser> SaveAsync(LoginUser loginUser, CancellationToken token = default(CancellationToken));

        Task<bool> ExitStaffAsync(int StafffId, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken));
        Task<ImportResult> ImportAsync(
            IEnumerable<LoginUser> InsertList,
            IEnumerable<LoginUser> UpdateList,
            IEnumerable<LoginUser> DeleteList, CancellationToken token = default(CancellationToken));
    }
}
