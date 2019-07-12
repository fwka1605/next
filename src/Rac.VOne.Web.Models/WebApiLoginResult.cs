using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    /// web api LoginController の処理結果
    /// </summary>
    [DataContract] public class WebApiLoginResult
    {
        /// <summary>
        /// 作成した セッションキー
        /// </summary>
        [DataMember] public string SessionKey { get; set; }

        /// <summary>
        /// ログイン検証結果 <see cref="Models.LoginResult"/>の int の値
        /// </summary>
        [DataMember] public int LoginResult { get; set; }

        /// <summary>
        /// パスワード検証結果 <see cref="Models.PasswordChangeResult"/>の int の値
        /// </summary>
        [DataMember] public int PasswordChangeResult { get; set; }
    }
}
