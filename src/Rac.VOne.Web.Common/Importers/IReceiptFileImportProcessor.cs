using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Importers
{
    public interface IReceiptFileImportProcessor
    {
        Task<ImportDataResult> ReadAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken));

        //Task<IEnumerable<ReceiptInput>> GetValidItemsAsync(long id, CancellationToken token = default(CancellationToken));
        //Task<IEnumerable<ReceiptInput>> GetInvalidItemsAsync(long id, CancellationToken token = default(CancellationToken));

        Task<ImportDataResult> ImportAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken));
    }
}
