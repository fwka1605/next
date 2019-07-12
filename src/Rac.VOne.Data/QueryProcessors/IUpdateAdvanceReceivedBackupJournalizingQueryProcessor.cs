using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    /// <summary>前受振替 仕訳出力時に 退避用テーブルにも 振替仕訳出力日時 を記録/消去するためのinterface</summary>
    public interface IUpdateAdvanceReceivedBackupJournalizingQueryProcessor
    {
        Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));

        Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
    }
}
