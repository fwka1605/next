using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>入金データ検索 帳票</summary>
    public interface IReceiptSearchReportProcessor
    {
        Task<byte[]> GetAsync(ReceiptSearch option, CancellationToken token = default(CancellationToken));
    }
}
