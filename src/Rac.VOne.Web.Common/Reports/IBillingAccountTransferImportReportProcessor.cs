using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>口座振替結果取込 帳票</summary>
    public interface IBillingAccountTransferImportReportProcessor
    {
        Task<byte[]> GetAsync(AccountTransferImportSource source, CancellationToken token = default(CancellationToken));
    }
}
