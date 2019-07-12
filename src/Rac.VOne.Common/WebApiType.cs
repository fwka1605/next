using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common
{
    /// <summary>
    ///  外部連携 API のタイプ
    /// </summary>
    public static class WebApiType
    {
        /// <summary>
        ///  1 : 働くDB
        /// </summary>
        public const int HatarakuDb = 1;
        /// <summary>
        ///  2 : PCA クラウド
        /// </summary>
        public const int PcaDX = 2;

        /// <summary>
        ///  3 : Money Forward クラウド
        /// </summary>
        public const int MoneyForward = 3;
    }
}
