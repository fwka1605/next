using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MatchingSimulationSource
    {
        [DataMember] public Billing[] Billings { get; set; }
        [DataMember] public decimal TargetAmount { get; set; }
    }
}
