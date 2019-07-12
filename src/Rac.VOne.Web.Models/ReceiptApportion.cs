using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReceiptApportion : ITransactionData
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public long ReceiptHeaderId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int ExcludeFlag { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public decimal? ExcludeAmount { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string PayerNameRaw { get; set; }
        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public DateTime RecordedAt { get; set; }
        [DataMember] public DateTime Workday { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public int Apportioned { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string ExcludeVirtualBranchCode { get; set; }
        [DataMember] public string ExcludeAccountNumber { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string SectionName { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public int? RefCustomerId { get; set; }
        [DataMember] public string RefCustomerCode { get; set; }
        [DataMember] public string RefCustomerName { get; set; }
        [DataMember] public int LearnIgnoreKana { get; set; }
        [DataMember] public int DoDelete { get; set; }
        [DataMember] public List<ReceiptExclude> ReceiptExcludes { get; set; } = new List<ReceiptExclude>();
        [DataMember] public int LearnKanaHistory { get; set; }
    }

    [DataContract]
    public class ReceiptApportionResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReceiptApportion[] ReceiptApportion { get; set; }
    }

    [DataContract]
    public class ReceiptApportionsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReceiptApportion> ReceiptApportion { get; set; }
        [DataMember] public string ExceptionMessage { get; set; }
    }
}
