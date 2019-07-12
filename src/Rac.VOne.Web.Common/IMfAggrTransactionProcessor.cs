using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IMfAggrTransactionProcessor
    {
        Task<int> SaveAsync(IEnumerable<MfAggrTransaction> transactions, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<long>> GetIdsAsync(CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MfAggrTransaction>> GetAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MfAggrTransaction>> GetLastOneAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken));
    }
}
