using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 相殺
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NettingController : ControllerBase
    {
        private readonly INettingProcessor nettingProcessor;
        private readonly INettingSearchProcessor nettingSearchProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public NettingController(
           INettingProcessor nettingProcessor,
           INettingSearchProcessor nettingSearchProcessor
            )
        {
            this.nettingProcessor = nettingProcessor;
            this.nettingSearchProcessor = nettingSearchProcessor;
        }

        /// <summary>
        /// 区分ID の登録確認
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistReceiptCategory([FromBody] int categoryId, CancellationToken token)
            => await nettingProcessor.ExistReceiptCategoryAsync(categoryId, token);

        /// <summary>
        /// 得意先ID の登録確認
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistCustomer([FromBody] int customerId, CancellationToken token)
            => await nettingProcessor.ExistCustomerAsync(customerId, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="netting">会社ID、得意先ID、通貨IDを指定して問合せ</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Netting>>> GetItems(Netting netting, CancellationToken token)
            => (await nettingSearchProcessor.GetAsync(netting.CompanyId, netting.CustomerId, netting.CurrencyId, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="nettings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Netting>>> Save(IEnumerable<Netting> nettings, CancellationToken token)
            => (await nettingProcessor.SaveAsync(nettings, token)).ToArray();

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete([FromBody] IEnumerable<long> ids, CancellationToken token)
            => await nettingProcessor.DeleteAsync(ids, token);

        /// <summary>
        /// 入金部門ID 存在確認
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistSection([FromBody] int sectionId, CancellationToken token)
            => await nettingProcessor.ExistSectionAsync(sectionId, token);

        /// <summary>
        /// 通貨ID 存在確認
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistCurrency([FromBody] int currencyId, CancellationToken token)
            => await nettingProcessor.ExistCurrencyAsync(currencyId, token);

    }
}
