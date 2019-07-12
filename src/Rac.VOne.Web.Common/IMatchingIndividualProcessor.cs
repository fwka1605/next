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
    public interface IMatchingIndividualProcessor
    {
        Task<MatchingResult> MatchAsync(
            MatchingSource source,
            CancellationToken token = default(CancellationToken),
            IProgressNotifier notifier = null);
    }
}
