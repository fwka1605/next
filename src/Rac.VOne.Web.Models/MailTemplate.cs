using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MailTemplate
    {

    }

    [DataContract]
    public class MailTemplateResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MailTemplate MailTemplate { get; set; }
    }

    [DataContract]
    public class MailTemplatesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MailTemplate> MailTemplates { get; set; }
    }
}


