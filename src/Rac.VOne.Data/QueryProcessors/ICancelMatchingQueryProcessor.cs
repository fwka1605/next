using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICancelMatchingQueryProcessor
    {
        Task<Receipt> UpdateReceiptForCancelMatchingAsync(Receipt Receipt, CancellationToken token = default(CancellationToken));
        Task<Billing> UpdateBillingForCancelMatchingAsync(Billing Billing, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Matching>> UpdatePreviousBillingLogsAsync(long MatchingHeaderId, long BillingId, decimal Amount, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Matching>> UpdatePreviousReceiptLogsAsync(long MatchingHeaderId, long ReceiptId, decimal Amount, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken));

        Task<int> DeleteMatchingOutputedAsync(long MatchingHeaderId, CancellationToken token = default(CancellationToken));
        Task<int> DeleteMatchingAsync(long matchingHeaderId, DateTime updateAt, CancellationToken token = default(CancellationToken));
        Task<int> DeleteBillingShceduledIncomeAsync(long BillingId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Matching>> GetByHeaderIdAsync(long MatchingHeaderId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistAssignmentScheduledIncomeAsync(IEnumerable<long> MatchingIds, CancellationToken token = default(CancellationToken));

        /// <summary>前受入金データを 元の入金IDから取得する処理</summary>
        Task<IEnumerable<Receipt>> GetByOriginalIdAsync(long ReceiptId, CancellationToken token = default(CancellationToken));

        Task<BillingScheduledIncome> GetBillingScheduledIncomeAsync(long MatchingId, CancellationToken token = default(CancellationToken));

        /// <summary>消込解除用 消込済請求データ取得</summary>
        Task<IEnumerable<Billing>> GetMatchedBillingsForCancelAsync(long headerId, CancellationToken token = default(CancellationToken));

        /// <summary>消込解除用 消込済入金データ取得</summary>
        Task<IEnumerable<Receipt>> GetMatchedReceiptsForCancelAsync(MatchingHeader MatchingHeader, CancellationToken token = default(CancellationToken));

    }
}
