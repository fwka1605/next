using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IBillingDiscountProcessor
    {
        Task<IEnumerable<BillingDiscount>> GetAsync(long BillingId, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(long BillingId, CancellationToken token = default(CancellationToken));
        /// <summary>
        /// 登録
        /// モデルが 特殊 最大5行登録できるが、web service に連携するオブジェクトは 単一
        /// プロパティに、1..5 までの歩引きの値を設定できる
        /// </summary>
        /// <param name="billingDiscount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> SaveAsync(BillingDiscount billingDiscount, CancellationToken token = default(CancellationToken));
    }
}
