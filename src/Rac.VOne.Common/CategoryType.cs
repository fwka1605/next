using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common
{
    /// <summary>
    ///  区分 識別タイプ
    /// </summary>
    public static class CategoryType
    {
        /// <summary>
        ///  1 : 請求区分
        /// </summary>
        public static int Billing => 1;

        /// <summary>
        ///  2 : 入金区分
        /// </summary>
        public static int Receipt => 2;

        /// <summary>
        ///  3 : 回収区分
        /// </summary>
        public static int Collect => 3;

        /// <summary>
        ///  4 : 対象外区分
        /// </summary>
        public static int Exclude => 4;
    }
}
