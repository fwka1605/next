using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IMatchingHistorySearchQueryProcessor
    {
        Task<IEnumerable<MatchingHistory>> GetAsync(MatchingHistorySearch searchOption, CancellationToken token, IProgressNotifier notifier);

    }
}
