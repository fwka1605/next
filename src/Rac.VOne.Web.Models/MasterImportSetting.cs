using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MasterImportSetting
    {
    }

    [DataContract]
    public class MasterImportSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MasterImportSetting MasterImportSetting { get; set; }
    }

    [DataContract]
    public class MasterImportSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MasterImportSetting> MasterImportSettings { get; set; }
    }
}
