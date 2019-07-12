using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingHistorySearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember]public int LoginUserId {get;set;}
        [DataMember] public int OutputOrder { get; set; }
        [DataMember] public DateTime? InputAtFrom { get; set; }
        [DataMember] public DateTime? InputAtTo { get; set; }
        [DataMember] public DateTime? RecordedAtFrom { get; set; }
        [DataMember] public DateTime? RecordedAtTo { get; set; }
        [DataMember] public long? ReceiptIdFrom { get; set; }
        [DataMember] public long? ReceiptIdTo { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public int? AccountType { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public DateTime? CreateAtFrom { get; set; }
        [DataMember] public DateTime? CreateAtTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string SectionCodeFrom { get; set; }
        [DataMember] public string SectionCodeTo { get; set; }
        [DataMember] public bool UseSectionMaster { get; set; }
        [DataMember] public string LoginUserFrom { get; set; }
        [DataMember] public string LoginUserTo { get; set; }
        [DataMember] public bool ExistsMemo { get; set; }
        [DataMember] public string Memo { get; set; }
        [DataMember] public decimal? BillingAmountFrom { get; set; }
        [DataMember] public decimal? BillingAmountTo { get; set; }
        [DataMember] public decimal? ReceiptAmountFrom { get; set; }
        [DataMember] public decimal? ReceiptAmountTo { get; set; }
        [DataMember] public int? BillingCategoryId { get; set; }
        [DataMember] public bool OnlyNonOutput { get; set; }
        /// <summary>
        /// 消込処理方法 0 : 一括, 1 : 個別, null : すべて
        /// </summary>
        [DataMember] public int? MatchingProcessType { get; set; }
        [DataMember] public string PayerName { get; set; }

        [DataMember] public bool RequireSubtotal { get; set; }

        public string ConnectionId { get; set; }
    }
}
