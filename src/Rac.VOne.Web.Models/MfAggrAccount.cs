using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MfAggrAccount : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public string DisplayName { get; set; }
        [DataMember] public DateTime LastAggregatedAt { get; set; }
        [DataMember] public DateTime LastLoginAt { get; set; }
        [DataMember] public DateTime LastSucceededAt { get; set; }
        [DataMember] public DateTime? AggregationStartDate { get; set; }
        [DataMember] public int Status { get; set; }
        [DataMember] public int IsSuspended { get; set; }
        [DataMember] public string BankCode { get; set; }

        [DataMember] public MfAggrSubAccount[] SubAccounts { get; set; }
    }

    [DataContract] public class MfAggrAccountsResult : IProcessResult
    {
        [DataMember] public List<MfAggrAccount> Accounts { get; set; }
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }

    [DataContract] public class MfAggrSubAccount : ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public long AccountId { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string AccountTypeName { get; set; }
        [DataMember] public int? AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public int ReceiptCategoryId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public long[] TagIds { get; set; }
    }

}
