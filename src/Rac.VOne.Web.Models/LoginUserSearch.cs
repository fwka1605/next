using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class LoginUserSearch
    {
        [DataMember] public bool UseCommonSearch { get; set; }
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int[] Ids { get; set; }
        [DataMember] public string[] Codes { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int? UseClient { get; set; }

        /// <summary>入金部門・担当者対応マスターから 指定したログインユーザーコードを除外して検索
        /// ※インポート 上書き時に、指定漏れを検索する用途
        /// </summary>
        [DataMember] public string[] ExcludeCodes { get; set; }
    }
}
