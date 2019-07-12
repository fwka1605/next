using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IReceiptApportionProcessor
    {
        Task<IEnumerable<ReceiptApportion>> GetAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken));
        Task<ReceiptApportionsResult> ApportionAsync(IEnumerable<ReceiptApportion> apportions, CancellationToken token = default(CancellationToken));

    }
}
