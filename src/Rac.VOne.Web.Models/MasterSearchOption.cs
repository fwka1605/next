using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MasterSearchOption
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string[] Codes { get; set; }
        [DataMember] public int Id { get; set; }
        [DataMember] public int[] Ids { get; set; }
    }
}
