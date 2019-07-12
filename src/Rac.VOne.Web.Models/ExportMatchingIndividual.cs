using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ExportMatchingIndividual
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public int BillingCategoryId { get; set; }
        [DataMember] public int InputType { get; set; }
        [DataMember] public int BillingInputTypeId { get; set; }
        [DataMember] public DateTime? BilledAt { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime? SalesAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public decimal? BillingAmount { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public decimal AssignmentAmount { get; set; }
        [DataMember] public decimal? RemainAmount { get; set; }
        [DataMember] public decimal? TargetAmount { get; set; }
        [DataMember] public decimal OffsetAmount { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public int Approved { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public int? OriginalCollectCategoryId { get; set; }
        [DataMember] public int? DebitAccountTitleId { get; set; }
        [DataMember] public int? CreditAccountTitleId { get; set; }
        [DataMember] public DateTime? OriginalDueAt { get; set; }
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public DateTime? PublishAt { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public int TaxClassId { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string Note5 { get; set; }
        [DataMember] public string Note6 { get; set; }
        [DataMember] public string Note7 { get; set; }
        [DataMember] public string Note8 { get; set; }
        [DataMember] public DateTime? DeleteAt { get; set; }
        [DataMember] public DateTime? RequestDate { get; set; }
        [DataMember] public int? ResultCode { get; set; }
        [DataMember] public DateTime? TransferOriginalDueAt { get; set; }
        [DataMember] public string ScheduledPaymentKey { get; set; }
        [DataMember] public decimal? Quantity { get; set; }
        [DataMember] public decimal? UnitPrice { get; set; }
        [DataMember] public string UnitSymbol { get; set; }
        [DataMember] public decimal? Price { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        [DataMember] public long? BillingInputId { get; set; }

        //Other table fields
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CustomerKana { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string Memo { get; set; }
        [DataMember] public string BillingCategoryCode { get; set; }
        [DataMember] public string BillingCategoryName { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
        [DataMember] public string LoginUserName { get; set; }
        [DataMember] public string ContractNumber { get; set; }
        [DataMember] public int Confirm { get; set; }
        [DataMember] public decimal? DiscountAmount { get; set; }
        [DataMember] public decimal DiscountAmount1 { get; set; }
        [DataMember] public decimal DiscountAmount2 { get; set; }
        [DataMember] public decimal DiscountAmount3 { get; set; }
        [DataMember] public decimal DiscountAmount4 { get; set; }
        [DataMember] public decimal DiscountAmount5 { get; set; }
        [DataMember] public long BillingId { get; set; }
        [DataMember] public string ParentCustomerCode { get; set; }
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public string AccountTitleCode { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public decimal PaymentAmount { get; set; }
        [DataMember] public string CategoryCodeAndName { get; set; }
        [DataMember] public string OrgCategoryCodeAndName { get; set; }
        [DataMember] public string BillingCategoryCodeAndName { get; set; }
        [DataMember] public string CollectCategoryCodeAndName { get; set; }
        [DataMember] public string InputTypeName { get; set; }
        [DataMember] public string AssignmentFlagName { get; set; }
        [DataMember] public string InputTypeNameAndIndex { get; set; }
        [DataMember] public string BillCheck { get; set; }

        //ReceiptExport
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public long ReceiptId { get; set; }
        [DataMember] public int ReceiptCompanyId { get; set; }
        [DataMember] public int ReceiptCurrencyId { get; set; }
        [DataMember] public string PrivateBankCode { get; set; }
        [DataMember] public long? ReceiptHeaderId { get; set; }
        [DataMember] public int ReceiptCategoryId { get; set; }
        [DataMember] public int? ReceiptCustomerId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int ReceiptInputType { get; set; }
        [DataMember] public int Apportioned { get; set; }
        [DataMember] public int ReceiptApproved { get; set; }
        [DataMember] public DateTime Workday { get; set; }
        [DataMember] public DateTime? RecordedAt { get; set; }
        [DataMember] public DateTime? OriginalRecordedAt { get; set; }
        [DataMember] public decimal? ReceiptAmount { get; set; }
        [DataMember] public decimal ReceiptAssignmentAmount { get; set; }
        [DataMember] public decimal? ReceiptRemainAmount { get; set; }
        [DataMember] public int ReceiptAssignmentFlag { get; set; }
        [DataMember] public string PayerCode { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string PayerNameRaw { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public DateTime? ReceiptOutputAt { get; set; }
        [DataMember] public DateTime? ReceiptDueAt { get; set; }
        [DataMember] public DateTime? MailedAt { get; set; }
        [DataMember] public long? OriginalReceiptId { get; set; }
        [DataMember] public int ExcludeFlag { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public decimal ExcludeAmount { get; set; }
        [DataMember] public string ReferenceNumber { get; set; }
        [DataMember] public string RecordNumber { get; set; }
        [DataMember] public DateTime? DensaiRegisterAt { get; set; }
        [DataMember] public string ReceiptNote1 { get; set; }
        [DataMember] public string ReceiptNote2 { get; set; }
        [DataMember] public string ReceiptNote3 { get; set; }
        [DataMember] public string ReceiptNote4 { get; set; }
        [DataMember] public string BillNumber { get; set; }
        [DataMember] public string BillBankCode { get; set; }
        [DataMember] public string BillBranchCode { get; set; }
        [DataMember] public DateTime? BillDrawAt { get; set; }
        [DataMember] public string BillDrawer { get; set; }
        [DataMember] public DateTime? ReceiptDeleteAt { get; set; }
        [DataMember] public int ReceiptCreateBy { get; set; }
        [DataMember] public DateTime ReceiptCreateAt { get; set; }
        [DataMember] public int ReceiptUpdateBy { get; set; }
        [DataMember] public DateTime ReceiptUpdateAt { get; set; }

        //other table fields
        [DataMember] public string CategoryName { get; set; }
        [DataMember] public string ReceiptCustomerCode { get; set; }
        [DataMember] public string ReceiptCustomerName { get; set; }
        [DataMember] public string ExcludeCategoryName { get; set; }
        [DataMember] public string ReceiptCurrencyCode { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public string PayerCodePrefix { get; set; }
        [DataMember] public string PayerCodeSuffix { get; set; }
        [DataMember] public int UseAdvanceReceived { get; set; }
        [DataMember] public int UseForeignCurrency { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string ReceiptMemo { get; set; }
        [DataMember] public string SourceBank { get; set; }
        [DataMember] public int UseFeeTolerance { get; set; }
        [DataMember] public int CustomerFee { get; set; }
        [DataMember] public int GeneralFee { get; set; }
        [DataMember] public DateTime OriginalUpdateAt { get; set; }
        [DataMember] public decimal BankTransferFee { get; set; }
        [DataMember] public long? NettingId { get; set; }
        [DataMember] public int RecExcOutputAt { get; set; }
        [DataMember] public int UseCashOnDueDates { get; set; }
        [DataMember] public string NettingState { get; set; }
        [DataMember] public string AccountTypeName { get; set; }
        [DataMember] public string ReceiptAssignmentFlagName { get; set; }
        [DataMember] public string RecCheck { get; set; }
        [DataMember] public string ReceiptCategroyCodeAndName { get; set; }

        public ExportMatchingIndividual()
        {

        }

        public ExportMatchingIndividual(Billing bill, Receipt receipt)
        {
            if (bill != null)
            {
                Id = bill.Id;
                CompanyId = bill.CompanyId;
                CurrencyId = bill.CurrencyId;
                CustomerId = bill.CustomerId;
                DepartmentId = bill.DepartmentId;
                StaffId = bill.StaffId;
                BillingCategoryId = bill.BillingCategoryId;
                InputType = bill.InputType;
                BillingInputTypeId = bill.BillingInputTypeId;
                BilledAt = bill.BilledAt;
                ClosingAt = bill.ClosingAt;
                SalesAt = bill.SalesAt;
                DueAt = bill.DueAt;
                BillingAmount = bill.BillingAmount;
                TaxAmount = bill.TaxAmount;
                AssignmentAmount = bill.AssignmentAmount;
                RemainAmount = bill.RemainAmount;
                TargetAmount = bill.TargetAmount;
                CustomerCode = bill.CustomerCode ;
                OffsetAmount = bill.OffsetAmount;
                AssignmentFlag = bill.AssignmentFlag;
                Approved = bill.Approved;
                CollectCategoryId = bill.CollectCategoryId;
                OriginalCollectCategoryId = bill.OriginalCollectCategoryId;
                DebitAccountTitleId = bill.DebitAccountTitleId;
                CreditAccountTitleId = bill.CreditAccountTitleId;
                OriginalDueAt = bill.OriginalDueAt;
                OutputAt = bill.OutputAt;
                PublishAt = bill.PublishAt;
                InvoiceCode = bill.InvoiceCode;
                TaxClassId = bill.TaxClassId;
                Note1 = bill.Note1;
                Note2 = bill.Note2;
                Note3 = bill.Note3;
                Note4 = bill.Note4;
                Note5 = bill.Note5;
                Note6 = bill.Note6;
                Note7 = bill.Note7;
                Note8 = bill.Note8;
                DeleteAt = bill.DeleteAt;
                RequestDate = bill.RequestDate;
                ResultCode = bill.ResultCode;
                TransferOriginalDueAt = bill.TransferOriginalDueAt;
                ScheduledPaymentKey = bill.ScheduledPaymentKey;
                Quantity = bill.Quantity;
                UnitPrice = bill.UnitPrice;
                UnitSymbol = bill.UnitSymbol;
                Price = bill.Price;
                CreateBy = bill.CreateBy;
                CreateAt = bill.CreateAt;
                UpdateBy = bill.UpdateBy;
                UpdateAt = bill.UpdateAt;
                CurrencyCode = bill.CurrencyCode;
                CustomerName = bill.CustomerName;
                CustomerKana = bill.CustomerKana;
                DepartmentCode = bill.DepartmentCode;
                DepartmentName = bill.DepartmentName;
                StaffName = bill.StaffName;
                StaffCode = bill.StaffCode;
                Memo = bill.Memo;
                BillingCategoryCode = bill.BillingCategoryCode;
                BillingCategoryName = bill.BillingCategoryName;
                CollectCategoryCode = bill.CollectCategoryCode;
                CollectCategoryName = bill.CollectCategoryName;
                LoginUserCode = bill.LoginUserCode;
                LoginUserName = bill.LoginUserName;
                ContractNumber = bill.ContractNumber;
                Confirm = bill.Confirm;
                DiscountAmount = bill.DiscountAmount;
                DiscountAmount1 = bill.DiscountAmount1;
                DiscountAmount2 = bill.DiscountAmount2;
                DiscountAmount3 = bill.DiscountAmount3;
                DiscountAmount4 = bill.DiscountAmount4;
                DiscountAmount5 = bill.DiscountAmount5;
                BillingId = bill.BillingId;
                ParentCustomerCode = bill.ParentCustomerCode;
                CompanyCode = bill.CompanyCode;
                AccountTitleCode = bill.AccountTitleCode;
                PaymentAmount = bill.PaymentAmount;
                CategoryCodeAndName = bill.CategoryCodeAndName;
                OrgCategoryCodeAndName = bill.OrgCategoryCodeAndName;
                BillingCategoryCodeAndName = bill.BillingCategoryCodeAndName;
                CollectCategoryCodeAndName = bill.CollectCategoryCodeAndName;
                InputTypeName = bill.InputTypeName;
                AssignmentFlagName = bill.AssignmentFlagName;
                InputTypeNameAndIndex = bill.InputTypeNameAndIndex;
            }

            //forReceipt
            if (receipt != null)
            {
                Amount = receipt.Amount;
                ReceiptId = receipt.Id;
                ReceiptCompanyId = receipt.CompanyId;
                ReceiptCurrencyId = receipt.CurrencyId;
                PrivateBankCode = receipt.PrivateBankCode;
                ReceiptHeaderId = receipt.ReceiptHeaderId;
                ReceiptCategoryId = receipt.ReceiptCategoryId;
                ReceiptCustomerId = receipt.CustomerId;
                SectionId = receipt.SectionId;
                ReceiptInputType = receipt.InputType;
                Apportioned = receipt.Apportioned;
                ReceiptApproved = receipt.Approved;
                Workday = receipt.Workday;
                RecordedAt = receipt.RecordedAt;
                OriginalRecordedAt = receipt.OriginalRecordedAt;
                ReceiptAmount = receipt.ReceiptAmount;
                ReceiptAssignmentAmount = receipt.AssignmentAmount;
                ReceiptRemainAmount = receipt.RemainAmount;
                ReceiptAssignmentFlag = receipt.AssignmentFlag;
                PayerCode = receipt.PayerCode;
                PayerName = receipt.PayerName;
                PayerNameRaw = receipt.PayerNameRaw;
                SourceBankName = receipt.SourceBankName;
                SourceBranchName = receipt.SourceBranchName;
                ReceiptOutputAt = receipt.OutputAt;
                ReceiptDueAt = receipt.DueAt;
                MailedAt = receipt.MailedAt;
                OriginalReceiptId = receipt.OriginalReceiptId;
                ExcludeFlag = receipt.ExcludeFlag;
                ExcludeCategoryId = receipt.ExcludeCategoryId;
                ExcludeAmount = receipt.ExcludeAmount;
                ReferenceNumber = receipt.ReferenceNumber;
                RecordNumber = receipt.RecordNumber;
                DensaiRegisterAt = receipt.DensaiRegisterAt;
                ReceiptNote1 = receipt.Note1;
                ReceiptNote2 = receipt.Note2;
                ReceiptNote3 = receipt.Note3;
                ReceiptNote4 = receipt.Note4;
                BillNumber = receipt.BillNumber;
                BillBankCode = receipt.BillBankCode;
                BillBranchCode = receipt.BillBranchCode;
                BillDrawAt = receipt.BillDrawAt;
                BillDrawer = receipt.BillDrawer;
                ReceiptDeleteAt = receipt.DeleteAt;
                ReceiptCreateBy = receipt.CreateBy;
                ReceiptCreateAt = receipt.CreateAt;
                ReceiptUpdateBy = receipt.UpdateBy;
                ReceiptUpdateAt = receipt.UpdateAt;
                CategoryName = receipt.CategoryName;
                ReceiptCustomerCode = receipt.CustomerCode;
                ReceiptCustomerName = receipt.CustomerName;
                ReceiptCurrencyCode = receipt.CurrencyCode;
                SectionCode = receipt.SectionCode;
                SectionName = receipt.SectionName;
                BankCode = receipt.BankCode;
                BankName = receipt.BankName;
                BranchCode = receipt.BranchCode;
                BranchName = receipt.BranchName;
                PayerCodePrefix = receipt.PayerCodePrefix;
                PayerCodeSuffix = receipt.PayerCodeSuffix;
                UseAdvanceReceived = receipt.UseAdvanceReceived;
                UseForeignCurrency = receipt.UseForeignCurrency;
                AccountNumber = receipt.AccountNumber;
                ReceiptMemo = receipt.ReceiptMemo;
                SourceBank = receipt.SourceBank;
                UseFeeTolerance = receipt.UseFeeTolerance;
                CustomerFee = receipt.CustomerFee;
                GeneralFee = receipt.GeneralFee;
                OriginalUpdateAt = receipt.OriginalUpdateAt;
                BankTransferFee = receipt.BankTransferFee;
                NettingId = receipt.NettingId;
                RecExcOutputAt = receipt.RecExcOutputAt;
                UseCashOnDueDates = receipt.UseCashOnDueDates;
                NettingState = receipt.NettingState;
                AccountTypeName = receipt.AccountTypeName;
                ReceiptAssignmentFlagName = receipt.AssignmentFlagName;
                ReceiptCategroyCodeAndName =receipt.CategoryCode + ":" + receipt.CategoryName;
            }
        }
    }
}
