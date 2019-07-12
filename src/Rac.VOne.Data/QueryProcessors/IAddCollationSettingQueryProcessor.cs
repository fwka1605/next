using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddCollationSettingQueryProcessor
    {
        Task<CollationSetting> SaveAsync(CollationSetting CollationSetting, CancellationToken token = default(CancellationToken));
        Task<CollationOrder> SaveCollationOrderAsync(CollationOrder CollationOrder, CancellationToken token = default(CancellationToken));
        Task<MatchingOrder> SaveMatchingOrderAsync(MatchingOrder MatchingOrder, CancellationToken token = default(CancellationToken));
    }
}
