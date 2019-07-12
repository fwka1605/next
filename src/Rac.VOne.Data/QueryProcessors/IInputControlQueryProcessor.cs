using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IInputControlQueryProcessor
    {
        Task<IEnumerable<InputControl>> GetAsync(InputControl control, CancellationToken token = default(CancellationToken));
    }
}
