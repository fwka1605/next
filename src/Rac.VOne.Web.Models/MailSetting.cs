using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MailSetting
    {

    }

    [DataContract]
    public class MailSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MailSetting MailSetting { get; set; }
    }

    [DataContract]
    public class MailSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MailSetting> MailSettings { get; set; }
    }
}
