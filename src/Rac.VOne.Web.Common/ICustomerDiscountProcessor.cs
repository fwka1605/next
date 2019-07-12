using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICustomerDiscountProcessor
    {
        Task<IEnumerable<CustomerDiscount>> GetAsync(int customerId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<CustomerDiscount>> GetItemsAsync(CustomerSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken));
        Task<CustomerDiscount> SaveAsync(CustomerDiscount CustomerDiscount, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(CustomerDiscount discount, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<CustomerDiscount> insert,
            IEnumerable<CustomerDiscount> update,
            IEnumerable<CustomerDiscount> delete,
            CancellationToken token = default(CancellationToken));
    }
}
