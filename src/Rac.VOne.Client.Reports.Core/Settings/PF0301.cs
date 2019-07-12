using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports.Settings
{
    /// <summary>
    ///  入金予定明細表 の 帳票設定
    /// </summary>
    public class PF0301
    {
        /// <summary>1 : 部門計要否</summary>
        public static int DepartmentTotal => 1;
        /// <summary>2 : 担当者計要否</summary>
        public static int StaffTotal => 2;
        /// <summary>3 : 得意先計要否</summary>
        public static int CustomerTotal => 3;
        /// <summary>4 : 予定日計要否</summary>
        public static int DueAtTotal => 4;
        /// <summary>5 : 金額単位</summary>
        public static int UnitPrice => 5;
        /// <summary>6 : 出力順</summary>
        public static int OutputOrder => 6;
        /// <summary>7 : 集計基準日</summary>
        public static int TargetDate => 7;
        /// <summary>8 : 遅延計算に当日を含める</summary>
        public static int IncludeOrNot => 8;
        /// <summary>8 : 日数計算基準</summary>
        public static int CalcBaseDate => 9;

    }
}
