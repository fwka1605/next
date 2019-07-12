using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    /// <summary>通常 入金仕訳 出力フラグ更新用</summary>
    public interface IUpdateReceiptJournalizingQueryProcessor
    {
        Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));

    }
}
