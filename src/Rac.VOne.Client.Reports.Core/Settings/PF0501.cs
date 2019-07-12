using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports.Settings
{
    /// <summary>得意先別消込台帳 の 帳票設定</summary>
    public class PF0501
    {

        /// <summary>1 : 集計基準日</summary>
        public static int TargetDate => 1;
        /// <summary>2 : 請求データ集計</summary>
        public static int TotalByDay => 2;
        /// <summary>3 : 月次改ページ</summary>
        public static int MonthlyBreak => 3;
        /// <summary>4 : 伝票集計方法</summary>
        public static int SlipTotal => 4;
        /// <summary>5 : 入金データ集計</summary>
        public static int ReceiptGrouping => 5;
        /// <summary>6 : 請求残高計算方法</summary>
        public static int RemainType => 6;
        /// <summary>7 : 請求部門コード表示</summary>
        public static int DisplayDepartmentCode => 7;
        /// <summary>8 : 入金部門コード表示</summary>
        public static int DisplaySectionCode => 8;
        /// <summary>9 : 金額単位</summary>
        public static int UnitPrice => 9;
        /// <summary>10: 消込記号表示</summary>
        public static int DisplaySymbol => 10;

    }
}
