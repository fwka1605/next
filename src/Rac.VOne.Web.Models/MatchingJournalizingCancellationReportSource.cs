using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>消込仕訳取消 印刷用モデル
    /// UI上で、どの消込を取消すか選択するため、モデルが必要</summary>
    [DataContract] public class MatchingJournalizingCancellationReportSource
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int Precision { get; set; }
        [DataMember] public List<MatchingJournalizingDetail> Items { get; set; }
    }
}
