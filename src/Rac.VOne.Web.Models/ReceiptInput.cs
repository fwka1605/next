using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReceiptInput : ITransactionData
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int? ReceiptHeaderId { get; set; }
        [DataMember] public int ReceiptCategoryId { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int InputType { get; set; }
        [DataMember] public int Apportioned { get; set; }
        [DataMember] public int Approved { get; set; }
        [DataMember] public DateTime Workday { get; set; }
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public DateTime? OriginalRecordedAt { get; set; }
        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public decimal AssignmentAmount { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public string PayerCode { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string PayerNameRaw { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public DateTime? MailedAt { get; set; }
        [DataMember] public long? OriginalReceiptId { get; set; }
        [DataMember] public int ExcludeFlag { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public decimal ExcludeAmount { get; set; }
        [DataMember] public string ReferenceNumber { get; set; }
        [DataMember] public string RecordNumber { get; set; }
        [DataMember] public DateTime? DensaiRegisterAt { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string BillNumber { get; set; }
        [DataMember] public string BillBankCode { get; set; }
        [DataMember] public string BillBranchCode { get; set; }
        [DataMember] public DateTime? BillDrawAt { get; set; }
        [DataMember] public string BillDrawer { get; set; }
        [DataMember] public DateTime? DeleteAt { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string Memo { get; set; }
        [DataMember] public bool LearnKanaHistory { get; set; }

        [DataMember] public int LineNo { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string ReceiptCategoryCode { get; set; }
        [DataMember] public string RecordAtForPrint { get; set; }
        [DataMember] public string DueAtForPrint { get; set; }
        [DataMember] public string BillDrawAtForPrint { get; set; }
        [DataMember] public string ReceiptAmountForPrint { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string CollationKey { get; set; } = string.Empty;
        [DataMember] public string BankCode { get; set; } = string.Empty;
        [DataMember] public string BankName { get; set; } = string.Empty;
        [DataMember] public string BranchCode { get; set; } = string.Empty;
        [DataMember] public string BranchName { get; set; } = string.Empty;
        [DataMember] public int? AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; } = string.Empty;
        [DataMember] public string AccountName { get; set; } = string.Empty;

    }
    [DataContract]
    public class ReceiptInputResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public ReceiptInput ReceiptInput { get; set; }
    }

    [DataContract]
    public class ReceiptInputsResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public List<ReceiptInput> ReceiptInputs { get; set; }
    }
}
