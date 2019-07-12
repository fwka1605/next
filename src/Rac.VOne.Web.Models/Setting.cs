using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Setting
    {
        [DataMember] public string ItemId { get; set; }
        [DataMember] public string ItemKey { get; set; }
        [DataMember] public string ItemValue { get; set; }
    }

    [DataContract]
    public class SettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Setting Setting { get; set; }
    }

    [DataContract]
    public class SettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Setting> Settings { get; set; }
    }
}
