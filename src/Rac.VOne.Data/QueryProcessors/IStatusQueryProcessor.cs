using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IStatusQueryProcessor
    {
        Task<IEnumerable<Status>> GetAsync(StatusSearch option, CancellationToken token = default(CancellationToken));
    }
}
