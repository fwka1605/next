using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MatchingJournalizingReportSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int Precision { get; set; }
        [DataMember] public bool ReOutput { get; set; }
        [DataMember] public List<MatchingJournalizing> Items { get; set; }
    }
}
