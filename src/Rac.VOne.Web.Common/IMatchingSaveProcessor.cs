using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingSaveProcessor
    {
        Task<MatchingResult> SaveAsync(
            MatchingSource source,
            ApplicationControl applicationControl,
            CancellationToken token = default(CancellationToken));
    }
}
