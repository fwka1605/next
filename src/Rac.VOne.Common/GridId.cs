using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common
{
    /// <summary>
    ///  グリッド表示設定 グリッドID
    /// </summary>
    public static class GridId
    {
        /// <summary>
        ///  1 : 請求データ検索(未消込削除)
        /// </summary>
        public static int BillingSearch => 1;

        /// <summary>
        ///  2 : 入金データ検索(未消込削除)
        /// </summary>
        public static int ReceiptSearch => 2;

        /// <summary>
        ///  3 : 請求データ 個別消込明細
        /// </summary>
        public static int BillingMatchingIndividual => 3;

        /// <summary>
        ///  4 : 入金データ 個別消込明細
        /// </summary>
        public static int ReceiptMatchingIndividual => 4;

        /// <summary>
        ///  5 : 入金データ入力
        /// </summary>
        public static int PaymentScheduleInput => 5;
        /// <summary>
        /// 6：請求書発行
        /// </summary>
        public static int BillingInvoicePublish => 6;
    }
}
