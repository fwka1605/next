using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ILogsQueryProcessor
    {
         Task<int> SaveAsync(Logs log, CancellationToken token = default(CancellationToken));

    }
}
