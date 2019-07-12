using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MfAggrTag : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public string Name { get; set; }
    }

    [DataContract] public class MfAggrTagsResult : IProcessResult
    {
        [DataMember] public List<MfAggrTag> Tags { get; set; }
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }

    [DataContract] public class MfAggrTagRel
    {
        [DataMember] public long SubAccountId { get; set; }
        [DataMember] public long TagId { get; set; }
    }

}
