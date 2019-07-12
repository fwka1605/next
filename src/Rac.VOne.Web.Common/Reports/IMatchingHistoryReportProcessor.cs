using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>消込履歴データ検索 帳票</summary>
    public interface IMatchingHistoryReportProcessor
    {
        Task<byte[]> GetAsync(MatchingHistorySearch option, CancellationToken token = default(CancellationToken));
    }
}
