using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common.Importers
{
    public interface IBillingAccountTransferFileImportProcessor
    {
        Task<AccountTransferImportResult> ReadAsync(AccountTransferImportSource source, CancellationToken token = default(CancellationToken));
        Task<AccountTransferImportResult> ImportAsync(AccountTransferImportSource source, CancellationToken token = default(CancellationToken));
    }
}
