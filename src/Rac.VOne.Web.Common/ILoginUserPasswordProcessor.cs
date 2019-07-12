using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ILoginUserPasswordProcessor
    {
        Task<int> ResetAsync(int LoginUserId, CancellationToken token = default(CancellationToken));
        Task<LoginResult> LoginAsync(int CompanyId, int LoginUserId, string Password, CancellationToken token = default(CancellationToken));
        Task<PasswordChangeResult> SaveAsync(int CompanyId, int LoginUserId, string Password, CancellationToken token = default(CancellationToken));
        Task<PasswordChangeResult> ChangeAsync(int CompanyId, int LoginUserId, string OldPassword, string NewPassword, CancellationToken token = default(CancellationToken));
    }
}
