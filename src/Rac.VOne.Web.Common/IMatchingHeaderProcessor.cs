using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingHeaderProcessor
    {
        Task<MatchingHeadersResult> ApproveAsync(IEnumerable<MatchingHeader> headers, CancellationToken token = default(CancellationToken));
        Task<MatchingHeadersResult> CancelAsync(IEnumerable<MatchingHeader> headers, CancellationToken token = default(CancellationToken));
    }
}
