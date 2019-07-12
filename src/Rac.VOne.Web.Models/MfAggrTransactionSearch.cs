using System;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MfAggrTransactionSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int? CurrencyId { get; set; }
        [DataMember] public DateTime? RecordedAtFrom { get; set; }
        [DataMember] public DateTime? RecordedAtTo { get; set; }
        [DataMember] public string AccountName { get; set; }
        [DataMember] public string SubAccountName { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public int? AccountTypeId { get; set; }
        [DataMember] public string AccountTypeName { get; set; }
        [DataMember] public string AccountNumber { get; set; }
    }
}
