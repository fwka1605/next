using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Matching : ITransactionData, ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public long ReceiptId { get; set; }
        [DataMember] public long BillingId { get; set; }
        [DataMember] public long MatchingHeaderId { get; set; }
        [DataMember] public decimal BankTransferFee { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public decimal BillingRemain { get; set; }
        [DataMember] public decimal ReceiptRemain { get; set; }
        [DataMember] public int AdvanceReceivedOccured { get; set; }
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public decimal TaxDifference { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string Memo { get; set; }
        [DataMember] public int UseCashOnDueDates { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public decimal? DiscountAmount { get; set; }
        [DataMember] public decimal DiscountAmount1 { get; set; }
        [DataMember] public decimal DiscountAmount2 { get; set; }
        [DataMember] public decimal DiscountAmount3 { get; set; }
        [DataMember] public decimal DiscountAmount4 { get; set; }
        [DataMember] public decimal DiscountAmount5 { get; set; }
        [DataMember] public bool IsNetting { get; set; }
        [DataMember] public decimal OffsetAmount { get; set; }
        [DataMember] public DateTime? OutputAt { get; set; }

        [DataMember] public long? ReceiptHeaderId { get; set; }


        public IEnumerable<MatchingBillingDiscount> ConvertToMatchingBillingDiscounts()
        {
            if (DiscountAmount1 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = Id,
                    DiscountType = 1,
                    DiscountAmount = DiscountAmount1
                };
            if (DiscountAmount2 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = Id,
                    DiscountType = 2,
                    DiscountAmount = DiscountAmount2
                };
            if (DiscountAmount3 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = Id,
                    DiscountType = 3,
                    DiscountAmount = DiscountAmount3
                };
            if (DiscountAmount4 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = Id,
                    DiscountType = 4,
                    DiscountAmount = DiscountAmount4
                };
            if (DiscountAmount5 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = Id,
                    DiscountType = 5,
                    DiscountAmount = DiscountAmount5
                };
        }
        public bool AnyDiscountInputted
        {
            get
            {
                return DiscountAmount1 != 0M
                    || DiscountAmount2 != 0M
                    || DiscountAmount3 != 0M
                    || DiscountAmount4 != 0M
                    || DiscountAmount5 != 0M;
            }
        }
    }

    [DataContract]
    public class MatchingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; } = new ProcessResult();
        [DataMember] public Matching Matching { get; set; }
        [DataMember] public MatchingErrorType MatchingErrorType { get; set; }
        /// <summary>消込実施の結果</summary>
        [DataMember] public List<Matching> Matchings { get; set; }
        /// <summary>前受発生の入金情報</summary>
        [DataMember] public List<AdvanceReceived> AdvanceReceiveds { get; set; }
        /// <summary>消込解除時に、戻し処理を行った前受入金情報</summary>
        [DataMember] public List<Receipt> DeleteReceipts { get; set; }
        /// <summary>一括消込/消込解除 に 失敗した Collation/MatchingHeader の Index</summary>
        [DataMember] public int? ErrorIndex { get; set; }
        /// <summary>相殺データを入金データに変換した結果</summary>
        [DataMember] public List<Receipt> NettingReceipts { get; set; }
    }

    [DataContract]
    public class MatchingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public int RemainType { get; set; }
        [DataMember] public List<Matching> Matchings { get; set; }
        /// <summary>
        ///  消込に利用する請求データの <see cref="Billing.RemainAmount"/>  の合計 消込直前に値のチェックを行う
        /// </summary>
        [DataMember] public decimal BillingRemainTotal { get; set; }
        /// <summary>
        ///  消込に利用する入金データの <see cref="Receipt.RemainAmount"/> の合計 消込直前に値のチェックを行う
        /// </summary>
        [DataMember] public decimal ReceiptRemainTotal { get; set; }
        [DataMember] public decimal BillingDiscountTotal { get; set; }
    }

    /// <summary>
    /// 消込エラータイプ
    /// </summary>
    public enum MatchingErrorType
    {
        /// <summary>エラーなし</summary>
        None = 0,
        /// <summary>請求残相違</summary>
        BillingRemainChanged,
        /// <summary>請求歩引額相違</summary>
        BillingDiscountChanged,
        /// <summary>入金残相違</summary>
        ReceiptRemainChanged,
        /// <summary>請求(論理)削除済</summary>
        BillingOmitted,
        /// <summary>入金(論理)削除済</summary>
        ReceiptOmitted,
        /// <summary>期日現金管理(論理)削除済</summary>
        CashOnDueDateOmitted,
        /// <summary>消込取消 請求データなし</summary>
        NotExistBillingData,
        /// <summary>消込取消 入金データなし</summary>
        NotExistReceiptData,
        /// <summary>消込取消 汎用</summary>
        CancelError,
        /// <summary>処理キャンセル</summary>
        ProcessCanceled,
        /// <summary>なんらかDBエラー</summary>
        DBError,
        /// <summary>連携処理エラー</summary>
        PostProcessError,
        /// <summary>消込ヘッダー変更済/承認フラグの書き換え</summary>
        MatchingHeaderChanged,
    }
}
