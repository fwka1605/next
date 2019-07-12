using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICustomerPaymentContractQueryProcessor
    {
        Task<bool> ExistCategoryAsync(int Id, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<CustomerPaymentContract>> GetAsync(IEnumerable<int> ids, CancellationToken token = default(CancellationToken));
    }
}
