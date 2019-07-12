using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingInvoice
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public long TemporaryBillingInputId { get; set; }
        [DataMember] public long? BillingInputId { get; set; }
        [DataMember] public int Checked { get; set; }
        [DataMember] public string InvoiceTemplateCode { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public int DetailsCount { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CustomerPostalCode { get; set; }
        [DataMember] public string CustomerAddress1 { get; set; }
        [DataMember] public string CustomerAddress2 { get; set; }
        [DataMember] public decimal AmountSum { get; set; }
        [DataMember] public decimal RemainAmountSum { get; set; }
        [DataMember] public string CollectCategoryCodeAndNeme { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public int InvoiceTemplateId { get; set; }
        [DataMember] public DateTime PublishAt { get; set; }
        [DataMember] public DateTime PublishAt1st { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string DestnationCode { get; set; }
        [DataMember] public string DestnationButton { get; set; } = "…";
        [DataMember] public int? DestinationId { get; set; }
        [DataMember] public string DestinationName { get; set; }
        [DataMember] public string DestinationPostalCode { get; set; }
        [DataMember] public string DestinationAddress1 { get; set; }
        [DataMember] public string DestinationAddress2 { get; set; }
        [DataMember] public string DestinationDepartmentName { get; set; }
        [DataMember] public string DestinationAddressee { get; set; }
        [DataMember] public string DestinationHonorific { get; set; }
        [DataMember] public string CustomerDestinationDepartmentName { get; set; } = string.Empty;
        [DataMember] public string CustomerStaffName {get; set;} = string.Empty;
        [DataMember] public string CustomerHonorific { get; set; } = string.Empty;

        public string DestnationContent
        {
            get
            {
                if (DestinationId == null || DestinationId == 0)
                {
                    return "〒"
                        + CustomerPostalCode + " "
                        + CustomerAddress1
                        + CustomerAddress2 + " "
                        + CustomerName
                        + CustomerDestinationDepartmentName
                        + CustomerStaffName
                        + CustomerHonorific;
                }

                return "〒"
                + DestinationPostalCode + " "
                + DestinationAddress1
                + DestinationAddress2 + " "
                + (string.IsNullOrEmpty(DestinationName) ? CustomerName : DestinationName)
                + DestinationDepartmentName
                + DestinationAddressee
                + DestinationHonorific;
            }
            set { }
        }
        public void SetDestination(Destination destination = null)
        {
            if (destination == null)
            {
                DestinationId = null;
                DestnationCode = string.Empty;
                DestinationName = string.Empty;
                DestinationPostalCode = string.Empty;
                DestinationAddress1 = string.Empty;
                DestinationAddress2 = string.Empty;
                DestinationDepartmentName = string.Empty;
                DestinationAddressee = string.Empty;
                DestinationHonorific = string.Empty;
            }
            else
            {
                DestinationId = destination.Id;
                DestnationCode = destination.Code;
                DestinationName = destination.Name;
                DestinationPostalCode = destination.PostalCode;
                DestinationAddress1 = destination.Address1;
                DestinationAddress2 = destination.Address2;
                DestinationDepartmentName = destination.DepartmentName;
                DestinationAddressee = destination.Addressee;
                DestinationHonorific = destination.Honorific;
            }
        }
    }

    [DataContract]
    public class BillingInvoiceResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingInvoice> BillingInvoices { get; set; }
    }

    [DataContract]
    public class BillingInvoiceForPublish
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public long TemporaryBillingInputId { get; set; }
        [DataMember] public long? BillingInputId { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public int InvoiceTemplateId { get; set; }
        [DataMember] public string InvoiceTemplateFixedString { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int? DestinationId { get; set; }
    }

    [DataContract]
    [Serializable]
    public class BillingInvoiceDetailForPrint
    {
        [DataMember] public long BillingInputId { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime? SalesAt { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public decimal TaxExcludedPrice { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public string TaxClassName { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public decimal ?Quantity { get; set; }
        [DataMember] public decimal ?UnitPrice { get; set; }
        [DataMember] public string UnitSymbol { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public int DisplayStaff { get; set; }
        [DataMember] public string Tel { get; set; }
        [DataMember] public string Fax { get; set; }
        [DataMember] public string CustomerPostalCode { get; set; }
        [DataMember] public string CustomerAddress1 { get; set; }
        [DataMember] public string CustomerAddress2 { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public int? DestinationId { get; set; }
        [DataMember] public string DestinationName { get; set; }
        [DataMember] public string DestinationPostalCode { get; set; }
        [DataMember] public string DestinationAddress1 { get; set; }
        [DataMember] public string DestinationAddress2 { get; set; }
        [DataMember] public string DestinationDepartmentName { get; set; }
        [DataMember] public string DestinationAddressee { get; set; }
        [DataMember] public string DestinationHonorific { get; set; }
        [DataMember] public string Title { get; set; }
        [DataMember] public string Greeting { get; set; }
        [DataMember] public string DueDateComment { get; set; }
        [DataMember] public int DueDateFormat { get; set; }
        [DataMember] public string TransferFeeComment { get; set; }
        [DataMember] public int ReceiveAccountId1 { get; set; }
        [DataMember] public int ReceiveAccountId2 { get; set; }
        [DataMember] public int ReceiveAccountId3 { get; set; }
        [DataMember] public string ReceiveAccount1 { get; set; } = string.Empty;
        [DataMember] public string ReceiveAccount2 { get; set; } = string.Empty;
        [DataMember] public string ReceiveAccount3 { get; set; } = string.Empty;
        [DataMember] public string BankAccountName { get; set; } = string.Empty;
        [DataMember] public string ExclusiveAccount { get; set; }
        [DataMember] public int DetailCount { get; set; }
        [DataMember] public int ShareTransferFee { get; set; }
        [DataMember] public string CustomerDestinationDepartmentName { get; set; } = string.Empty;
        [DataMember] public string CustomerStaffName { get; set; } = string.Empty;
        [DataMember] public string CustomerHonorific { get; set; } = string.Empty;
    }

    [DataContract]
    public class BillingInvoiceDetailResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingInvoiceDetailForPrint> BillingInvoicesDetails { get; set; }
    }

    [DataContract]
    public class BillingInvoiceDetailForExport
    {
        [DataMember] public string CompanyCode { get; set; } //1
        [DataMember] public long BillingInputId { get; set; }
        [DataMember] public long BillingId { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime SalesAt { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public decimal Price { get; set; } //10
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; } //15
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string Note5 { get; set; }
        [DataMember] public string Note6 { get; set; }
        [DataMember] public string Note7 { get; set; }
        [DataMember] public string Note8 { get; set; } //20
        [DataMember] public string Memo { get; set; }
        [DataMember] public string ContractNumber { get; set; }
        [DataMember] public decimal Quantity { get; set; }
        [DataMember] public string UnitSymbol { get; set; }
        [DataMember] public decimal UnitPrice { get; set; }//25
        [DataMember] public DateTime PublishAt { get; set; }
        [DataMember] public DateTime PublishAt1st { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; } //30
        [DataMember] public string BillingCategoryCode { get; set; }
        [DataMember] public string BillingCategoryName { get; set; }
        [DataMember] public string BillingCategoryExternalCode { get; set; }
        [DataMember] public int TaxClassId { get; set; }
        [DataMember] public string TaxClassName { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string CollectCategoryExternalCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string StaffCode { get; set; }//40
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public int ShareTransferFee { get; set; }
        [DataMember] public string CustomerPostalCode { get; set; }
        [DataMember] public string CustomerAddress1 { get; set; }//45
        [DataMember] public string CustomerAddress2 { get; set; }
        [DataMember] public string CustomerDepartmentName { get; set; }
        [DataMember] public string CustomerAddressee { get; set; }
        [DataMember] public string CustomerHonorific { get; set; }
        [DataMember] public string CustomerNote { get; set; } //50
        [DataMember] public string ExclusiveBankCode { get; set; }
        [DataMember] public string ExclusiveBankName { get; set; }
        [DataMember] public string ExclusiveBranchCode { get; set; }
        [DataMember] public string VirtualBranchCode { get; set; }
        [DataMember] public string ExclusiveBranchName { get; set; }//55
        [DataMember] public string VirtualAccountNumber { get; set; }
        [DataMember] public int ExclusiveAccountTypeId { get; set; }
        [DataMember] public string CompanyBankName1 { get; set; }
        [DataMember] public string CompanyBranchName1 { get; set; }
        [DataMember] public string CompanyAccountType1 { get; set; } //60
        [DataMember] public string CompanyAccountNumber1 { get; set; }
        [DataMember] public string CompanyBankName2 { get; set; }
        [DataMember] public string CompanyBranchName2 { get; set; }
        [DataMember] public string CompanyAccountType2 { get; set; }
        [DataMember] public string CompanyAccountNumber2 { get; set; }
        [DataMember] public string CompanyBankName3 { get; set; }
        [DataMember] public string CompanyBranchName3 { get; set; }
        [DataMember] public string CompanyAccountType3 { get; set; }
        [DataMember] public string CompanyAccountNumber3 { get; set; }
        [DataMember] public string CompanyBankAccountName { get; set; } //70
        [DataMember] public string CompanyName { get; set; }
        [DataMember] public string CompanyPostalCode { get; set; }
        [DataMember] public string CompanyAddress1 { get; set; }
        [DataMember] public string CompanyAddress2 { get; set; }
        [DataMember] public string CompanyTel { get; set; }
        [DataMember] public string CompanyFax { get; set; } //76
    }

    [DataContract]
    public class BillingInvoiceDetailForExportResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingInvoiceDetailForExport> BillingInvoicesDetails { get; set; }
    }

    [DataContract]
    public class BillingInvoiceDetailSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public long[] BillingInputIds { get; set; }
        [DataMember] public long[] TemporaryBillingInputIds { get; set; }
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public int InvoiceTemplateId { get; set; }

        /// <summary>プレビューの時だけ利用</summary>
        [DataMember] public int? DestinationId { get; set; }
    }

}
