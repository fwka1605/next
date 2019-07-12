using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IBillingAccountTransferProcessor
    {
        Task<IEnumerable<Billing>> GetAsync(int companyId, int paymentAgencyId, DateTime transferDate, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> ImportAsync(IEnumerable<AccountTransferImportData> datas, CancellationToken token = default(CancellationToken));

    }
}
