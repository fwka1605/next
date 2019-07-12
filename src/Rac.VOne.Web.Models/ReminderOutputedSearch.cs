using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReminderOutputedSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public DateTime? OutputAtFrom { get; set; }
        [DataMember] public DateTime? OutputAtTo { get; set; }
        [DataMember] public int? OutputNoFrom { get; set; }
        [DataMember] public int? OutputNoTo { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember]public bool UseDestinationSummarized { get; set; }
    }
}
