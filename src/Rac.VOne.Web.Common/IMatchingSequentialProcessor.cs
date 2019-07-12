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
    public interface IMatchingSequentialProcessor
    {
        Task<MatchingResult> MatchAsync(
            IEnumerable<Collation> collations,
            CollationSearch option,
            CancellationToken token = default(CancellationToken),
            IProgressNotifier notifier = null);
    }
}
