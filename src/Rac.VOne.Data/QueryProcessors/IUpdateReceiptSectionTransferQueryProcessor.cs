using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateReceiptSectionTransferQueryProcessor
    {
        Task<IEnumerable<ReceiptSectionTransfer>> UpdateReceiptSectionTransferPrintFlagAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<ReceiptSectionTransfer> UpdateDestinationSectionAsync(int DestinationSectionId, int LoginUserId, long SourceReceiptId, long DestinationReceiptId, CancellationToken token = default(CancellationToken));
    }
}
