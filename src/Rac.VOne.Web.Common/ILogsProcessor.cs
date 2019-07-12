using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ILogsProcessor
    {
        Task<int> SaveAsync(Logs log, CancellationToken token = default(CancellationToken));
    }
}
