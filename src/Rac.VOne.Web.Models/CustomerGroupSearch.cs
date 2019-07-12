using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerGroupSearch
    {
        [DataMember] public int? CompanyId { get; set; }
        /// <summary>親得意先IDの配列</summary>
        [DataMember] public int[] ParentIds { get; set; }
        /// <summary>子得意先IDの配列 children?</summary>
        [DataMember] public int[] ChildIds { get; set; }
        /// <summary>単一得意先 の 親子関係を CustomerGroup として取得</summary>
        [DataMember] public bool RequireSingleCusotmerRelation { get; set; }
        /// <summary>得意先コード</summary>
        [DataMember] public string Code { get; set; }

    }
}
