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
    public interface IReceiptHeaderProcessor
    {
        Task<IEnumerable<ReceiptHeader>> GetByIdsAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptHeader>> GetItemsAsync(int companyId, CancellationToken token = default(CancellationToken));
        Task<int> UpdateAsync(ReceiptHeaderUpdateOption option, CancellationToken token = default(CancellationToken));
    }
}
