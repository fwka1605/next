using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ArrearagesListSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime BaseDate { get; set; }
        [DataMember] public bool ExistsMemo { get; set; }
        [DataMember] public string BillingMemo { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string DepartmentCodeFrom { get; set; }
        [DataMember] public string DepartmentCodeTo { get; set; }
        [DataMember] public string StaffCodeFrom { get; set; }
        [DataMember] public string StaffCodeTo { get; set; }
        [DataMember] public bool CustomerSummaryFlag { get; set; }
        [DataMember] public List<ReportSetting> ReportSettings { get; set; } = new List<ReportSetting>();
        [DataMember] public int Precision { get; set; }
    }
}
