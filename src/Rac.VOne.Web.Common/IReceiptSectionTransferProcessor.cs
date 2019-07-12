using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IReceiptSectionTransferProcessor
    {
        Task<IEnumerable<ReceiptSectionTransfer>> GetReceiptSectionTransferForPrintAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptSectionTransfer>> UpdateReceiptSectionTransferPrintFlagAsync(int CompanyId, CancellationToken token = default(CancellationToken));

        Task<ReceiptSectionTransfersResult> SaveAsync(IEnumerable<ReceiptSectionTransfer> transfers, CancellationToken token = default(CancellationToken));
    }
}
