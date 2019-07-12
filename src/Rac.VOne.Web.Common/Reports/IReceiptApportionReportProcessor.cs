using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>入金データ振分 帳票</summary>
    public interface IReceiptApportionReportProcessor
    {
        Task<byte[]> GetAsync(ReceiptApportionForReportSource source, CancellationToken token = default(CancellationToken));
    }
}
