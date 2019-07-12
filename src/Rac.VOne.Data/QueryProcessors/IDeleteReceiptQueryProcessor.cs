using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteReceiptQueryProcessor
    {
        Task<int> CancelAdvanceReceivedAsync(long id, CancellationToken token = default(CancellationToken));
        Task<int> DeleteByFileLogIdAsync(int ImportFileLogId, CancellationToken token = default(CancellationToken));
        Task<int> OmitByApportionAsync(ReceiptApportion receiptApportion, CancellationToken token = default(CancellationToken));
    }
}
