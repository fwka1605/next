using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>得意先別消込台帳 帳票</summary>
    public interface ICustomerLedgerReportProcessor
    {
        Task<byte[]> GetAsync(CustomerLedgerSearch option, IProgressNotifier notifier, CancellationToken token = default(CancellationToken));
    }
}
