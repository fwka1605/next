using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class LogDataSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime? LoggedAtFrom { get; set; }
        [DataMember] public DateTime? LoggedAtTo { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
    }
}
