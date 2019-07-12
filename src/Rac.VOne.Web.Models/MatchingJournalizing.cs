using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MatchingJournalizing
    {
        /// <summary>会社ID</summary>
        [DataMember] public int CompanyId { get; set; }
        /// <summary>伝票日付</summary>
        [DataMember] public DateTime RecordedAt { get; set; }
        /// <summary>伝票番号</summary>
        [DataMember] public long SlipNumber { get; set; }
        /// <summary>借方部門コード</summary>
        [DataMember] public string DebitDepartmentCode { get; set; }
        /// <summary>借方部門名</summary>
        [DataMember] public string DebitDepartmentName { get; set; }
        /// <summary>借方科目コード</summary>
        [DataMember] public string DebitAccountTitleCode { get; set; }
        /// <summary>借方科目名</summary>
        [DataMember] public string DebitAccountTitleName { get; set; }
        /// <summary>借方補助コード</summary>
        [DataMember] public string DebitSubCode { get; set; }
        /// <summary>借方補助名</summary>
        [DataMember] public string DebitSubName { get; set; }
        /// <summary>貸方部門コード</summary>
        [DataMember] public string CreditDepartmentCode { get; set; }
        /// <summary>貸方部門名</summary>
        [DataMember] public string CreditDepartmentName { get; set; }
        /// <summary>貸方科目コード</summary>
        [DataMember] public string CreditAccountTitleCode { get; set; }
        /// <summary>貸方科目名</summary>
        [DataMember] public string CreditAccountTitleName { get; set; }
        /// <summary>貸方補助コード</summary>
        [DataMember] public string CreditSubCode { get; set; }
        /// <summary>貸方補助名</summary>
        [DataMember] public string CreditSubName { get; set; }
        /// <summary>金額</summary>
        [DataMember] public decimal Amount { get; set; }
        /// <summary>備考</summary>
        [DataMember] public string Note { get; set; }
        /// <summary>得意先コード</summary>
        [DataMember] public string CustomerCode { get; set; }
        /// <summary>得意先名</summary>
        [DataMember] public string CustomerName { get; set; }
        /// <summary>請求番号</summary>
        [DataMember] public string InvoiceCode { get; set; }
        /// <summary>担当者コード</summary>
        [DataMember] public string StaffCode { get; set; }
        /// <summary>振込依頼人コード</summary>
        [DataMember] public string PayerCode { get; set; }
        /// <summary>振込依頼人名</summary>
        [DataMember] public string PayerName { get; set; }
        /// <summary>仕向銀行（カナ）</summary>
        [DataMember] public string SourceBankName { get; set; }
        /// <summary>仕向支店（カナ）</summary>
        [DataMember] public string SourceBranchName { get; set; }
        /// <summary>期日</summary>
        [DataMember] public DateTime DueDate { get; set; }
        /// <summary>銀行コード</summary>
        [DataMember] public string BankCode { get; set; }
        /// <summary>銀行名</summary>
        [DataMember] public string BankName { get; set; }
        /// <summary>支店コード</summary>
        [DataMember] public string BranchCode { get; set; }
        /// <summary>支店名</summary>
        [DataMember] public string BranchName { get; set; }
        /// <summary>預金種別</summary>
        [DataMember] public int? AccountTypeId { get; set; }
        /// <summary>口座番号</summary>
        [DataMember] public string AccountNumber { get; set; }
        /// <summary>税区分</summary>
        [DataMember] public int? TaxClassId { get; set; }
        /// <summary>作成日時</summary>
        [DataMember] public DateTime CreateAt { get; set; }
        /// <summary>通貨コード</summary>
        [DataMember] public string CurrencyCode { get; set; }
        /// <summary>承認フラグ</summary>
        [DataMember] public bool Approved { get; set; }
        /// <summary>トランザクションID ？？</summary>
        [DataMember] public long Id { get; set; }

        //PE1001 MF消込仕訳出力
        [DataMember] public string MatchingMemo { get; set; }
        /// <summary> MF仕訳出力 借方税区分 </summary>
        public string DebitTaxCategory { get; set; } = string.Empty;
        /// <summary> 借方金額(円) </summary>
        public decimal DebitAmount { get { return this.Amount; } }
        /// <summary> MF仕訳出力 借方税額 </summary>
        public int DebitTaxAmount { get; set; }
        /// <summary> MF仕訳出力 貸方税区分 </summary>
        public string CreditTaxCategory { get; set; } = string.Empty;
        /// <summary> 貸方金額(円) </summary>
        public decimal CreditAmount { get { return this.Amount; } }
        /// <summary> MF仕訳出力 貸方税額 </summary>
        public int CreditTaxAmount { get; set; }
        /// <summary> MF仕訳出力 タグ </summary>
        public string MFTag { get; set; } = string.Empty;
        /// <summary> MF仕訳出力 MF仕訳タイプ インポート固定</summary>
        public string MFJournalizingType { get { return "インポート"; } }
        /// <summary> MF仕訳出力 決算整理仕訳 </summary>
        public string ClosingAdjustment { get; set; } = string.Empty;
        /// <summary> 最終更新日時 </summary>
        public DateTime MFUpdateAt { get { return this.CreateAt; } }

    }

    [DataContract]
    public class MatchingJournalizingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<MatchingJournalizing> MatchingJournalizings { get; set; }
    }

    [DataContract]
    public class MatchingJournalizingProcessResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }
}
