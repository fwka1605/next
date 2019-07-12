using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class StatusSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int StatusType { get; set; }
        [DataMember] public string[] Codes { get; set; }
    }
}
