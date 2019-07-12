using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderOutputed
    {
        [DataMember] public int OutputNo { get; set; }
        [DataMember] public long BillingId { get; set; }
        [DataMember] public int ReminderId { get; set; }
        [DataMember] public int ReminderTemplateId { get; set; }
        [DataMember] public DateTime OutputAt { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public int BillingCount { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public decimal RemainAmount { get; set; }
        [DataMember] public int? DestinationId { get; set; }
        [DataMember] public string DestinationCode { get; set; } = string.Empty;
        [DataMember] public string DestinationDisplay { get; set; } = string.Empty;
        [DataMember] public int[] OutputNos { get; set; }
        [DataMember] public int[] DestinationIds { get; set; }
        public bool Checked { get; set; }
        public string OutputNoPaddingZero
            => OutputNo.ToString("000000");

    }

    public class ReminderOutputedResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderOutputed> ReminderOutputed { get; set; }
    }
}
