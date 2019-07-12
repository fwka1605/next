using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    public interface IMatchingJournalizingCancellationReportProcessor
    {
        Task<byte[]> GetAsync(MatchingJournalizingCancellationReportSource source, CancellationToken token = default(CancellationToken));
    }
}
