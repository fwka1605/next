using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class SectionWithLoginUserSearch
    {
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int? LoginUserId { get; set; }
        [DataMember] public string[] SectionCodes { get; set; } = new string[] { };
        [DataMember] public string SectionName { get; set; }
    }
}
