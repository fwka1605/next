using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 個別消込チェックリスト印刷用モデル 画面での操作を反映して連携
    /// </summary>
    [DataContract] public class MatchingIndividualReportSource
    {
        [DataMember] public int CompanyId { get; set; }
        /// <summary>入金データを優先 標準：請求/入金 有効化した場合 入金/請求 の順で表示</summary>
        [DataMember] public bool PriorReceipt { get; set; }
        [DataMember] public int Precision { get; set; }
        [DataMember] public decimal BillingTaxDiff { get; set; }
        [DataMember] public decimal ReceiptTaxDiff { get; set; }
        [DataMember] public decimal BankFee { get; set; }
        [DataMember] public decimal DiscountAmount { get; set; }
        [DataMember] public List<ExportMatchingIndividual> Items { get; set; }
        [DataMember] public List<GridSetting> BillingGridSettings { get; set; }
        [DataMember] public List<GridSetting> ReceiptGridSettings { get; set; }
    }
}
