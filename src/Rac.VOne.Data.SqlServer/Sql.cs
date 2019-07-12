using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer
{
    public static class Sql
    {
        /// <summary>SQL Server LIKE 用の escape 処理</summary>
        /// <param name="value"></param>
        /// <returns>
        /// to escape below SQL Server symbols
        /// _ : under score
        /// % : percent
        /// [ : blaket
        /// </returns>
        /// <remarks>
        /// _ -> [_]
        /// % -> [%]
        /// [ -> [[]
        /// sample
        ///  10% discount -> 10[%] discount
        /// </remarks>
        public static string EscapeSqlLike(string value)
            => Regex.Replace(value, "([_%\\[])", "[$1]");

        /// <summary>
        ///     Wrap for SQL Server Like string parameter.
        ///      value -> %{EscapeSqlLike(value)}%
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetWrappedValue(this string value)
            => $"%{EscapeSqlLike(value)}%";

        /// <summary>Id, UpdateAt を利用した Update 処理で、
        /// 影響を与えた件数が 0 件の場合に 例外を出力する記述を追加する拡張メソッド
        /// </summary>
        public static string AppendIfNotAnyRowsAffectedThenRaiseError(this string value)
            => value + @"
IF @@ROWCOUNT = 0
BEGIN
    RAISERROR ('this row was changed by another user', 18, 1)
END;";

    }
}
