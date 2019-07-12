using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ScheduledPaymentImport
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public int BillingCategoryId { get; set; }
        [DataMember] public int InputType { get; set; }
        [DataMember] public int BillingInputTypeId { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime SalesAt { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public decimal AssignmentAmount { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public decimal TargetAmount { get; set; }
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
        [DataMember] public string ScheduledPaymentKey { get; set; } = string.Empty;

        //Other table fields
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CustomerKana { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string BillingCategoryCode { get; set; }
        [DataMember] public string BillingCategoryName { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
        [DataMember] public int BillingId { get; set; }
        [DataMember] public int ParentCustomerId { get; set; }
        [DataMember] public string ParentCustomerCode { get; set; }
        [DataMember] public bool IsParent { get; set; }
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public string AccountTitleCode { get; set; }
        [DataMember] public int UpdateFlg { get; set; }
        [DataMember] public int CustomerFlg { get; set; }
        [DataMember] public int LineNo { get; set; }
    }

    [DataContract]
    public class ScheduledPaymentImportResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ScheduledPaymentImport[] ScheduledPaymentImport { get; set; }
    }

    [DataContract]
    public class ScheduledPaymentImportsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ScheduledPaymentImport> ScheduledPaymentImports { get; set; }
    }
}