using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  請求書日付
    /// </summary>
    public enum ReportInvoiceDate
    {
        /// <summary>
        ///  0 : 作成日(システム日付)
        /// </summary>
        SystemDate = 0,
        /// <summary>
        ///  1 : 請求締日
        /// </summary>
        ClosingAt = 1,
        /// <summary>
        ///  2 : 請求日
        /// </summary>
        BilledAt = 2,
    }
}
