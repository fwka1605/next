using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>定期請求データ作成済 フラグ管理用</summary>
    [DataContract]
    public class PeriodicBillingCreated
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public long PeriodicBillingSettingId { get; set; }
        [DataMember] public DateTime CreateYearMonth { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

}
