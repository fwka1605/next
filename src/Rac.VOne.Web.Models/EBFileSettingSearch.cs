using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class EBFileSettingSearch
    {
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int[] Ids { get; set; }
        /// <summary>
        /// 利用可否 更新時に指定するログインユーザーID
        /// </summary>
        [DataMember] public int? LoginUserId { get; set; }
        /// <summary>
        /// 利用可否 更新時に指定するID配列
        /// </summary>
        [DataMember] public int[] UpdateIds { get; set; }
    }
}
