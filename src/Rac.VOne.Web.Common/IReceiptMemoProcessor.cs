using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IReceiptMemoProcessor
    {
        Task<ReceiptMemo> SaveAsync(long ReceiptId, string Memo, CancellationToken token = default(CancellationToken));
        Task<ReceiptMemo> GetAsync(long ReceiptId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptMemo>> GetItemsAsync(IEnumerable<long> receiptIds, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(long ReceiptId, CancellationToken token = default(CancellationToken));
    }
}
