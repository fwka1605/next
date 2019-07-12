using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IReceiptApportionByIdQueryProcessor
    {
        Task<IEnumerable<ReceiptApportion>> GetApportionItemsAsync(IEnumerable<long> headerIds, CancellationToken token = default(CancellationToken));
    }
}
