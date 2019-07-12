using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class MenuAuthoritySearch
    {
        /// <summary>
        ///  会社ID
        ///  未指定時は初期メニュー一覧を取得する ※ Menuテーブルの全項目(絞り込み条件なし)
        /// </summary>
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int? LoginUserId { get; set; }
    }
}
