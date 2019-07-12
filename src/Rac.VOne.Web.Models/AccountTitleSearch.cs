using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class AccountTitleSearch
    {
        [DataMember] public bool UseCommonSearch { get; set; }
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public string[] Codes { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string ContraAccountCode { get; set; }
        [DataMember] public string ContraAccountName { get; set; }
        [DataMember] public string ContraAccountSubCode { get; set; }

        /// <summary>
        /// id の配列 科目マスターの id が分かっている場合に利用
        /// </summary>
        [DataMember] public int[] Ids { get; set; }

    }
}
