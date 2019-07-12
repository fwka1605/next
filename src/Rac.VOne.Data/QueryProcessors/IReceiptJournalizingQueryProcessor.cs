using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptJournalizingQueryProcessor
    {

        Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptJournalizing>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));

    }
}
