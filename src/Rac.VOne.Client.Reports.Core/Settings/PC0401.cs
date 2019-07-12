using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports.Settings
{
    /// <summary>請求書発行</summary>
    public class PC0401
    {
        /// <summary>
        ///  1 : 請求日付
        /// </summary>
        public static int ReportInvoiceDate => 1;

        /// <summary>
        ///  2 : ロゴの表示
        /// </summary>
        public static int DisplayLogo => 2;

        /// <summary>
        ///  3 : 社判の表示
        /// </summary>
        public static int DisplayOfficialSeal => 3;

        /// <summary>
        ///  4 : 丸印の表示
        /// </summary>
        public static int DisplayRoundSeal => 4;

        /// <summary>
        ///  5 : 住所の出力
        /// </summary>
        public static int DisplayAddress => 5;

        /// <summary>
        ///  6 : 承認欄の出力
        /// </summary>
        public static int DisplayApproval => 6;

        /// <summary>
        ///  7 : 請求金額
        /// </summary>
        public static int ReportInvoiceAmount => 7;

        /// <summary>
        ///  8 : 明細に数量単価単位の表示
        /// </summary>
        public static int DisplayQuantity => 8;

        /// <summary>
        ///  9 : 売上日を取引日として明細に表示
        /// </summary>
        public static int DisplaySalesAt => 9;

        /// <summary>
        ///  10 : 仮想口座を利用
        /// </summary>
        public static int DisplayVirtualAccount => 10;

        /// <summary>
        ///  11 : 本書と控えを出力
        /// </summary>
        public static int OutputCopy => 11;

        /// <summary>
        ///  12 : 再発行の際、「再発行」文面を表示
        /// </summary>
        public static int DisplayRePrint => 12;
    }
}
