using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class SectionSearch
    {
        [DataMember] public bool UseCommonSearch { get; set; }
        [DataMember] public int[] Ids { get; set; }
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public string[] Codes { get; set; }
        [DataMember] public int? LoginUserId { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string[] PayerCodes { get; set; }
        [DataMember] public int? CustomerId { get; set; }
    }
}
