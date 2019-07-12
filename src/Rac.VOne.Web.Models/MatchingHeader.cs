using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingHeader : ITransactionData, ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public int? PaymentAgencyId { get; set; }
        /// <summary>
        ///  承認フラグ  0 :未承認, 1 : 承認済
        /// </summary>
        [DataMember] public int Approved { get; set; }
        [DataMember] public int ReceiptCount { get; set; }
        [DataMember] public int BillingCount { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public decimal BankTransferFee { get; set; }
        [DataMember] public decimal TaxDifference { get; set; }
        /// <summary>
        /// 消込処理方法 0 : 一括, 1 : 個別
        /// </summary>
        [DataMember] public int MatchingProcessType { get; set; }
        [DataMember] public string Memo { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }


        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string DispCustomerCode { get; set; }
        [DataMember] public string DispCustomerName { get; set; }
        [DataMember] public int ShareTransferFee { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string PaymentAgencyCode { get; set; }
        /// <summary>消込解除 同時実行制御確認用 Matching の MAX 値</summary>
        [DataMember] public DateTime MatchingUpdateAt { get; set; }
        /// <summary> 一括消込 請求情報表示順 </summary>
        [DataMember] public int BillingDisplayOrder { get; set; }
        /// <summary> 一括消込 入金情報表示順 </summary>
        [DataMember] public int ReceiptDisplayOrder { get; set; }

        public decimal? DispBillingAmount { get { return BillingAmount; } }
        public decimal? DispReceiptAmount { get { return ReceiptAmount; } }
        public int? DispBillingCount { get { return BillingCount; } }
        public int? DispReceiptCount { get { return ReceiptCount; } }

        public string DispShareTransferFee
        {
            get
            {
                if (ShareTransferFee == 1)
                {
                    return "自";
                }
                else if (ShareTransferFee == 0)
                {
                    return "相";
                }
                else
                {
                    return "";
                }
            }
        }

        public decimal Different
        {
            get { return BillingAmount - ReceiptAmount; }
        }

        public decimal? DispDifferent
        {
            get
            {
                if (DispBillingCount == null || DispReceiptCount == null)
                {
                    return null;

                }
                else
                {
                    return Different;
                }
            }
        }

        public bool Checked { get; set; }
        public string DispMemo { get { return string.IsNullOrEmpty(Memo) ? "" : "○"; } }

    }

    [DataContract]
    public class MatchingHeadersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchingHeader> MatchingHeaders { get; set; }
    }
}
