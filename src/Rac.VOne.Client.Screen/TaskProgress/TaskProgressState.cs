using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen
{
    public enum TaskProgressState
    {
        /// <summary>処理中</summary>
        InProcess,
        /// <summary>完了</summary>
        Completed,
        /// <summary>エラー</summary>
        Error,
        /// <summary>キャンセル</summary>
        Cancel
    }
}
