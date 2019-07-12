using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ClosingSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public DateTime ClosingAtFrom { get; set; }
        [DataMember] public DateTime ClosingAtTo { get; set; }
    }
}
