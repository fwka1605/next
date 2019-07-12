using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingJournalizing
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public long SlipNumber { get; set; }
        [DataMember] public string DebitDepartmentCode { get; set; }
        [DataMember] public string DebitDepartmentName { get; set; }
        [DataMember] public string DebitAccountTitleCode { get; set; }
        [DataMember] public string DebitAccountTitleName { get; set; }
        [DataMember] public string DebitSubCode { get; set; }
        [DataMember] public string DebitSubName { get; set; }
        [DataMember] public string CreditDepartmentCode { get; set; }
        [DataMember] public string CreditDepartmentName { get; set; }
        [DataMember] public string CreditAccountTitleCode { get; set; }
        [DataMember] public string CreditAccountTitleName { get; set; }
        [DataMember] public string CreditSubCode { get; set; }
        [DataMember] public string CreditSubName { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string PayerCode { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public int? AccountType { get; set; }
        [DataMember] public string AccountNumber { get; set; }
    }

    [DataContract]
    public class BillingJournalizingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingJournalizing> BillingJournalizings { get; set; }
    }

}
