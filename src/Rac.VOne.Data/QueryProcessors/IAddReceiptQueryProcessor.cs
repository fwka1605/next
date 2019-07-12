using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReceiptQueryProcessor
    {
        Task<Receipt> SaveAsync(Receipt Receipt, bool specifyCreateAt = false, CancellationToken token = default(CancellationToken));
        Task<Receipt> AddAdvanceReceivedAsync(long receiptId,int? customerId, int updateBy, DateTime updateAt, DateTime newUpdateAt, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptInput>> SaveReceiptInputAsync(IEnumerable<ReceiptInput> ReceiptInput, CancellationToken token = default(CancellationToken));
    }
}
