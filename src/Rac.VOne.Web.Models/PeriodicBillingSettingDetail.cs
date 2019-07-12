using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PeriodicBillingSettingDetail
    {
        [DataMember] public long PeriodicBillingSettingId { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public int BillingCategoryId { get; set; }
        [DataMember] public int TaxClassId { get; set; }
        [DataMember] public int? DebitAccountTitleId { get; set; }
        [DataMember] public decimal? Quantity { get; set; }
        [DataMember] public string UnitSymbol { get; set; }
        [DataMember] public decimal? UnitPrice { get; set; }
        [DataMember] public decimal? Price { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public string Note1 { get; set; } = string.Empty;
        [DataMember] public string Note2 { get; set; } = string.Empty;
        [DataMember] public string Note3 { get; set; } = string.Empty;
        [DataMember] public string Note4 { get; set; } = string.Empty;
        [DataMember] public string Note5 { get; set; } = string.Empty;
        [DataMember] public string Note6 { get; set; } = string.Empty;
        [DataMember] public string Note7 { get; set; } = string.Empty;
        [DataMember] public string Note8 { get; set; } = string.Empty;
        [DataMember] public string Memo { get; set; } = string.Empty;
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        public bool IsAnyNoteInputted
            => !string.IsNullOrEmpty(Memo)
            || !string.IsNullOrEmpty(Note1)
            || !string.IsNullOrEmpty(Note2)
            || !string.IsNullOrEmpty(Note3)
            || !string.IsNullOrEmpty(Note4)
            || !string.IsNullOrEmpty(Note5)
            || !string.IsNullOrEmpty(Note6)
            || !string.IsNullOrEmpty(Note7)
            || !string.IsNullOrEmpty(Note8);
    }

}
