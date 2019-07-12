using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddMatchingQueryProcessor
    {
        Task<MatchingHeader> SaveMatchingHeaderAsync(MatchingHeader MatchingHeader, CancellationToken token = default(CancellationToken));
        Task<Matching> SaveMatchingAsync(Matching matching, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 同時実行制御で、請求データ取得時のUpdateAtと更新後のUpdateAtを指定するため、引数の追加が必要
        /// </summary>
        /// <param name="Billing"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> UpdateBillingForMatchingAsync(Billing Billing, CancellationToken token = default(CancellationToken));
        /// <summary>
        /// 同時実行制御で、入金データ取得時のUpdateAtと更新後のUpdateAtを指定するため、引数の追加が必要
        /// </summary>
        /// <param name="Receipt"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> UpdateReceiptForMatchingAsync(Receipt Receipt, CancellationToken token = default(CancellationToken));

        Task<int> UpdateMatchingAsync(Matching Matching, CancellationToken token = default(CancellationToken));
        Task<Billing> SaveMatchingBillingAsync(Billing Billing, CancellationToken token = default(CancellationToken));

    }
}
