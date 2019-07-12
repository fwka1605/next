using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICollationQueryProcessor
    {
        ///// <summary>
        ///// 照合処理 初期化
        ///// 請求/入金データを キー情報で集計し、ワークへ登録
        ///// </summary>
        ///// <param name="CollationSearch"></param>
        ///// <returns></returns>
        Task<int> InitializeAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        ///// <summary>
        ///// 照合処理 得意先ID 債権代表者グループを考慮した、得意先IDでの照合処理
        ///// </summary>
        ///// <param name="CollationSearch"></param>
        ///// <returns></returns>
        Task<int> CollateCustomerIdAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        ///// <summary>
        ///// 照合処理 カナ学習履歴を利用した照合処理
        ///// </summary>
        ///// <param name="CollationSearch"></param>
        ///// <returns></returns>
        Task<int> CollateHistoryAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        ///// <summary>
        ///// 照合処理 専用口座（振込依頼人コード）を利用した照合処理
        ///// </summary>
        ///// <param name="CollationSearch"></param>
        ///// <returns></returns>
        Task<int> CollatePayerCodeAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        ///// <summary>
        ///// 照合処理 振込依頼人名と得意先カナを利用した照合処理
        ///// </summary>
        ///// <param name="CollationSearch"></param>
        ///// <returns></returns>
        Task<int> CollatePayerNameAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        ///// <summary>
        ///// 照合処理 照合番号 振込依頼人名 の数字 と、得意先マスター照合番号を利用した照合処理
        ///// </summary>
        ///// <param name="collationSearch"></param>
        ///// <returns></returns>
        Task<int> CollateKeyAsync(CollationSearch collationSearch, CancellationToken token = default(CancellationToken));

        ///// <summary>
        ///// 未照合データの登録
        ///// </summary>
        ///// <param name="CollationSearch"></param>
        ///// <returns></returns>
        Task<int> CollateMissingAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

        ///// <summary>
        ///// ワークから照合結果の取得
        ///// </summary>
        ///// <param name="CollationSearch"></param>
        ///// <returns></returns>
        Task<IEnumerable<Collation>> GetItemsAsync(CollationSearch CollationSearch, CancellationToken token = default(CancellationToken));

    }
}
