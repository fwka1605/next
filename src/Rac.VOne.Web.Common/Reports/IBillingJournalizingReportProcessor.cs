using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>請求仕訳出力 帳票</summary>
    public interface IBillingJournalizingReportProcessor
    {
        Task<byte[]> GetAsync(BillingJournalizingOption option, CancellationToken token = default(CancellationToken));
    }
}
