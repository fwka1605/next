using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IAuthenticationWebApiProcessor
    {
        Task<ProcessResult> AuthenticateAsync(string authenticationKey, string tenantCode, CancellationToken token = default(CancellationToken));

        Task<ProcessResult> CreateSessionAsync(string tenantCode, CancellationToken token = default(CancellationToken));

    }
}
