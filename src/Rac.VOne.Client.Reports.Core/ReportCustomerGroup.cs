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
    public enum ReportCustomerGroup
    {
        /// <summary>
        ///  0 : 得意先
        /// </summary>
        PlainCusomter = 0,
        /// <summary>
        ///  1 : 債権代表者/得意先
        /// </summary>
        ParentWithChildren = 1,
        /// <summary>
        ///  2 : 債権代表者
        ///  適切な名前を何か
        /// </summary>
        ParentOnly = 2,
    }
}
