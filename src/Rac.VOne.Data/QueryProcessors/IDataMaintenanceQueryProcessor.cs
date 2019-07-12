using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDataMaintenanceQueryProcessor
    {
        Task<int> DeleteBillingBeforeAsync(DateTime date, CancellationToken token = default(CancellationToken));

        Task<int> DeleteReceiptBeforeAsync(DateTime date, CancellationToken token = default(CancellationToken));
    }
}
