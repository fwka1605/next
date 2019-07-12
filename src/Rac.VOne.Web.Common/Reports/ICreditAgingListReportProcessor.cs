using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>債権総額管理表 帳票</summary>
    public interface ICreditAgingListReportProcessor
    {
        Task<byte[]> GetAsync(CreditAgingListSearch option, IProgressNotifier notifier = null, CancellationToken token = default(CancellationToken));
    }
}
