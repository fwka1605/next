using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  日付 フォーマット
    ///  ※簡易請求書発行 入金期日書式指定
    /// </summary>
    public enum ReportDateFormat
    {
        /// <summary>
        ///  0 : yyyy年MM月dd日
        /// </summary>
        yyyyMMddJp = 0,
        /// <summary>
        ///  1 : yyyy/MM/dd
        /// </summary>
        yyyyMMdd = 1,
        /// <summary>
        ///  2 : ee年MM月dd日
        /// </summary>
        eeMMddJp = 2,
        /// <summary>
        ///  3 : ee/MM/dd
        /// </summary>
        eeMMdd = 3,
    }
}
