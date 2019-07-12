using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IKanaHistoryCustomerQueryProcessor
    {
        Task<IEnumerable<KanaHistoryCustomer>> GetAsync(KanaHistorySearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistAsync(KanaHistoryCustomer KanaHistoryCustomer, CancellationToken token = default(CancellationToken));
    }
}
