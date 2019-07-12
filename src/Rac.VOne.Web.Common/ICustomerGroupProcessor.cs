using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICustomerGroupProcessor
    {
        Task<IEnumerable<CustomerGroup>> GetAsync(CustomerGroupSearch option, CancellationToken token = default(CancellationToken));


        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));
        Task<bool> HasChildAsync(int ParentCustomerId, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<CustomerGroup>> SaveAsync(MasterImportData<CustomerGroup> items, CancellationToken token = default(CancellationToken));


        Task<int> GetUniqueGroupCountAsync(IEnumerable<int> Ids, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<CustomerGroup> insert,
            IEnumerable<CustomerGroup> update,
            IEnumerable<CustomerGroup> delete, CancellationToken token = default(CancellationToken));
    }
}
