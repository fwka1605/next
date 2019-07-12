using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IPeriodicBillingSettingProcessor
    {
        Task<IEnumerable<PeriodicBillingSetting>> GetAsync(PeriodicBillingSettingSearch option, CancellationToken token = default(CancellationToken));

        /// <summary>登録処理 明細は delete insert</summary>
        /// <param name="setting"><see cref="PeriodicBillingSetting.Details"/>に明細を設定して連携すること</param>
        Task<PeriodicBillingSetting> SaveAsync(PeriodicBillingSetting setting, CancellationToken token = default(CancellationToken));

        /// <summary>マスター削除</summary>
        Task<int> DeleteAsync(long id, CancellationToken token  = default(CancellationToken));
    }
}
