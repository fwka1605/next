using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IInputControlProcessor
    {
        Task<IEnumerable<InputControl>> GetAsync(InputControl control, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<InputControl>> SaveAsync(IEnumerable<InputControl> controls, CancellationToken token = default(CancellationToken));
    }
}
