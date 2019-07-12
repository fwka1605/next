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
    public interface ICustomerFeeProcessor
    {
        Task<IEnumerable<CustomerFee>> GetAsync(CustomerFeeSearch option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<CustomerFee>> SaveAsync(IEnumerable<CustomerFee> fees, CancellationToken token = default(CancellationToken));

        Task<ImportResult> ImportAsync(
            IEnumerable<CustomerFee> insert,
            IEnumerable<CustomerFee> update,
            IEnumerable<CustomerFee> delete,
            CancellationToken token = default(CancellationToken));
    }
}
