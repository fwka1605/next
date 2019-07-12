using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ReminderSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int UseForeignCurrency { get; set; }
        [DataMember] public Reminder[] Items { get; set; }
        [DataMember] public ReminderSummary[] Summaries { get; set; }
        [DataMember] public int[] ReminderIds { get; set; }
        [DataMember] public int[] CustomerIds { get; set; }
        [DataMember] public ReminderCommonSetting Setting { get; set; }
        [DataMember] public ReminderSummarySetting[] SummarySettings { get; set; }
        [DataMember] public ReminderOutputed ReminderOutputed { get; set; }
        [DataMember] public ReminderOutputed[] ReminderOutputeds { get; set; }
    }
}
