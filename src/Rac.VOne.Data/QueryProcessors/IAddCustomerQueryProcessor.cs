using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddCustomerQueryProcessor
    {
        Task<Customer> SaveAsync(Customer Customer, bool requireIsParentUpdate = false, CancellationToken token = default(CancellationToken));

    }
}
