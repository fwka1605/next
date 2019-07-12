using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    /// <summary>
    /// 消込仕訳出力 消込済入金データ出力 汎用仕訳出力 関連の interface
    /// Matching だけではなく、前受、前受振替、対象外入金 も範囲内
    /// </summary>
    public interface IMatchingJournalizingProcessor
    {
        Task<IEnumerable<JournalizingSummary>> GetSummaryAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MatchingJournalizing>> ExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MatchedReceipt>> GetMatchedReceiptAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<GeneralJournalizing>> ExtractGeneralJournalizingAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));


        /// <summary>出力済フラグ更新</summary>
        Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        /// <summary>出力済 取消</summary>
        Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));

        /// <summary>消込取消 明細取得</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<MatchingJournalizingDetail>> GetDetailsAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        /// <summary>消込取消 における　取消処理</summary>
        /// <param name="details"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> CancelDetailsAsync(IEnumerable<MatchingJournalizingDetail> details, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MatchingJournalizing>> MFExtractAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
    }
}
