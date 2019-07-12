using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class GeneralSetting : IMaster
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Value { get; set; }
        [DataMember] public int Length { get; set; }
        [DataMember] public int Precision { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

    }

    [DataContract]
    public class GeneralSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public GeneralSetting GeneralSetting { get; set; }
    }

    [DataContract]
    public class GeneralSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<GeneralSetting> GeneralSettings { get; set; }
    }
}
