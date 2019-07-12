using System.Collections.Generic;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICustomerGroupByIdQueryProcessor
    {
        Task<IEnumerable<CustomerGroup>> GetAsync(CustomerGroupSearch option, CancellationToken token = default(CancellationToken));
        /// <summary>
        /// 債権代表者/子得意先 何れかに 得意先ID の登録があるか確認する処理
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 子得意先を所持しているかどうか
        /// </summary>
        /// <param name="ParentCustomerId"></param>
        /// <returns></returns>
        Task<bool> HasChildAsync(int ParentCustomerId, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 請求データと入金データの得意先はそれぞれの債権代表者入子になっていないか確認する処理
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        Task<int> GetUniqueGroupCountAsync(IEnumerable<int> Ids, CancellationToken token = default(CancellationToken));
    }
}
