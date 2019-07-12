using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAuthenticationQueryProcessor
    {
        Task<bool> ExistsAuthencticationKeyAsync(string AuthenticationKey, CancellationToken token = default(CancellationToken));
        Task<bool> IsConnectableAsync(IConnectionFactory factory, CancellationToken token = default(CancellationToken));

        Task<string> GetConnectionStringAsync(string tenantCode, CancellationToken token = default(CancellationToken));
        Task<string> CreateSessionKeyAsync(string connectionString, CancellationToken token = default(CancellationToken));
    }
}
