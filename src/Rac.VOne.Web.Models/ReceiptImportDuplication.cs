using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    /// <summary>入金フリーインポーター 重複チェック用
    /// テーブル型変数 の column 定義順と property の定義順を合わせること
    /// reflection で datatable への値詰込みを行う際に、property の順番が一致している必要がある
    /// </summary>
    [DataContract] public class ReceiptImportDuplication
    {
        [DataMember] public int RowNumber { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public DateTime? RecordedAt { get; set; }
        [DataMember] public int? ReceiptCategoryId { get; set; }
        [DataMember] public decimal? ReceiptAmount { get; set; }
        [DataMember] public DateTime? DueAt { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int? CurrencyId { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public int? AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string AccountName { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public string BillNumber { get; set; }
        [DataMember] public string BillBankCode { get; set; }
        [DataMember] public string BillBranchCode { get; set; }
        [DataMember] public string BillDrawer { get; set; }
        [DataMember] public DateTime? BillDrawAt { get; set; }
    }

    [DataContract]
    public class ReceiptImportDuplicationResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public int[] RowNumbers { get; set; }
    }
}
