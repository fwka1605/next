using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>個別消込チェックリスト 帳票</summary>
    public interface IMatchingIndividualReportProcessor
    {
        Task<byte[]> GetAsync(MatchingIndividualReportSource source, CancellationToken token = default(CancellationToken));
    }
}
