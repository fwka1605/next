using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class KanaHistorySearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string CodeFrom { get; set; }
        [DataMember] public string CodeTo { get; set; }
    }
}
