using Rac.VOne.Data;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Common
{
    public interface IPaymentFileFormatProcessor
    {
        Task<IEnumerable<PaymentFileFormat>> GetAsync(CancellationToken token = default(CancellationToken));
    }
}
