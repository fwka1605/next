using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Company : IMasterData, ISynchronization, IIdentical
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Kana { get; set; }
        [DataMember] public string PostalCode { get; set; }
        [DataMember] public string Address1 { get; set; }
        [DataMember] public string Address2 { get; set; }
        [DataMember] public string Tel { get; set; }
        [DataMember] public string Fax { get; set; }
        [DataMember] public string ProductKey { get; set; }
        [DataMember] public string BankAccountName { get; set; }
        [DataMember] public string BankAccountKana { get; set; }
        [DataMember] public string BankName1 { get; set; }
        [DataMember] public string BranchName1 { get; set; }
        [DataMember] public string AccountType1 { get; set; }
        [DataMember] public string AccountNumber1 { get; set; }
        [DataMember] public string BankName2 { get; set; }
        [DataMember] public string BranchName2 { get; set; }
        [DataMember] public string AccountType2 { get; set; }
        [DataMember] public string AccountNumber2 { get; set; }
        [DataMember] public string BankName3 { get; set; }
        [DataMember] public string BranchName3 { get; set; }
        [DataMember] public string AccountType3 { get; set; }
        [DataMember] public string AccountNumber3 { get; set; }
        [DataMember] public int ShowConfirmDialog { get; set; }
        [DataMember] public int PresetCodeSearchDialog { get; set; }
        [DataMember] public int ShowWarningDialog { get; set; }
        [DataMember] public int ClosingDay { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public int TransferAggregate { get; set; }
        [DataMember] public int AutoCloseProgressDialog { get; set; }
    }

    [DataContract]
    public class CompanyResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Company Company { get; set; }
    }

    [DataContract]
    public class CompaniesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Company> Companies { get; set; }
    }

    [DataContract]
    public class CompanySource
    {
        [DataMember] public Company Company;
        [DataMember] public List<CompanyLogo> SaveCompanyLogos;
        [DataMember] public List<CompanyLogo> DeleteCompanyLogos;
        [DataMember] public ApplicationControl ApplicationControl;
        [DataMember] public MenuAuthority[] MenuAuthorities;
        [DataMember] public FunctionAuthority[] FunctionAuthorities;
        [DataMember] public PasswordPolicy PasswordPolicy;
        [DataMember] public LoginUserLicense LoginUserLicense;
    }
}
