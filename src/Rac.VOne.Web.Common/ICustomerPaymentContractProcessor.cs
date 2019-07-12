using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface ICustomerPaymentContractProcessor
    {
        Task<bool> ExistCategoryAsync(int Id, CancellationToken token = default(CancellationToken));
        Task<CustomerPaymentContract> SaveAsync(CustomerPaymentContract CustomerPayment, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<CustomerPaymentContract>> GetAsync(IEnumerable<int> ids, CancellationToken token = default(CancellationToken));
    }
}
