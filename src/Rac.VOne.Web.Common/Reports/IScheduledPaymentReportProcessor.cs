using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>入金予定一覧 帳票</summary>
    public interface IScheduledPaymentReportProcessor
    {
        Task<byte[]> GetAsync(ScheduledPaymentListSearch option, CancellationToken token = default(CancellationToken));
    }
}
