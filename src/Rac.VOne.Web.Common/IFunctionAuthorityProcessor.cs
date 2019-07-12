using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IFunctionAuthorityProcessor
    {
        Task<IEnumerable<FunctionAuthority>> GetAsync(FunctionAuthoritySearch option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<FunctionAuthority>> SaveAsync(IEnumerable<FunctionAuthority> authorities, CancellationToken token = default(CancellationToken));
    }
}
