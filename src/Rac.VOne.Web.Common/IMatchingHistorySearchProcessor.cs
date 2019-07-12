using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Common
{
    public interface IMatchingHistorySearchProcessor
    {
        Task<IEnumerable<MatchingHistory>> GetAsync(MatchingHistorySearch seaerchOption, CancellationToken token, IProgressNotifier notifier);
    }
}
