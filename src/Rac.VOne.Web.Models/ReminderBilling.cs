using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderBilling
    {
        [DataMember] public int RowNumber { get; set; }
        [DataMember] public long Id { get; set; }
        [DataMember] public int ReminderId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerPostalCode { get; set; }
        [DataMember] public string CustomerAddress1 { get; set; }
        [DataMember] public string CustomerAddress2 { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CustomerStaffName { get; set; }
        [DataMember] public int CustomerReceiveAccount1 { get; set; }
        [DataMember] public int CustomerReceiveAccount2 { get; set; }
        [DataMember] public int CustomerReceiveAccount3 { get; set; }
        [DataMember] public DateTime SalesAt { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string Note5 { get; set; }
        [DataMember] public string Note6 { get; set; }
        [DataMember] public string Note7 { get; set; }
        [DataMember] public string Note8 { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string StaffTel { get; set; }
        [DataMember] public string StaffFax { get; set; }
        [DataMember] public decimal BillingAmount { get; set;}
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public DateTime OriginalDueAt { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public int OutputNo { get; set; }
        [DataMember] public int? DestinationId { get; set; }
        [DataMember] public string DestinationName { get; set; } = string.Empty;
        [DataMember] public string DestinationPostalCode { get; set; } = string.Empty;
        [DataMember] public string DestinationAddress1 { get; set; } = string.Empty;
        [DataMember] public string DestinationAddress2 { get; set; } = string.Empty;
        [DataMember] public string DestinationDepartmentName { get; set; } = string.Empty;
        [DataMember] public string DestinationAddressee { get; set; } = string.Empty;
        [DataMember] public string DestinationHonorific { get; set; } = string.Empty;

    }

    [DataContract]
    public class ReminderBillingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderBilling> ReminderBilling { get; set; }
    }
}
