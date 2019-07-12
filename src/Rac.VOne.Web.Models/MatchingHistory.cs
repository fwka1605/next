using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingHistory
    {
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public DateTime? BilledAt { get; set; }
        [DataMember] public DateTime? SalesAt { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string BillingCategoryCode { get; set; }
        [DataMember] public string BillingCategoryName { get; set; }
        [DataMember] public decimal? BillingAmount { get; set; }
        [DataMember] public decimal? BillingAmountExcludingTax { get; set; }
        [DataMember] public decimal? TaxAmount { get; set; }
        [DataMember] public decimal? MatchingAmount { get; set; }
        [DataMember] public decimal? BillingRemain { get; set; }
        [DataMember] public string BillingNote1 { get; set; }
        [DataMember] public string BillingNote2 { get; set; }
        [DataMember] public string BillingNote3 { get; set; }
        [DataMember] public string BillingNote4 { get; set; }
        [DataMember] public DateTime? RecordedAt { get; set; }
        [DataMember] public long? ReceiptId { get; set; }
        [DataMember] public string ReceiptCategoryCode { get; set; }
        [DataMember] public string ReceiptCategoryName { get; set; }
        [DataMember] public decimal? ReceiptAmount { get; set; }
        [DataMember] public bool AdvanceReceivedOccured { get; set; }
        [DataMember] public decimal? ReceiptRemain { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string ReceiptNote1 { get; set; }
        [DataMember] public string ReceiptNote2 { get; set; }
        [DataMember] public string ReceiptNote3 { get; set; }
        [DataMember] public string ReceiptNote4 { get; set; }
        [DataMember] public string PayerCode { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
        [DataMember] public string LoginUserName { get; set; }
        [DataMember] public string MatchingProcessType { get; set; }
        [DataMember] public string MatchingMemo { get; set; }
        [DataMember] public long MatchingId { get; set; }
        [DataMember] public decimal? TaxDifference { get; set; }
        [DataMember] public decimal? BankTransferFee { get; set; }
        [DataMember] public long MatchingHeaderId { get; set; }
        [DataMember] public long BillingId { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }

        /// <summary>
        ///  請求区分 コード：名称 <see cref="BillingCategoryCode"/>：<see cref="BillingCategoryName"/>
        /// </summary>
        public string BillingCategory
        { get { return $"{BillingCategoryCode}：{BillingCategoryName}"; } }

        public string CollectCategory
        { get { return !string.IsNullOrEmpty(CollectCategoryCode) && !string.IsNullOrEmpty(CollectCategoryName)
                    ? $"{CollectCategoryCode}：{CollectCategoryName}" : ""; } }

        /// <summary>
        ///  入金区分 コード：名称 <see cref="ReceiptCategoryCode"/>：<see cref="ReceiptCategoryName"/>
        /// </summary>
        public string ReceiptCategory
        { get { return $"{ReceiptCategoryCode}：{ReceiptCategoryName}"; } }

        /// <summary>
        /// 前受発生 有りなら「前」、無しなら空文字を返す
        /// </summary>
        public string AdvanceReceivedOccuredString
        { get { return AdvanceReceivedOccured ? "前" : ""; } }

        /// <summary>
        /// 消込処理方法 「一括」または「個別」を返す
        /// </summary>
        public string MatchingProcessTypeString
        { get { return MatchingProcessType == "0" ? "一括" : "個別"; } }

        public string VirtualBranchCode
        { get { return (PayerCode != null && PayerCode.Length >= 3) ? PayerCode.Substring(0, 3) : ""; } }

        public string VirtualAccountNumber
        { get { return (PayerCode != null && PayerCode.Length >= 10) ? PayerCode.Substring(3, 7) : ""; } }
    }

    [DataContract]
    public class MatchingHistoryResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MatchingHistory MatchingHistory { get; set; }
    }

    [DataContract]
    public class MatchingHistorysResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchingHistory> MatchingHistorys { get; set; }
    }
}
