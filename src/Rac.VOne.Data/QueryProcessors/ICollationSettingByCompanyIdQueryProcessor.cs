using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICollationSettingByCompanyIdQueryProcessor
    {

        Task<IEnumerable<MatchingOrder>> GetMatchingBillingOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MatchingOrder>> GetMatchingReceiptOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken));
    }
}
