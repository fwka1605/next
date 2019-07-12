using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  与信限度額集計方法
    /// </summary>
    public enum ReportCreditLimitType
    {
        /// <summary>
        ///  0 : 得意先の与信限度額を集計する
        /// </summary>
        UseCustomerSummaryCredit = 0,
        /// <summary>
        ///  1 : 債権代表者の与信限度額を使用する
        /// </summary>
        UseParentCustomerCredit = 1,
    }
}
