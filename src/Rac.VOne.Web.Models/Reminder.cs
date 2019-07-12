using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Reminder : IIdentical, IByCompany
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int StatusId { get; set; }
        [DataMember] public string StatusCode { get; set; } = string.Empty;
        [DataMember] public string StatusName { get; set; } = string.Empty;
        public string StatusCodeAndName
            => string.IsNullOrEmpty(StatusCode) ? "" : (StatusCode + ":" + StatusName);

        [DataMember] public string Memo { get; set; } = string.Empty;
        [DataMember] public DateTime? OutputAt { get; set; }
        [DataMember] public int? ArrearsDays { get; set; }
        [DataMember] public DateTime CalculateBaseDate { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; } = string.Empty;
        [DataMember] public string CollectCategoryName { get; set; } = string.Empty;
        public string CollectCategoryCodeAndName
            => CollectCategoryCode + ":" + CollectCategoryName;

        [DataMember] public int? DestinationId { get; set; }
        [DataMember] public string DestinationCode { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string CustomerStaffName { get; set; }
        [DataMember] public string CustomerNote { get; set; }
        [DataMember] public string CustomerTel { get; set; }
        [DataMember] public int DetailCount { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public decimal? ReminderAmount { get; set; }
        [DataMember] public decimal? ArrearsInterest { get; set; }
        [DataMember] public int ExcludeReminderPublish { get; set; }
        [DataMember] public int[] Ids { get; set; }
        [DataMember] public int[] DestinationIds { get; set; }
        [DataMember] public int? DestinationIdInput { get; set; }
        [DataMember] public bool NoDestination { get; set; } = false;
        [DataMember] public string DestinationDisplay { get; set; } = string.Empty;
        public bool Checked { get; set; }
        public int? TemplateId { get; set; }
        public DateTime BaseDate { get; set; }

    }

    public class ReminderResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Reminder> Reminder { get; set; }
    }
}
