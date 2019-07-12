using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReportSetting
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string ReportId { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public int IsText { get; set; }
        [DataMember] public string Caption { get; set; }
        [DataMember] public string ItemId { get; set; }
        [DataMember] public string ItemKey { get; set; }
        [DataMember] public string ItemValue { get; set; }

        // コンボボックス
        [DataMember] public List<Setting> SettingList { get; set; }
    }

    [DataContract]
    public class ReportSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReportSetting ReportSetting { get; set; }
    }

    [DataContract]
    public class ReportSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReportSetting> ReportSettings { get; set; }
    }
}
