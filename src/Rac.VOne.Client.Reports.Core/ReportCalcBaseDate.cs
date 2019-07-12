using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  日数計算基準
    /// </summary>
    public enum ReportCalcBaseDate
    {
        /// <summary>
        ///  0 : 当初予定日
        /// </summary>
        OrigianlDueAt = 0,
        /// <summary>
        ///  1 : 入金予定日
        /// </summary>
        DueAt = 1,
        /// <summary>
        ///  2 : 請求日
        /// </summary>
        BilledAt = 2,
    }
}
