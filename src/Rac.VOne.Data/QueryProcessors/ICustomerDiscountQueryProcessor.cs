using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICustomerDiscountQueryProcessor
    {
        Task<IEnumerable<CustomerDiscount>> GetItemsAsync(CustomerSearch option, CancellationToken token = default(CancellationToken));
        Task<bool> ExistAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<CustomerDiscount>> GetAsync(int customerId, CancellationToken token = default(CancellationToken));
    }
}
