using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    /// <summary>請求データ入力用 データ登録 interface</summary>
    public interface IBillingSaveForInputProcessor
    {
        /// <summary>請求データ入力用
        /// 明細削除時に連携されていないID のデータを削除する
        /// billings に連携されるすべての header 情報
        /// BilledAt, ClosingAt, DueAt, CustomerId, DepartmentId, StaffId, CollectCategoryId は同一の想定
        /// また、請求データ検索 → 請求データ入力 で 新規追加される行であっても、同一の InputId を設定して連携すること
        /// 詳細なビジネスロジックは Web Common 側で実装すること
        /// </summary>
        /// <param name="billings"></param>
        /// <returns></returns>
        Task<IEnumerable<Billing>> SaveAsync(IEnumerable<Billing> billings, CancellationToken token = default(CancellationToken));

    }
}
