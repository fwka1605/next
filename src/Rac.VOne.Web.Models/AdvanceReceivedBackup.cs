using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class AdvanceReceivedBackup : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public string PrivateBankCode { get; set; }
        [DataMember] public long? ReceiptHeaderId { get; set; }
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
        [DataMember] public string PayerCode { get; set; } = string.Empty;
        [DataMember] public string PayerName { get; set; } = string.Empty;
        [DataMember] public string PayerNameRaw { get; set; } = string.Empty;
        [DataMember] public string SourceBankName { get; set; } = string.Empty;
        [DataMember] public string SourceBranchName { get; set; } = string.Empty;
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public DateTime? MailedAt { get; set; }
        [DataMember] public long? OriginalReceiptId { get; set; }
        [DataMember] public int ExcludeFlag { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public decimal ExcludeAmount { get; set; }
        [DataMember] public string ReferenceNumber { get; set; } = string.Empty;
        [DataMember] public string RecordNumber { get; set; } = string.Empty;
        [DataMember] public DateTime? DensaiRegisterAt { get; set; }
        [DataMember] public string Note1 { get; set; } = string.Empty;
        [DataMember] public string Note2 { get; set; } = string.Empty;
        [DataMember] public string Note3 { get; set; } = string.Empty;
        [DataMember] public string Note4 { get; set; } = string.Empty;
        [DataMember] public string BillNumber { get; set; } = string.Empty;
        [DataMember] public string BillBankCode { get; set; } = string.Empty;
        [DataMember] public string BillBranchCode { get; set; } = string.Empty;
        [DataMember] public DateTime? BillDrawAt { get; set; }
        [DataMember] public string BillDrawer { get; set; } = string.Empty;
        [DataMember] public DateTime? DeleteAt { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public string Memo { get; set; } = string.Empty;
        [DataMember] public DateTime? TransferOutputAt { get; set; }
        [DataMember] public int? StaffId { get; set; }
        [DataMember] public string CollationKey { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public int? AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string AccountName { get; set; }


        public Receipt ConvertToReceipt(int LoginUserId)
            => new Receipt {
                CompanyId           = this.CompanyId,
                CurrencyId          = this.CurrencyId,
                ReceiptHeaderId     = this.ReceiptHeaderId,
                ReceiptCategoryId   = this.ReceiptCategoryId,
                CustomerId          = this.CustomerId,
                SectionId           = this.SectionId,
                InputType           = this.InputType,
                Apportioned         = this.Apportioned,
                Approved            = this.Approved,
                Workday             = this.Workday,
                RecordedAt          = this.RecordedAt,
                OriginalRecordedAt  = this.OriginalRecordedAt,
                ReceiptAmount       = this.ReceiptAmount,
                AssignmentAmount    = this.AssignmentAmount,
                RemainAmount        = this.RemainAmount,
                AssignmentFlag      = this.AssignmentFlag,
                PayerCode           = this.PayerCode,
                PayerName           = this.PayerName,
                PayerNameRaw        = this.PayerNameRaw,
                SourceBankName      = this.SourceBankName,
                SourceBranchName    = this.SourceBranchName,
                OutputAt            = this.OutputAt,
                DueAt               = this.DueAt,
                MailedAt            = this.MailedAt,
                OriginalReceiptId   = this.OriginalReceiptId,
                ExcludeFlag         = this.ExcludeFlag,
                ExcludeCategoryId   = this.ExcludeCategoryId,
                ExcludeAmount       = this.ExcludeAmount,
                ReferenceNumber     = this.ReferenceNumber,
                RecordNumber        = this.RecordNumber,
                DensaiRegisterAt    = this.DensaiRegisterAt,
                Note1               = this.Note1,
                Note2               = this.Note2,
                Note3               = this.Note3,
                Note4               = this.Note4,
                BillNumber          = this.BillNumber,
                BillBankCode        = this.BillBankCode,
                BillBranchCode      = this.BillBranchCode,
                BillDrawAt          = this.BillDrawAt,
                BillDrawer          = this.BillDrawer,
                DeleteAt            = this.DeleteAt,
                CreateBy            = this.CreateBy,
                CreateAt            = this.CreateAt,
                UpdateBy            = LoginUserId,
                StaffId             = this.StaffId,
                CollationKey        = this.CollationKey,
                BankCode            = this.BankCode,
                BankName            = this.BankName,
                BranchCode          = this.BranchCode,
                BranchName          = this.BranchName,
                AccountTypeId       = this.AccountTypeId,
                AccountNumber       = this.AccountNumber,
                AccountName         = this.AccountName,
            };

    }

    [DataContract]
    public class AdvanceReceivedBackupResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public AdvanceReceivedBackup AdvanceReceivedBackup { get; set; }
    }
    
    [DataContract]
    public class AdvanceReceivedBackupsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<AdvanceReceivedBackup> AdvanceReceivedBackups { get; set; }
    }
}
