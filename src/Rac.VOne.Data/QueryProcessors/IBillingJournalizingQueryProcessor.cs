using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    /// <summary>
    /// 請求仕訳出力関連
    /// </summary>
    public interface IBillingJournalizingQueryProcessor
    {
        Task<IEnumerable<JournalizingSummary>> GetAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<BillingJournalizing>> ExtractAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Billing>> UpdateAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Billing>> CancelAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));

    }
}
