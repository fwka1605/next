using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingSolveProcessor
    {
        Task<MatchingSource> SolveAsync(
            MatchingSource source,
            CollationSearch option,
            ApplicationControl control = null,
            CancellationToken token = default(CancellationToken));
    }
}
