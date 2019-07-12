using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IPaymentAgencyFeeProcessor
    {
        Task<IEnumerable<PaymentAgencyFee>> GetAsync(PaymentAgencyFeeSearch option, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<PaymentAgencyFee>> SaveAsync(IEnumerable<PaymentAgencyFee> fees, CancellationToken token = default(CancellationToken));
    }
}
