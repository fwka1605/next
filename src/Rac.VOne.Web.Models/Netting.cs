using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Netting : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int ReceiptCategoryId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public long? ReceiptId { get; set; }
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string ReceiptMemo { get; set; }

        //Other table
        [DataMember] public string CategoryCode { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerKana { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string UseAdvanceReceived { get; set; }


        public ReceiptInput ConvertToReceiptInput(int LoginUserId, DateTime CreateAt)
        {
            return new ReceiptInput
            {
                CompanyId = this.CompanyId,
                CurrencyId = this.CurrencyId,
                ReceiptHeaderId = null,
                ReceiptCategoryId = this.ReceiptCategoryId,
                CustomerId = this.CustomerId,
                SectionId = this.SectionId,
                InputType = 2,
                Apportioned = 1,
                Approved = 1,
                Workday = DateTime.Today,
                RecordedAt = this.RecordedAt,
                OriginalRecordedAt = null,
                ReceiptAmount = this.Amount,
                AssignmentAmount = 0,
                RemainAmount = this.Amount,
                AssignmentFlag = 0,
                PayerCode = "",
                PayerName = this.CustomerKana,
                PayerNameRaw = this.CustomerKana,
                SourceBankName = "",
                SourceBranchName = "",
                OutputAt = null,
                DueAt = this.DueAt,
                MailedAt = null,
                OriginalReceiptId = null,
                ExcludeFlag = 0,
                ExcludeCategoryId = null,
                ExcludeAmount = 0,
                ReferenceNumber = "",
                RecordNumber = "",
                DensaiRegisterAt = null,
                Note1 = this.Note,
                Note2 = "",
                Note3 = "",
                Note4 = "",
                BillNumber = "",
                BillBankCode = "",
                BillBranchCode = "",
                BillDrawAt = null,
                BillDrawer = "",
                DeleteAt = null,
                CreateBy = LoginUserId,
                CreateAt = CreateAt,
                UpdateBy = LoginUserId,
                UpdateAt = CreateAt,
                Memo = this.ReceiptMemo,
                LearnKanaHistory = false,
            };
        }
    }

    [DataContract]
    public class NettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Netting[] Netting { get; set; }
    }

    [DataContract]
    public class NettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Netting> Nettings { get; set; }
    }
}
