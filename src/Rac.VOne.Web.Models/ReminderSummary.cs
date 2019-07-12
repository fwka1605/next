using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderSummary
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public int ReminderCount { get; set; }
        [DataMember] public int BillingCount { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public decimal ReminderAmount { get; set; }
        [DataMember] public string Memo { get; set; } = string.Empty;
        [DataMember] public int? DestinationId { get; set; }
        [DataMember] public string DestinationCode { get; set; }
        [DataMember] public string DestinationDepartmentName { get; set; }
        [DataMember] public string CustomerStaffName { get; set; }
        [DataMember] public string CustomerNote { get; set; }
        [DataMember] public string CustomerTel { get; set; }
        [DataMember] public string CustomerFax { get; set; }
        [DataMember] public int ExcludeReminderPublish { get; set; }
        public bool Checked { get; set; }
        public int TemplateId { get; set; }
        [DataMember] public int[] CustomerIds { get; set; }
        [DataMember] public int[] DestinationIds { get; set; }
        [DataMember] public int? DestinationIdInput { get; set; }
        [DataMember] public bool NoDestination { get; set; } = false;
    }

    public class ReminderSummaryResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderSummary> ReminderSummary { get; set; }
    }
}
