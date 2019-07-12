using System;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PeriodicBillingSettingSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public long[] Ids { get; set; }
        [DataMember] public DateTime? BaseDate { get; set; }
        /// <summary>再作成用 フラグ</summary>
        [DataMember] public bool ReCreate { get; set; }
        /// <summary>曖昧検索用</summary>
        [DataMember] public string Name { get; set; }
    }
}
