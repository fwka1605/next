using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ScheduledPaymentListSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime BaseDate { get; set; }
        [DataMember] public DateTime? BilledAtFrom { get; set; }
        [DataMember] public DateTime? BilledAtTo { get; set; }
        [DataMember] public DateTime? DueAtFrom { get; set; }
        [DataMember] public DateTime? DueAtTo { get; set; }
        [DataMember] public DateTime? ClosingAtFrom { get; set; }
        [DataMember] public DateTime? ClosingAtTo { get; set; }
        [DataMember] public string InvoiceCodeFrom { get; set; }
        [DataMember] public string InvoiceCodeTo { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string CategoryCode { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember] public bool CustomerSummaryFlag { get; set; }
        [DataMember] public int Precision { get; set; }
        [DataMember] public List<ReportSetting> ReportSettings { get; set; } = new List<ReportSetting>();
    }
}
