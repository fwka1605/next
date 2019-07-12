using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface ICollationSettingProcessor
    {
        Task<CollationSetting> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MatchingOrder>> GetMatchingBillingOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<MatchingOrder>> GetMatchingReceiptOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<CollationOrder>> GetCollationOrderAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<CollationSetting> SaveAsync(CollationSetting CollationSetting, CancellationToken token = default(CancellationToken));
    }
}
