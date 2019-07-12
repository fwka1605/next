using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    /// <summary>請求仕訳出力系</summary>
    public interface IBillingJournalizingProcessor
    {
        /// <summary>請求仕訳出力履歴 取得</summary>
        Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));
        /// <summary>請求仕訳出力 抽出</summary>
        Task<IEnumerable<BillingJournalizing>> ExtractAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));
        /// <summary>請求仕訳出力 仕訳出力済フラグ更新処理</summary>
        Task<IEnumerable<Billing>> UpdateAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));
        /// <summary>請求仕訳出力 仕訳出力済フラグキャンセル処理</summary>
        Task<IEnumerable<Billing>> CancelAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));

    }
}
