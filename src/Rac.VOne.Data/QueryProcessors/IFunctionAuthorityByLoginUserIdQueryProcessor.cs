using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IFunctionAuthorityByLoginUserIdQueryProcessor
    {
        Task<IEnumerable<FunctionAuthority>> GetAsync(FunctionAuthoritySearch option, CancellationToken token = default(CancellationToken));
    }
}
