using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  債権総額計算方法
    /// </summary>
    public enum ReportReceiptType
    {
        /// <summary>
        ///  0 : 消込額を使用する
        /// </summary>
        UseMatchingAmount = 0,
        /// <summary>
        ///  1 : 入金額を使用する
        /// </summary>
        UseReceiptAmount = 1,
    }
}
