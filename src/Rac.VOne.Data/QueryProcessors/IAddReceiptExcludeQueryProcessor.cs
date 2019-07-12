using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddReceiptExcludeQueryProcessor
    {
        //ReceiptExclude SaveReceiptExclude(long ReceiptId, decimal ExcludeAmount, int? ExcludeCategoryId, int UserId, DateTime? OutputAt);
        //ReceiptExclude ExcludeWithReceiptApportion(ReceiptApportion receiptApportion);

        Task<ReceiptExclude> SaveAsync(ReceiptExclude exclude, CancellationToken token = default(CancellationToken));
    }
}
