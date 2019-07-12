using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IAccountTransferProcessor
    {
        Task<IEnumerable<AccountTransferLog>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<AccountTransferDetail>> ExtractAsync(AccountTransferSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<AccountTransferDetail>> SaveAsync(IEnumerable<AccountTransferDetail> details, CancellationToken token = default(CancellationToken));
        Task<int> CancelAsync(IEnumerable<AccountTransferLog> logs, CancellationToken token = default(CancellationToken));
    }
}
