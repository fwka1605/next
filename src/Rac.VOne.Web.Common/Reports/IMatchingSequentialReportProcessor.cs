using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>一括消込チェックリスト 帳票</summary>
    public interface IMatchingSequentialReportProcessor
    {
        Task<byte[]> GetAsync(MatchingSequentialReportSource source, CancellationToken token = default(CancellationToken));
    }
}
