using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class HatarakuDBData
    {
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public decimal AssignmentAmount { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        public string VOneTransferedFlag
            => AssignmentAmount != 0M
            && AssignmentAmount == BillingAmount ? "済" : "";

    }

    [DataContract]
    public class HatarakuDBDataResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<HatarakuDBData> HatarakuDBData { get; set; }
    }
}
