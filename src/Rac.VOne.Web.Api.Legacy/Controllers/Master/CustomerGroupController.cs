using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  債権代表者マスター 得意先グループ
    /// </summary>
    public class CustomerGroupController : ApiControllerAuthorized
    {
        private readonly ICustomerGroupProcessor customerGroupProcessor;
        private readonly ICustomerGroupFileImportProcessor customerGroupImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public CustomerGroupController(
            ICustomerGroupProcessor customerGroupProcessor,
            ICustomerGroupFileImportProcessor customerGroupImportProcessor
            )
        {
            this.customerGroupProcessor = customerGroupProcessor;
            this.customerGroupImportProcessor = customerGroupImportProcessor;
        }


        /// <summary>
        /// 指定した会社ID に属する 債権代表者グループを取得
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CustomerGroup>> GetItems(CustomerGroupSearch option, CancellationToken token)
            => (await customerGroupProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 債権代表者 のグループの数を返す
        /// </summary>
        /// <param name="ids">対象となる得意先ID の配列 親・子共に参照する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> GetUniqueGroupCount([FromBody] IEnumerable<int> ids, CancellationToken token)
            => await customerGroupProcessor.GetUniqueGroupCountAsync(ids, token);

        /// <summary>
        /// 債権代表者グループの登録があるかどうか
        /// </summary>
        /// <param name="parentCustomerId">親得意先ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> HasChild([FromBody] int parentCustomerId, CancellationToken token)
            => await customerGroupProcessor.HasChildAsync(parentCustomerId, token);

        /// <summary>
        /// 指定した得意先ID が債権代表者グループに存在するか確認
        /// </summary>
        /// <param name="customerId">得意先ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCustomer([FromBody] int customerId, CancellationToken token)
            => await customerGroupProcessor.ExistCustomerAsync(customerId, token);

        /// <summary>
        /// 登録 新規登録/削除 も 一度に実施する
        /// </summary>
        /// <param name="importData">登録処理 用パラメータ
        /// 登録する項目は、<see cref="MasterImportData{TModel}.InsertItems"/>に、
        /// 削除する項目は、<see cref="MasterImportData{TModel}.DeleteItems"/> に 値を設定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CustomerGroup>> Save(MasterImportData<CustomerGroup> importData, CancellationToken token)
            => (await customerGroupProcessor.SaveAsync(importData, token)).ToArray();

        /// <summary>
        /// インポート処理
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await customerGroupImportProcessor.ImportAsync(source, token);

    }
}
