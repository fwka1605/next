using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IPaymentAgencyProcessor
    {
        Task<IEnumerable<PaymentAgency>> GetAsync(MasterSearchOption option, CancellationToken token = default(CancellationToken));
        Task<PaymentAgency> SaveAsync(PaymentAgency paymentAgency, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int paymentAgencyId, CancellationToken token = default(CancellationToken));
    }
}
