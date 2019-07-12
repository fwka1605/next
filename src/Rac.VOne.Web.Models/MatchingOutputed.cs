using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]

    public class MatchingOutputed
    {
        [DataMember]public long MatchingHeaderId { get; set; }
        [DataMember]public DateTime OutputAt { get; set; }
    }

    [DataContract]
    public class MatchingOutputedResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public MatchingOutputed[] MatchingOutputed { get; set; }
    }

    [DataContract]
    public class MatchingOutputedsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchingOutputed> MatchingOutputeds { get; set; }
    }
}