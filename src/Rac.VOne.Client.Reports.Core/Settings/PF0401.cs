using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports.Settings
{
    /// <summary>滞留明細一覧表</summary>
    public class PF0401
    {
        /// <summary>1 : 部門計出力</summary>
        public static int DepartmentTotal => 1;
        /// <summary>2 : 担当者計出力</summary>
        public static int StaffTotal => 2;
        /// <summary>3 : 得意先計出力</summary>
        public static int CustomerTotal => 3;
        /// <summary>4 : 予定日計出力</summary>
        public static int DueAtTotal => 4;
        /// <summary>5 : 金額単位</summary>
        public static int UnitPrice => 5;
        /// <summary>6 : 出力順</summary>
        public static int OutputOrder => 6;
        /// <summary>7 : 日付基準</summary>
        public static int BaseDateWithOriginal => 7;
        /// <summary></summary>
        public static int CaclBaseDate => 8;
        /// <summary>9 : 滞留日数計算に当日を含める</summary>
        public static int IncludeOrNot => 9;
    }
}
