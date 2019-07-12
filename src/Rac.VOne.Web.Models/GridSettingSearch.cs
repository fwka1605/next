using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class GridSettingSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int? GridId { get; set; }
        [DataMember] public bool IsDefault { get; set; }
    }
}
