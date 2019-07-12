using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    /// <summary>請求データ入力用</summary>
    public interface IBillingSaveProcessor
    {
        ///// <summary>単一データ登録 </summary>
        //Billing Save(Billing Billing);
        Task<Billing> SaveAsync(Billing Billing, CancellationToken token = default(CancellationToken));

        /// <summary>複数件データ登録</summary>
        Task<IEnumerable<Billing>> SaveItemsAsync(IEnumerable<Billing> billings, CancellationToken token = default(CancellationToken));
    }
}
