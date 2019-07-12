using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class AdvanceReceivedSplitSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public long OriginalReceiptId { get; set; }
        [DataMember] public AdvanceReceivedSplit[] Items { get; set; }
    }
}
