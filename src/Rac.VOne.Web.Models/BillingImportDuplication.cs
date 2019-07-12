using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingImportDuplication
    {
        [DataMember] public int RowNumber { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public DateTime? BilledAt { get; set; }
        [DataMember] public decimal? BillingAmount { get; set; }
        [DataMember] public decimal? TaxAmount { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public int DebitAccountTitleId { get; set; }
        [DataMember] public DateTime? SalesAt { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public DateTime? ClosingAt { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public int BillingCategoryId { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public decimal? Price { get; set; }
        [DataMember] public int? TaxClassId { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string Note5 { get; set; }
        [DataMember] public string Note6 { get; set; }
        [DataMember] public string Note7 { get; set; }
        [DataMember] public string Note8 { get; set; }
        [DataMember] public int CurrencyId { get; set; }
    }

    [DataContract]
    public class BillingImportDuplicationWithCode : BillingImportDuplication
    {
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DebitAccountTitleCode { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string BillingCategoryCode { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
    }

    [DataContract]
    public class BillingImportDuplicationResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public int[] RowNumbers { get; set; }
    }
}
