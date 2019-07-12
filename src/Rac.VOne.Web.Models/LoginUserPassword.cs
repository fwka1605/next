using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>
    ///  ログインユーザーの パスワード履歴
    ///  ハッシュ化されたデータとはいえ、クライアント側に値を渡すことはない
    ///  DataMember などの属性設定は不要
    /// </summary>
    public class LoginUserPassword
    {
        public int LoginUserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime UpdateAt { get; set; }
        public string PasswordHash0 { get; set; } = string.Empty;
        public string PasswordHash1 { get; set; } = string.Empty;
        public string PasswordHash2 { get; set; } = string.Empty;
        public string PasswordHash3 { get; set; } = string.Empty;
        public string PasswordHash4 { get; set; } = string.Empty;
        public string PasswordHash5 { get; set; } = string.Empty;
        public string PasswordHash6 { get; set; } = string.Empty;
        public string PasswordHash7 { get; set; } = string.Empty;
        public string PasswordHash8 { get; set; } = string.Empty;
        public string PasswordHash9 { get; set; } = string.Empty;

        /// <summary>
        ///  新しいPassword Hash が正しいかどうか
        /// </summary>
        /// <param name="newHash"></param>
        /// <param name="history">確認する履歴</param>
        /// <returns></returns>
        public bool Validate(string newHash, int history)
        {
            var valid = true;

            valid &= !string.IsNullOrEmpty(newHash);

            if (1 <= history)
            {
                valid &= newHash != PasswordHash;
                valid &= newHash != PasswordHash0;
            }

            if (2  <= history) valid &= newHash != PasswordHash1;
            if (3  <= history) valid &= newHash != PasswordHash2;
            if (4  <= history) valid &= newHash != PasswordHash3;
            if (5  <= history) valid &= newHash != PasswordHash4;
            if (6  <= history) valid &= newHash != PasswordHash5;
            if (7  <= history) valid &= newHash != PasswordHash6;
            if (8  <= history) valid &= newHash != PasswordHash7;
            if (9  <= history) valid &= newHash != PasswordHash8;
            if (10 <= history) valid &= newHash != PasswordHash9;
            return valid;
        }
    }


    public enum LoginResult
    {
        /// <summary>データベースエラー</summary>
        DBError = 0,
        /// <summary>成功</summary>
        Success = 1,
        /// <summary>失敗</summary>
        Failed = 2,
        /// <summary>期限切れ</summary>
        Expired = 3,
    }

    [DataContract] public class LoginProcessResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult {get;set;}
        [DataMember] public LoginResult Result { get; set; }
    }

    public enum PasswordChangeResult
    {
        /// <summary>データベースエラー</summary>
        DBError = 0,
        /// <summary>成功</summary>
        Success = 1,
        /// <summary>失敗 古いパスワードが異なる</summary>
        Failed = 2,
        /// <summary>過去登録しているものと同一</summary>
        ProhibitionSamePassword = 3,
    }

    [DataContract] public class LoginPasswordChangeResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public PasswordChangeResult Result { get; set; }
    }
}
