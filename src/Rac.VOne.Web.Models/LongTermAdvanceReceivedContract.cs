using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class LongTermAdvanceReceivedContract
    {
    }

    [DataContract]
    public class LongTermAdvanceReceivedContractResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public LongTermAdvanceReceivedContract LongTermAdvanceReceivedContract { get; set; }
    }

    [DataContract]
    public class LongTermAdvanceReceivedContractsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<LongTermAdvanceReceivedContract> LongTermAdvanceReceivedContracts { get; set; }
    }
}
