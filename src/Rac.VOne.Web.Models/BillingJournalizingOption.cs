using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class BillingJournalizingOption
    {
        [DataMember] public int CompanyId { get; set; }
        /// <summary>通貨ID 円貨のみの場合は 未指定でOK</summary>
        [DataMember] public int? CurrencyId { get; set; }
        [DataMember] public int? LoginUserId { get; set; }
        [DataMember] public bool IsOutuptted { get; set; }
        [DataMember] public DateTime? BilledAtFrom { get; set; }
        [DataMember] public DateTime? BilledAtTo { get; set; }
        [DataMember] public DateTime[] OutputAt { get; set; }
        [DataMember] public int Precision { get; set; }
    }
}
