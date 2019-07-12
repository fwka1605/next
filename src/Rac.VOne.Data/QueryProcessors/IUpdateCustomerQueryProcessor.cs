using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateCustomerQueryProcessor
    {
        Task<Customer> UpdateForBilingImportAsync(Customer updateCustmer, CancellationToken token = default(CancellationToken));

        Task<int> UpdateIsParentAsync(int isParent, int loginUserId, IEnumerable<int> ids, CancellationToken token = default(CancellationToken));

        Task<Customer> UpdateTransferNewCodeAsync(int id, int loginUserId, string transferNewCode, CancellationToken token = default(CancellationToken));
    }
}
