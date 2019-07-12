using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common
{
    /// <summary>Web Service Error Code の一覧</summary>
    /// <remarks>
    /// http status code との兼ね合いがある
    /// 通常, 403 などで返すべき箇所が web service の場合どうなるか
    /// Web Service / client 共に この 定数群を利用していれば、ここを変えるだけで良い
    /// ※ 一度外部へ公開してしまった場合、数字に意味合いが生まれるので、変更できない
    /// ※ 認証系 110...113/121..122 は さくらケーシーエス様と共有している情報のため、変更を行う場合は、十分な告知が必要になる点に注意
    /// </remarks>
    public static class ErrorCode
    {
        /// <summary>エラーなし</summary>
        public const string None = "0";

        /// <summary>原因の詳細を特定していない内部例外が発生</summary>
        public const string ExceptionOccured = "99";


        /// <summary>認証キーが不正です。</summary>
        public const string InvalidAuthenticationKey = "110";
        /// <summary>DB接続に失敗しました。</summary>
        public const string CompanyDataBaseConnectionFailure = "111";
        /// <summary>セッションキーの生成に失敗しました。</summary>
        public const string SessionKeyCreationFailure = "112";
        /// <summary>会社コードが不正です。</summary>
        public const string InvalidCompanyCode = "113";

        /// <summary>セッションキーが無効です</summary>
        public const string InvalidSessionKey = "121";
        /// <summary>セッションキーが有効期限切れです</summary>
        public const string SessionKeyExpired = "122";

        /// <summary>更新系：他ユーザーにより更新済</summary>
        public const string OtherUserAlreadyUpdated = "4031";
    }
}
