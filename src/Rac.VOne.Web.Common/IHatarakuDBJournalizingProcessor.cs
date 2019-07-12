using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IHatarakuDBJournalizingProcessor
    {
        Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<HatarakuDBData>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
    }
}
