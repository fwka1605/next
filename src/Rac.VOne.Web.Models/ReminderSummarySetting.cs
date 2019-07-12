using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderSummarySetting : IIdentical, IByCompany
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string ColumnName { get; set; } = string.Empty;
        [DataMember] public string ColumnNameJp { get; set; } = string.Empty;
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public int Available { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class ReminderSummarySettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReminderSummarySetting ReminderSummarySetting { get; set; }
    }

    [DataContract]
    public class ReminderSummarySettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderSummarySetting> ReminderSummarySettings { get; set; }
    }
}
