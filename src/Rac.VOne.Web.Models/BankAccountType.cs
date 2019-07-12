using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BankAccountType : IIdentical
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int UseReceipt { get; set; }
        [DataMember] public int UseTransfer { get; set; }
    }

    [DataContract]
    public class BankAccountTypesResult : IProcessResult
    {
        [DataMember] public List<BankAccountType> BankAccountTypes { get; set; }
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }
}
