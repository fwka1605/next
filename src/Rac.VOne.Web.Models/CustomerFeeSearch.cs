using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerFeeSearch
    {
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int? CustomerId { get; set; }

        [DataMember] public int? CurrencyId { get; set; }

        [DataMember] public decimal? Fee { get; set; }

        [DataMember] public bool ForPrint { get; set; }
    }
}
