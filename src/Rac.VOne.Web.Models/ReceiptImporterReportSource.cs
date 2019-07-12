using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 入金フリーインポーター 印刷用のモデル インポート処理によっては変更される
    /// 現状、クライアント側に、インポートの検証処理が行われた結果が連携され、当オブジェクトに印刷用のインスタンスを詰め込み Web API へ連携する想定
    /// だいぶ厳しい
    /// </summary>
    [DataContract] public class ReceiptImporterReportSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public bool IsPossible { get; set; }
        [DataMember] public ReceiptInput[] Items { get; set; }
    }
}
