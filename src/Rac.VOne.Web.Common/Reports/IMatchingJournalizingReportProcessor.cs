using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>消込仕訳出力 帳票</summary>
    public interface IMatchingJournalizingReportProcessor
    {
        Task<byte[]> GetAsync(MatchingJournalizingReportSource source, CancellationToken token = default(CancellationToken));
    }
}
