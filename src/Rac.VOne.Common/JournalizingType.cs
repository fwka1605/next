using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Common
{
    /// <summary>
    /// 仕訳タイプ
    /// </summary>
    public static class JournalizingType
    {
        /// <summary>0 : 入金仕訳</summary>
        public const int Receipt = 0;
        /// <summary>1 : 消込仕訳</summary>
        public const int Matching = 1;
        /// <summary>2 : 前受計上仕訳</summary>
        public const int AdvanceReceivedOccured = 2;
        /// <summary>3 : 前受振替仕訳</summary>
        public const int AdvanceReceivedTransfer = 3;
        /// <summary>4 : 入金対象外仕訳</summary>
        public const int ReceiptExclude = 4;

    }
}
