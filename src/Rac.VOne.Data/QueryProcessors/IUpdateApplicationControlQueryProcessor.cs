using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateApplicationControlQueryProcessor
    {
        Task<int> UpdateUseOperationLogDataAsync(ApplicationControl AppData, CancellationToken token = default(CancellationToken));
    }
}
