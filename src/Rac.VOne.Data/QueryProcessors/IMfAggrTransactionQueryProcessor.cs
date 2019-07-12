using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IMfAggrTransactionQueryProcessor
    {
        Task<IEnumerable<long>> GetIdsAsync(CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MfAggrTransaction>> GetAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MfAggrTransaction>> GetLastOneAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken));
    }
}
