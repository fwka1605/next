using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateNettingQueryProcessor
    {
        Task<Netting> UpdateMatchingNettingAsync(int CompanyId, long ReceiptId, long Id, int CancelFlg, CancellationToken token = default(CancellationToken));

    }
}
