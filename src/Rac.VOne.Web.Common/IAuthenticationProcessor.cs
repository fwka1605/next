using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IAuthenticationProcessor
    {
        Task<AuthenticationResult> AuthenticateAsync(string authenticationKey, string companyCode, string userCode, CancellationToken token = default(CancellationToken));
    }
}
