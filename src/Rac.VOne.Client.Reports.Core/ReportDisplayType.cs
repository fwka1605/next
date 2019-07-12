using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Reports
{
    /// <summary>
    ///  消込承認 表示モード
    ///  ※ 内容詳細未確認 todo:名称変更
    /// </summary>
    public enum ReportDisplayType
    {
        /// <summary>
        ///  0 : 実績値
        /// </summary>
        MatchingAmount = 0,
        /// <summary>
        ///  1 : 予定額
        /// </summary>
        TargetAmount = 1,
    }
}
