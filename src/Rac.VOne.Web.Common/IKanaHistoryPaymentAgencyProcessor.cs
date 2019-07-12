using System.Collections.Generic;
using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IKanaHistoryPaymentAgencyProcessor
    {
        Task<IEnumerable<KanaHistoryPaymentAgency>> GetAsync(KanaHistorySearch option, CancellationToken token = default(CancellationToken));

        Task<int> DeleteAsync(KanaHistoryPaymentAgency history, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<KanaHistoryPaymentAgency> insert,
            IEnumerable<KanaHistoryPaymentAgency> update,
            IEnumerable<KanaHistoryPaymentAgency> delete, CancellationToken token = default(CancellationToken));
    }
}
