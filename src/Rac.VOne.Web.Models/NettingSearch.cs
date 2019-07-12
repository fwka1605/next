using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class NettingSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public string NettingRecordedAt { get; set; }
        [DataMember] public string NettingAmount { get; set; }
        [DataMember] public string CategoryCode { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerKana { get; set; }
        [DataMember] public string CustomerName { get; set; }
    }
}
