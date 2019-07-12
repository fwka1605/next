using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateMatchingHeaderQueryProcessor
    {
        Task<MatchingHeader> ApproveAsync(MatchingHeader header, CancellationToken token = default(CancellationToken));
        Task<MatchingHeader> CancelApprovalAsync(MatchingHeader header, CancellationToken token = default(CancellationToken));
    }
}
