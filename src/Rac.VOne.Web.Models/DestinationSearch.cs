using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class DestinationSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public string[] Codes { get; set; }
        /// <summary>like 用</summary>
        [DataMember] public string Name { get; set; }
    }
}
