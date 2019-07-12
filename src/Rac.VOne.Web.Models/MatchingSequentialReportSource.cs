using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class MatchingSequentialReportSource
    {
        [DataMember] public int CompanyId { get; set; }

        /// <summary>入金データを優先 標準：請求/入金 有効化した場合 入金/請求 の順で表示</summary>
        [DataMember] public bool PriorReceipt { get; set; }
        [DataMember] public int Precision { get; set; }
        [DataMember] public List<Collation> Items { get; set; }
    }
}
