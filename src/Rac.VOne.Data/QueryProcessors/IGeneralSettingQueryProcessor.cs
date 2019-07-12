using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IGeneralSettingQueryProcessor
    {
        Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken));
    }
}
