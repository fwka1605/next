using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingSource
    {
        [DataMember] public MatchingHeader MatchingHeader { get; set; }
        [DataMember] public List<Matching> Matchings { get; set; }
        [DataMember] public List<Billing> Billings { get; set; }
        [DataMember] public List<Receipt> Receipts { get; set; }
        [DataMember] public List<MatchingBillingDiscount> MatchingBillingDiscounts { get; set; }
        [DataMember] public IEnumerable<long> BillingDiscounts { get; set; }
        [DataMember] public List<BillingScheduledIncome> BillingScheduledIncomes { get; set; }
        /// <summary>
        /// 消込に利用する請求データの 消込前 請求残<see cref="Billing.RemainAmount"/>合計 DBチェック用
        /// </summary>
        [DataMember] public decimal BillingRemainTotal { get; set; }
        /// <summary>
        /// 消込に利用する入金データの 消込前 入金残<see cref="Receipt.RemainAmount"/>合計 DBチェック用
        /// </summary>
        [DataMember] public decimal ReceiptRemainTotal { get; set; }
        [DataMember] public decimal BillingDiscountTotal { get; set; }
        [DataMember] public int RemainType { get; set; }

        #region 消込 組み合わせ問合せ用
        [DataMember] public decimal TaxDifference { get; set; }
        [DataMember] public decimal BankTransferFee { get; set; }
        #endregion
        #region 消込用
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public List<int> ChildCustomerIds { get; set; }
        [DataMember] public int? AdvanceReceivedCustomerId { get; set; }
        [DataMember] public int MatchingProcessType { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public int? PaymentAgencyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int UseKanaLearning { get; set; }
        [DataMember] public int UseFeeLearning { get; set; }
        [DataMember] public CollationSearch Option { get; set; }
        public DateTime UpdateAt { get; set; }
        /// <summary>消込区分 0:一括消込, 1:個別消込</summary>
        #endregion
    }

    [DataContract]
    public class MatchingSourceResult : IProcessResult
    {
        [DataMember] public MatchingSource MatchingSource { get; set; }
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }
}
