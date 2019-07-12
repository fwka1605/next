using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingCancellationProcessor
    {
        Task<MatchingResult> CancelAsync(IEnumerable<MatchingHeader> headers, int loginUserId,
            CancellationToken token = default(CancellationToken), IProgressNotifier notifier = null);
    }
}
