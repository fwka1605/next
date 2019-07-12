using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class ReceiptApportionForReportSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public ReceiptHeader Header { get; set; }
        [DataMember] public List<ReceiptApportion> Apportions { get; set; }
        [DataMember] public string CategoryName { get; set; }
    }
}
