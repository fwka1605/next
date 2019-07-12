using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>請求データ検索 帳票</summary>
    public interface IBillingSearchReportProcessor
    {
        Task<byte[]> GetAsync(BillingSearch option, CancellationToken token = default(CancellationToken));
    }
}
