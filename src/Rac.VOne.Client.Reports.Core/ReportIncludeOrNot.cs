using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  含めない/含める
    /// </summary>
    public enum ReportIncludeOrNot
    {
        /// <summary>
        ///  0 : 含めない
        /// </summary>
        Exclude = 0,
        /// <summary>
        ///  1 : 含める
        /// </summary>
        Include = 1,
    }
}
