using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBillingExistsQueryProcessor
    {
        Task<bool> ExistDebitAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCreditAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCollectCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistBillingCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken));
        Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken));
    }
}
