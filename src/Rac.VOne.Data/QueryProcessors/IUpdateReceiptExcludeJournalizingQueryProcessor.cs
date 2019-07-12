using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateReceiptExcludeJournalizingQueryProcessor
    {
        Task<int> UpdateAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<int> CancelAsync(JournalizingOption option, CancellationToken token = default(CancellationToken));
        Task<int> CancelDetailAsync(ReceiptExclude detail, CancellationToken token = default(CancellationToken));
    }
}
