using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReceiptHeader : ITransactionData, ITransactional
    {
        [DataMember] public long Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        /// <summary>ファイルタイプ 0 : EBデータ default 0</summary>
        [DataMember] public int FileType { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int ImportFileLogId { get; set; }
        [DataMember] public DateTime Workday { get; set; }
        [DataMember] public string BankCode { get; set; } = string.Empty;
        [DataMember] public string BankName { get; set; } = string.Empty;
        [DataMember] public string BranchCode { get; set; } = string.Empty;
        [DataMember] public string BranchName { get; set; } = string.Empty;
        [DataMember] public int AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; } = string.Empty;
        [DataMember] public string AccountName { get; set; } = string.Empty;
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public int ImportCount { get; set; }
        [DataMember] public decimal ImportAmount { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        [DataMember] public string AccountTypeName { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public int ExistApportioned { get; set; }
        [DataMember] public int IsAllApportioned { get; set; }

        [DataMember] public List<Receipt> Receipts { get; set; } = new List<Receipt>();
        [DataMember] public List<ReceiptExclude> ReceiptExcludes { get; set; } = new List<ReceiptExclude>();
    }

    [DataContract]
    public class ReceiptHeaderResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ReceiptHeader[] ReceiptHeader { get; set; }
    }

    [DataContract]
    public class ReceiptHeadersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ReceiptHeader> ReceiptHeaders { get; set; }
    }
}
