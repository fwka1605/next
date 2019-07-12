using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderTemplateSetting : IMaster
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; } = string.Empty;
        [DataMember] public string Name { get; set; } = string.Empty;
        [DataMember] public string Title { get; set; } = string.Empty;
        [DataMember] public string Greeting { get; set; } = string.Empty;
        [DataMember] public string MainText { get; set; } = string.Empty;
        [DataMember] public string SubText { get; set; } = string.Empty;
        [DataMember] public string Conclusion { get; set; } = string.Empty;
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class ReminderTemplateSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReminderTemplateSetting ReminderTemplateSetting { get; set; }
    }

    [DataContract]
    public class ReminderTemplateSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReminderTemplateSetting> ReminderTemplateSettings { get; set; }
    }
}
