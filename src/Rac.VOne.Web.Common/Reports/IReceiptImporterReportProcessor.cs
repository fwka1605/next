using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>入金フリーインポーター 帳票</summary>
    public interface IReceiptImporterReportProcessor
    {
        Task<byte[]> GetAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken));
    }
}
