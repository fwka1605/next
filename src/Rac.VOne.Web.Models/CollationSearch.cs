using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// 照合/消込用 オプション
    /// </summary>
    [DataContract]
    public class CollationSearch
    {
        [DataMember] public byte[] ClientKey { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int? CurrencyId { get; set; }
        [DataMember] public DateTime? RecordedAtFrom { get; set; }
        [DataMember] public DateTime? RecordedAtTo { get; set; }
        [DataMember] public DateTime? DueAtFrom { get; set; }
        [DataMember] public DateTime? DueAtTo { get; set; }
        /// <summary>
        ///  請求データタイプ
        ///   0 : すべて
        ///   1 : 通常請求
        ///   2 : 期日入金予定
        /// </summary>
        [DataMember] public int BillingType { get; set; }
        [DataMember] public int LimitDateType { get; set; }
        /// <summary>
        ///  請求側 表示金額
        ///   0 : 請求額
        ///   1 : 消込対象額
        /// </summary>
        [DataMember] public int AmountType { get; set; }

        [DataMember] public bool UseDepartmentWork { get; set; }
        [DataMember] public bool UseSectionWork { get; set; }

        [DataMember] public bool Approved { get; set; }

        [DataMember] public int LoginUserId { get; set; }
        /// <summary>入金予定入力 前受振替を行う UI 要素含む</summary>
        [DataMember] public bool DoTransferAdvanceReceived { get; set; }
        [DataMember] public int RecordedAtType { get; set; }
        [DataMember] public DateTime? AdvanceReceivedRecordedAt { get; set; }
        /// <summary>照合設定 前受振替を利用
        /// && 通常得意先の消込 (決済代行会社の場合は false)とする
        /// </summary>
        [DataMember] public bool UseAdvanceReceived { get; set; }
        /// <summary>承認/消込済データ表示用 消込日時From</summary>
        [DataMember] public DateTime? CreateAtFrom { get; set; }
        /// <summary>承認/消込済データ表示用 消込日時To</summary>
        [DataMember] public DateTime? CreateAtTo { get; set; }
        /// <summary> 一括消込 請求情報・入金情報表示順 </summary>
        public SortOrderColumnType SortOrderDirection { get; set; }

        public DateTime? GetRecordedAt(IEnumerable<Billing> billings)
        {
            var recordedAt = AdvanceReceivedRecordedAt;
            if (billings != null && billings.Any())
            {
                if (RecordedAtType == 2)
                    recordedAt = billings.Max(item => item.BilledAt);
                if (RecordedAtType == 3)
                    recordedAt = billings.Max(item => item.SalesAt);
                if (RecordedAtType == 4)
                    recordedAt = billings.Max(item => item.ClosingAt);
                if (RecordedAtType == 5)
                    recordedAt = billings.Max(item => item.DueAt);
                if (RecordedAtType == 6)
                    recordedAt = null;
            }
            return recordedAt;
        }

        public string ConnectionId { get; set; }
    }
}
