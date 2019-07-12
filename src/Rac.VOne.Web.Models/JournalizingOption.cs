using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class JournalizingOption
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int? CurrencyId { get; set; }
        [DataMember] public int? CustomerId { get; set; }
        [DataMember] public bool IsOutputted { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public DateTime? RecordedAtFrom { get; set; }
        [DataMember] public DateTime? RecordedAtTo { get; set; }

        [DataMember] public List<DateTime> OutputAt { get; set; } = new List<DateTime>();
        [DataMember] public bool UseDiscount { get; set; }
        /// <summary>消込済 入金データ出力用 前受発生を含めるかどうか</summary>
        [DataMember] public bool ContainAdvanceReceivedOccured { get; set; }
        /// <summary>消込済 入金データ出力用 前受振替の消込データを含めるかどうか</summary>
        [DataMember] public bool ContainAdvanceReceivedMatching { get; set; }

        [DataMember] public DateTime? CreateAtFrom { get; set; }
        [DataMember] public DateTime? CreateAtTo { get; set; }
        /// <summary>消込仕訳取消 仕訳タイプ
        /// <see cref="Rac.VOne.Common.Constants.JournalizingType"/></summary>の<see cref="List{T}"/>
        [DataMember] public List<int> JournalizingTypes { get; set; } = new List<int>();
        /// <summary>汎用仕訳出力かどうか</summary>
        [DataMember] public bool IsGeneral { get; set; }
        /// <summary>MFC請求書用：補助科目に得意先名を出力する</summary>
        [DataMember] public bool OutputCustoemrName { get; set; }

        /// <summary>
        /// 帳票用
        /// </summary>
        [DataMember] public int Precision { get; set; }
    }
}
