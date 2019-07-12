using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderLevelSetting : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int ReminderLevel { get; set; }
        [DataMember] public int ReminderTemplateId { get; set; }
        [DataMember] public int ArrearDays { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string ReminderTemplateCode { get; set; }
        [DataMember] public string ReminderTemplateName { get; set; }
    }

    [DataContract]
    public class ReminderLevelSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReminderLevelSetting ReminderLevelSetting { get; set; }
        [DataMember] public int MaxReminderLevel { get; set; }
    }

    [DataContract]
    public class ReminderLevelSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderLevelSetting> ReminderLevelSettings { get; set; }
    }

    [DataContract]
    public class MaxReminderLevelResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public int MaxReminderLevel { get; set; }
    }
}
