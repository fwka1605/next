using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  金額単位
    /// </summary>
    public enum ReportUnitPrice
    {
        /// <summary>
        ///  0 : 1円
        /// </summary>
        Per1 = 0,
        /// <summary>
        ///  1 : 千円
        /// </summary>
        Per1000 = 1,
        /// <summary>
        ///  2 : 万円
        /// </summary>
        Per10000 = 2,
        /// <summary>
        ///  3 : 百万円
        /// </summary>
        Per1000000 = 3,
    }
}
