using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    /// <summary>消込仕訳（前受入金 計上/振替） 出力フラグ更新用</summary>
    public interface IUpdateReceiptMatchingJournalizingQueryProcessor
    {
        Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<int> CancelDetailAsync(Receipt detail, CancellationToken token = default(CancellationToken));
    }
}
