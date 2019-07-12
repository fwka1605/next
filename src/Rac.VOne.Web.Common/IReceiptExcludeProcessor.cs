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
    public interface IReceiptExcludeProcessor
    {
        Task<bool> ExistExcludeCategoryAsync(int ExcludeCategoryId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptExclude>> GetByReceiptIdAsync(long ReceiptId, CancellationToken token = default(CancellationToken));
        Task<ReceiptExcludeResult> SaveAsync(IEnumerable<ReceiptExclude> ReceipExclude, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptExclude>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken));


    }
}
