using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>入金予定日変更用 モデル 変更用の property が必要</summary>
    [DataContract] public class BillingDueAtModify
    {
        [DataMember] public long? Id { get; set; }
        [DataMember] public long? BillingInputId { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public decimal TargetAmount { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public DateTime? OriginalDueAt { get; set; }
        [DataMember] public DateTime? ModifiedDueAt { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public int? OriginalCollectCategoryId { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string OriginalCollectCategoryCode { get; set; }
        [DataMember] public string OriginalCollectCategoryName { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        #region 更新処理用 画面側で、変更があった場合に、値をセットする
        [DataMember] public DateTime? NewDueAt { get; set; }
        [DataMember] public int? NewCollectCategoryId { get; set; }
        #endregion

        /// <summary>当初 または 変更済の回収区分</summary>
        public string CollectCategory
            => !string.IsNullOrEmpty(CollectCategoryCode)
            ?  $"{CollectCategoryCode}：{CollectCategoryName}" : "";

        /// <summary>当初回収区分
        /// COALESCE(OriginalCollectCategoryId, CollectCategoryId) の結果にjoin したもの </summary>
        public string OriginalCollectCategory
            => !string.IsNullOrEmpty(OriginalCollectCategoryCode)
            ? $"{OriginalCollectCategoryCode}：{OriginalCollectCategoryName}" : "";

        public bool IsDueAtModified
            => ModifiedDueAt != null;

        public BillingDueAtModify Clone() => new BillingDueAtModify {
            Id = Id,
            BillingInputId = BillingInputId,
            CompanyId = CompanyId,
            CurrencyId = CurrencyId,
            CustomerId = CustomerId,
            InvoiceCode = InvoiceCode,
            TargetAmount = TargetAmount,
            DueAt = DueAt,
            OriginalDueAt = OriginalDueAt,
            ModifiedDueAt = ModifiedDueAt,
            CollectCategoryId = CollectCategoryId,
            OriginalCollectCategoryId = OriginalCollectCategoryId,
            CustomerCode = CustomerCode,
            CustomerName = CustomerName,
            CurrencyCode = CurrencyCode,
            CollectCategoryCode = CollectCategoryCode,
            CollectCategoryName = CollectCategoryName,
            OriginalCollectCategoryCode = OriginalCollectCategoryCode,
            OriginalCollectCategoryName = OriginalCollectCategoryName,
            UpdateBy = UpdateBy,
            UpdateAt = UpdateAt,
        };
    }

    [DataContract] public class BillingDueAtModifyResults : IProcessResult
    {
        [DataMember] public List<BillingDueAtModify> Billings { get; set; }
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }
}
