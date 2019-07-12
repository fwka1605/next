using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchedReceipt
    {
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public long SlipNumber { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public string ReceiptCategoryCode { get; set; }
        [DataMember] public string ReceiptCategoryName { get; set; }
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public long Id { get; set; }
        [DataMember] public string BillingNote1 { get; set; }
        [DataMember] public string BillingNote2 { get; set; }
        [DataMember] public string BillingNote3 { get; set; }
        [DataMember] public string BillingNote4 { get; set; }
        [DataMember] public string ReceiptNote1 { get; set; }
        [DataMember] public string ReceiptNote2 { get; set; }
        [DataMember] public string ReceiptNote3 { get; set; }
        [DataMember] public string ReceiptNote4 { get; set; }
        [DataMember] public string BillNumber { get; set; }
        [DataMember] public string BillBankCode { get; set; }
        [DataMember] public string BillBranchCode { get; set; }
        [DataMember] public DateTime? BillDrawAt { get; set; }
        [DataMember] public string BillDrawer { get; set; }
        [DataMember] public string BillingMemo { get; set; }
        [DataMember] public string ReceiptMemo { get; set; }
        [DataMember] public string MatchingMemo { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public string VirtualBranchCode { get; set; }
        [DataMember] public string VirtualAccountNumber { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
        [DataMember] public string ReceiptCategoryExternalCode { get; set; }
        [DataMember] public long OriginalReceiptId { get; set; }
        [DataMember] public string JournalizingCategory { get; set; }
    }

    [DataContract]
    public class MatchedReceiptResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MatchedReceipt MatchedReceipt { get; set; }
    }

    [DataContract]
    public class MatchedReceiptsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchedReceipt> MatchedReceipts { get; set; }
    }
}
