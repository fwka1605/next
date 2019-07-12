using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeletePaymentAgencyFeeQueryProcessor
    {
        Task<int> DeleteAsync(int PaymentAgencyId, int CurrencyId, decimal Fee, CancellationToken token = default(CancellationToken));
    }
}
