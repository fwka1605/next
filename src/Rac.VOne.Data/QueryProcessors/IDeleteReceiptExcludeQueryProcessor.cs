using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteReceiptExcludeQueryProcessor
    {
        Task<int> DeleteAsync(long ReceiptId, CancellationToken token = default(CancellationToken));
        Task<int> DeleteByFileLogIdAsync(int Id, CancellationToken token = default(CancellationToken));
    }
}