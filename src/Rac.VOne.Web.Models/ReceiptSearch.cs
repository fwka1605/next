using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ReceiptSearch
    {
        [DataMember] public int UseForeignCurrencyFlg { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        /// <summary>対象外フラグ
        /// 0 : 通常入金
        /// 1 : 対象外入金
        /// 2 : すべて
        /// </summary>
        /// <remarks>
        /// 0 : 通常入金
        /// → 対象外なし または 全額対象外以外
        /// 1 : 対象外入金
        /// → 対象外に設定されている入金データ
        /// 2 : すべて
        /// → 絞込条件に対象外かどうかを付けない
        /// </remarks>
        [DataMember] public int ExcludeFlag { get; set; }

        /// <summary>消込フラグ 検索条件の選択状態
        /// 実際の<see cref="Receipt.AssignmentFlag"/>とは異なる
        /// <see cref="Rac.VOne.Common.Constants.AssignmentFlagChecked"/>を参照
        /// 0b000 何も選択されていない
        /// 0b001 2 ^ 0 未消込   の選択有無
        /// 0b010 2 ^ 1 一部消込 の選択有無
        /// 0b100 2 ^ 2 消込済   の選択有無
        /// なにも選択なし            0x00 0b000
        ///                   未消込  0x01 0b001
        ///         一部消込          0x02 0b010
        ///         一部消込  未消込  0x03 0b011
        /// 消込済                    0x04 0b100
        /// 消込済            未消込  0x05 0b101
        /// 消込済  一部消込          0x06 0b110
        /// すべて選択                0x07 0b111
        /// </summary>
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public decimal? ReceiptAmountFrom { get; set; }
        [DataMember] public decimal? ReceiptAmountTo { get; set; }
        [DataMember] public decimal? RemainAmountFrom { get; set; }
        [DataMember] public decimal? RemainAmountTo { get; set; }
        [DataMember] public int? InputType { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public DateTime? UpdateAtFrom { get; set; }
        [DataMember] public DateTime? UpdateAtTo { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public string PrivateBankCode { get; set; }
        [DataMember] public string PayerCodePrefix { get; set; }
        [DataMember] public string PayerCodeSuffix { get; set; }
        [DataMember] public string BillNumber { get; set; }
        [DataMember] public string BillBankCode { get; set; }
        [DataMember] public string BillBranchCode { get; set; }
        [DataMember] public DateTime? BillDrawAtFrom { get; set; }
        [DataMember] public DateTime? BillDrawAtTo { get; set; }
        [DataMember] public string BillDrawer { get; set; }
        [DataMember] public DateTime? RecordedAtFrom { get; set; }
        [DataMember] public DateTime? RecordedAtTo { get; set; }
        [DataMember] public string SourceBankName { get; set; }
        [DataMember] public string SourceBranchName { get; set; }
        [DataMember] public int ReceiptCategoryId { get; set; }
        [DataMember] public string CustomerCodeFrom { get; set; }
        [DataMember] public string CustomerCodeTo { get; set; }
        [DataMember] public string SectionCodeFrom { get; set; }
        [DataMember] public string SectionCodeTo { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string ReceiptCategoryCode { get; set; }
        [DataMember] public string ReceiptCategoryCodeFrom { get; set; }
        [DataMember] public string ReceiptCategoryCodeTo { get; set; }
        [DataMember] public int? ExcludeCategoryId { get; set; }
        [DataMember] public string SectionCode { get; set; }
        [DataMember] public string BankCode { get; set; }
        [DataMember] public string BranchCode { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public int AccountTypeId { get; set; }
        [DataMember] public int ExistsMemo { get; set; }
        [DataMember] public string ReceiptMemo { get; set; }
        [DataMember] public int? UpdateBy { get; set; }
        [DataMember] public bool UseSectionMaster { get; set; }
        [DataMember] public int PageCount { get; set; }
        [DataMember] public int? DeleteFlg { get; set; }
        [DataMember] public DateTime? DeleteAtFrom { get; set; }
        [DataMember] public DateTime? DeleteAtTo { get; set; }
        [DataMember] public int? AdvanceReceivedFlg { get; set; }
        [DataMember] public string KobetsuType { get; set; }
        [DataMember] public bool UseSectionWork { get; set; }
        [DataMember] public byte[] ClientKey { get; set; }
        /// <summary>PD0501 入金データ入力で利用 編集可のデータのみ表示</summary>
        /// <remarks>
        /// 編集可：未消込、未仕訳、振替後の前受データではない
        /// </remarks>
        [DataMember] public bool IsEditable { get; set; }
    }
}
