using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// ログイン パラメータ
    /// </summary>
    [DataContract]
    public class LoginParameters
    {
        /// <summary>会社ID</summary>
        [DataMember] public int? CompanyId { get; set; }
        /// <summary>会社コード</summary>
        [DataMember] public string CompanyCode { get; set; }
        /// <summary>ログインユーザーコード</summary>
        [DataMember] public string UserCode { get; set; }
        /// <summary>ログインユーザーID</summary>
        [DataMember] public int? LoginUserId { get; set; }
        /// <summary>パスワード</summary>
        [DataMember] public string Password { get; set; }
        /// <summary>パスワード変更時 旧パスワード</summary>
        [DataMember] public string OldPassword { get; set; }
    }
}
