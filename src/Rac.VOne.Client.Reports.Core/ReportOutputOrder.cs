using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  出力順
    /// </summary>
    public enum ReportOutputOrder
    {
        /// <summary>
        ///  0 : 得意先コード順
        /// </summary>
        ByCustomerCode = 0,
        /// <summary>
        ///  1 : 日付順
        /// </summary>
        ByDate = 1,
        /// <summary>
        ///  2 : 登録順
        /// </summary>
        ById = 2
    }
}
