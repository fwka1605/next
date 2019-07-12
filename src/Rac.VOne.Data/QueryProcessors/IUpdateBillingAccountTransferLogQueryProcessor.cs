using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateBillingAccountTransferLogQueryProcessor
    {
        Task<Billing> UpdateAsync(AccountTransferDetail detail, CancellationToken token = default(CancellationToken));

        Task<int> CancelAsync(long AccountTransferLogId, int LoginUserId, CancellationToken token = default(CancellationToken));
    }
}
