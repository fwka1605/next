using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IAdvanceReceivedProcessor
    {
        Task<IEnumerable<Receipt>> GetAdvanceReceiptsAsync(long originalReceiptId, CancellationToken token = default(CancellationToken));
        Task<AdvanceReceivedResult> SaveAsync(IEnumerable<AdvanceReceived> receiveds, CancellationToken token = default(CancellationToken));

        Task<AdvanceReceivedResult> CancelAsync(IEnumerable<AdvanceReceived> receiveds, CancellationToken token = default(CancellationToken));
    }
}
