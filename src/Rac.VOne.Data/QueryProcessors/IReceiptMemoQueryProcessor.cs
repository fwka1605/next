using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptMemoQueryProcessor
    {
        Task<ReceiptMemo> GetAsync(long receiptId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptMemo>> GetItemsAsync(IEnumerable<long> receiptIds, CancellationToken token = default(CancellationToken));
    }
}
