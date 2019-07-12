using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IMatchingQueryProcessor
    {
        Task<IEnumerable<Billing>> GetBillingsForSequentialMatchingAsync(MatchingBillingSearch searchOption, IEnumerable<MatchingOrder> orders, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Receipt>> GetReceiptsForSequentialMatchingAsync(MatchingReceiptSearch searchOption, IEnumerable<MatchingOrder> orders, CancellationToken token = default(CancellationToken));
        Task<int> UpdateReceiptHeaderAsync(long Id, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken));

        /// <summary>
        ///  消込用 請求データ取得 個別消込画面表示
        /// </summary>
        /// <param name="option"></param>
        /// <param name="MatchingOrder"></param>
        /// <returns></returns>
        Task<IEnumerable<Billing>> GetBillingsForMatchingAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken));


        /// <summary>
        ///  消込用 入金データ取得 個別消込画面表示
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<Receipt>> GetReceiptsForMatchingAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Netting>> SearchMatchingNettingAsync(CollationSearch CollationSearch, Collation Collation, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MatchingHeader>> SearchMatchedDataAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 消込済請求データ取得 個別消込画面表示用
        /// </summary>
        /// <param name="option"></param>
        /// <param name="MatchingOrderList"></param>
        /// <returns></returns>
        Task<IEnumerable<Billing>> GetMatchedBillingsAsync(MatchingBillingSearch option, CancellationToken token = default(CancellationToken));
        /// <summary>
        /// 消込済入金データ取得 個別消込画面表示用
        /// </summary>
        /// <param name="option"></param>
        /// <param name="MatchingOrderList"></param>
        /// <returns></returns>
        Task<IEnumerable<Receipt>> GetMatchgedReceiptsAsync(MatchingReceiptSearch option, CancellationToken token = default(CancellationToken));


        Task<Receipt> SaveMatchingReceiptAsync(ReceiptInput receiptInput, CancellationToken token = default(CancellationToken));

        Task<Billing> GetBillingRemainAmountAsync(IEnumerable<long> BillingIds, CancellationToken token = default(CancellationToken));
        Task<decimal> GetReceiptRemainAmountAsync(IEnumerable<long> ReceiptIds, CancellationToken token = default(CancellationToken));
        Task<decimal> GetNettingRemainAmountAsync(IEnumerable<long> NettingIds, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Receipt>> SearchReceiptByIdAsync(IEnumerable<long> ReceiptId, CancellationToken token = default(CancellationToken));


        Task<int> SaveWorkReceiptTargetAsync(byte[] ClientKey, long ReceiptId,
            int CompanyId, int CurrencyId, string PayerName, string BankCode, string BranchCode, string PayerCode, string SourceBankName, string SourceBranchName, string CollationKey, int? CustomerId,
            CancellationToken token = default(CancellationToken));
        Task<int> SaveWorkCollationAsync(byte[] ClientKey, long ReceiptId, int ParentCustomerId, int PaymentAgencyId, int CurrencyId,
            string PayerName, string PayerCode, string BankCode, string BranchCode, string SourceBankName, string SourceBranchName, string CollationKey, decimal ReceiptAmount,
            CancellationToken token = default(CancellationToken));

    }
}
