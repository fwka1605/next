using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IAdvanceReceivedSplitProcessor
    {
        Task<int> SplitAsync(AdvanceReceivedSplitSource source, CancellationToken token = default(CancellationToken));
        Task<int> CancelAsync(AdvanceReceivedSplitSource source, CancellationToken token = default(CancellationToken));

    }
}
