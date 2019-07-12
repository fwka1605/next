using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  消込額計算
    /// </summary>
    public enum ReportAdvanceReceivedType
    {
        /// <summary>
        ///  0 : 消込額を使用する
        /// </summary>
        UseMatchingAmount = 0,
        /// <summary>
        ///  1 : 入金額を使用する
        /// </summary>
        UseReceiptAmount = 1,
        /// <summary>
        ///  2 : 消込額を使用して入金額を表示
        /// </summary>
        UseMatchingAmountWithReceiptAmount = 2,
    }
}
