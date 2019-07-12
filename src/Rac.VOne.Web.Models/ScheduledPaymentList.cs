using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ScheduledPaymentList
    {
        [DataMember] public DateTime? BaseDate { get; set; }
        [DataMember] public int Id { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CurrencyCode { get; set; }

        [DataMember] public DateTime? BilledAt { get; set; }
        [DataMember] public DateTime? SalesAt { get; set; }
        [DataMember] public DateTime? ClosingAt { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public DateTime? OriginalDueAt { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string DelayDivision { get; set; }
        [DataMember] public decimal TotalAmount { get; set; }

        public string CodeAndName => $"{CollectCategoryCode}：{CollectCategoryName}";


    }

    [DataContract]
    public class ScheduledPaymentListResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember]public ScheduledPaymentList ScheduledPaymentList { get; set; }
    }

    [DataContract]
    public class ScheduledPaymentListsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ScheduledPaymentList> ScheduledPaymentLists { get; set; }
    }
}
