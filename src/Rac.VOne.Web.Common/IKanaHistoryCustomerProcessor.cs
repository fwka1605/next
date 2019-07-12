using System.Collections.Generic;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IKanaHistoryCustomerProcessor
    {
        Task<IEnumerable<KanaHistoryCustomer>> GetAsync(KanaHistorySearch option, CancellationToken token = default(CancellationToken));

        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistAsync(KanaHistoryCustomer KanaHistoryCustomer, CancellationToken token = default(CancellationToken));

        Task<KanaHistoryCustomer> SaveAsync(KanaHistoryCustomer item, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(KanaHistoryCustomer item, CancellationToken token = default(CancellationToken));
        Task<ImportResult> ImportAsync(
            IEnumerable<KanaHistoryCustomer> insert,
            IEnumerable<KanaHistoryCustomer> update,
            IEnumerable<KanaHistoryCustomer> delete, CancellationToken token = default(CancellationToken));
    }
}
