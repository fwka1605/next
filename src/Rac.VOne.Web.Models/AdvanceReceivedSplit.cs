using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class AdvanceReceivedSplit
    {
        [DataMember] public DateTime? ProcessingAt { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int? StaffId { get; set; }
        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public string Memo { get; set; } = string.Empty;
        [DataMember] public string Note1 { get; set; } = string.Empty;
        [DataMember] public string Note2 { get; set; } = string.Empty;
        [DataMember] public string Note3 { get; set; } = string.Empty;
        [DataMember] public string Note4 { get; set; } = string.Empty;

        public Receipt ConvertToReceipt(Receipt original, int advanceReceiptCategoryId,  int LoginUserId)
            => new Receipt {
                CompanyId           = original.CompanyId,
                CurrencyId          = original.CurrencyId,
                ReceiptHeaderId     = original.ReceiptHeaderId,
                ReceiptCategoryId   = advanceReceiptCategoryId,
                CustomerId          = CustomerId,
                SectionId           = original.SectionId,
                InputType           = original.InputType,
                Apportioned         = 1,
                Approved            = 1,
                Workday             = original.Workday,
                RecordedAt          = original.RecordedAt,
                ReceiptAmount       = ReceiptAmount,
                AssignmentAmount    = 0,
                RemainAmount        = ReceiptAmount,
                AssignmentFlag      = 0,
                PayerCode           = original.PayerCode,
                PayerName           = original.PayerName,
                PayerNameRaw        = original.PayerNameRaw,
                SourceBankName      = original.SourceBankName,
                SourceBranchName    = original.SourceBranchName,
                DueAt               = original.DueAt,
                MailedAt            = original.MailedAt,
                OriginalReceiptId   = original.Id,
                ExcludeFlag         = 0,
                ExcludeAmount       = 0,
                ReferenceNumber     = original.ReferenceNumber,
                RecordNumber        = original.RecordNumber,
                DensaiRegisterAt    = original.DensaiRegisterAt,
                Note1               = Note1,
                Note2               = Note2,
                Note3               = Note3,
                Note4               = Note4,
                BillNumber          = original.BillNumber,
                BillBankCode        = original.BillBankCode,
                BillBranchCode      = original.BillBranchCode,
                BillDrawAt          = original.BillDrawAt,
                BillDrawer          = original.BillDrawer,
                CreateBy            = LoginUserId,
                UpdateBy            = LoginUserId,
                ProcessingAt        = ProcessingAt,
                StaffId             = StaffId,
                CollationKey        = original.CollationKey,
                BankCode            = original.BankCode,
                BankName            = original.BankName,
                BranchCode          = original.BranchCode,
                BranchName          = original.BranchName,
                AccountTypeId       = original.AccountTypeId,
                AccountNumber       = original.AccountNumber,
                AccountName         = original.AccountName,
            };
    }
}
