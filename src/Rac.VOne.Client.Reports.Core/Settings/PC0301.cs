using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports.Settings
{
    /// <summary>
    ///  請求データ検索 の 帳票設定
    /// </summary>
    public class PC0301
    {
        /// <summary>
        ///  1 : 部門計出力
        /// </summary>
        public static int DepartmentSubtotal => 1;

        /// <summary>
        ///  2 : 担当者計出力
        /// </summary>
        public static int StaffSubtotal => 2;

        /// <summary>
        ///  3 : 得意先計出力
        /// </summary>
        public static int CustomerSubtotal => 3;

        /// <summary>
        ///  4 : 金額単位
        /// </summary>
        public static int UnitPrice => 4;

        /// <summary>
        ///  5 : 出力順
        /// </summary>
        public static int OutputOrder => 5;

        /// <summary>
        ///  6 : 日付基準
        /// </summary>
        public static int OrderDateType => 6;

    }
}
