using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PaymentAgencyFeeSearch
    {
        /// <summary>会社ID</summary>
        [DataMember] public int? CompanyId { get; set; }
        /// <summary>決済代行会社ID</summary>
        [DataMember] public int? PaymentAgencyId { get; set; }
        /// <summary>通貨ID</summary>
        [DataMember] public int? CurrencyId { get; set; }
    }
}
