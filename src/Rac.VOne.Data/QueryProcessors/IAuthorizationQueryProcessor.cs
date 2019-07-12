using Rac.VOne.Data.Entities;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAuthorizationQueryProcessor
    {
        Task<SessionStorage> GetSessionAsync(string SessionKey);
    }
}
