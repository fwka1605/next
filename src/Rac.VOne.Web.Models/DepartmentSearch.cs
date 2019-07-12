using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class DepartmentSearch
    {
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int[] Ids { get; set; }
        [DataMember] public string[] Codes { get; set; }
        /// <summary>
        /// すでに登録済で除外したい 請求部門ID配列
        /// 入金部門 関連マスターで、請求部門検索時に利用
        /// </summary>
        [DataMember] public int[] SkipIds { get; set; }
        /// <summary>
        /// 入金部門ID を指定することで、自入金部門ID 以外で登録されている 請求部門を除外する
        /// </summary>
        [DataMember] public int? WithSectionId { get; set; }
        /// <summary>
        /// 入金部門 ログインユーザー の制限ありの場合に指定
        /// </summary>
        [DataMember] public int? LoginUserId { get; set; }

    }
}
