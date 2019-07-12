using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BankAccount : IMasterData, IIdentical, IByCompany, ISynchronization
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public int AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public int? ReceiptCategoryId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int ImportSkipping { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        // Category.Id = BankAccount.ReceiptCategoryId
        [DataMember] public string ReceiptCategoryCode { get; set; }
        [DataMember] public string ReceiptCategoryName { get; set; }

        // AccountType.Id = BankAccount.AccountTypeId
        [DataMember] public string AccountTypeName { get; set; }

        // Section.Id = BankAccount.SectionId
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
    }

    [DataContract]
    public class BankAccountResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember]public BankAccount BankAccount { get; set; }
    }

    [DataContract]
    public class BankAccountsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BankAccount> BankAccounts { get; set; }
    }
}
