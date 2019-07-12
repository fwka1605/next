using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptExcludeQueryProcessor
    {
        Task<bool> ExistExcludeCategoryAsync(int ExcludeCategoryId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ReceiptExclude>> GetByReceiptIdAsync(long ReceiptId, CancellationToken token = default(CancellationToken));
    }
}
