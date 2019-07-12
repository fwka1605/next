using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  基準日 +初期値
    /// </summary>
    public enum ReportBaseDateWithOriginal
    {
        /// <summary>
        ///  0 : 請求日
        /// </summary>
        BilledAt = 0,
        /// <summary>
        ///  1 : 売上日
        /// </summary>
        SalesAt = 1,
        /// <summary>
        ///  2 : 請求締日
        /// </summary>
        ClosingAt = 2,
        /// <summary>
        ///  3 : 入金予定日
        /// </summary>
        DueAt = 3,
        /// <summary>
        ///  4 : 当初予定日
        /// </summary>
        OrigianlDueAt = 4,
    }
}
