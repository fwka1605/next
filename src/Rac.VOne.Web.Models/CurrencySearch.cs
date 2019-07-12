using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CurrencySearch
    {
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public bool UseCommonSearch { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int[] Ids { get; set; }
        [DataMember] public string[] Codes { get; set; }
    }
}
