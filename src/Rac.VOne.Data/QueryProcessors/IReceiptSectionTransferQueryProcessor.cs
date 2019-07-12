using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptSectionTransferQueryProcessor
    {
        Task<IEnumerable<ReceiptSectionTransfer>> GetReceiptSectionTransferForPrintAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<ReceiptSectionTransfer> GetItemByReceiptIdAsync(ReceiptSectionTransfer ReceiptSectionTransfer, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptSectionTransfer>> GetItemsAsync(ReceiptSectionTransfer ReceiptSectionTransfer, CancellationToken token = default(CancellationToken));
    }
}
