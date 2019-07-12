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
    ///  
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyProcessor currencyProcess;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="currencyProcess"></param>
        public CurrencyController(
            ICurrencyProcessor currencyProcess
            )
        {
            this.currencyProcess = currencyProcess;
        }

        /// <summary>
        /// 配列取得
        /// </summary>
        /// <param name="option">通貨マスター検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Currency>>> GetItems(CurrencySearch option, CancellationToken token)
            => (await currencyProcess.GetAsync(option, token)).ToArray();


        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="currency">通貨</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Currency>> Save(Currency currency, CancellationToken token)
            => await currencyProcess.SaveAsync(currency, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="id">通貨ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete([FromBody] int id, CancellationToken token)
            => await currencyProcess.DeleteAsync(id, token);

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="importData"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(
            MasterImportData<Currency> importData,
            CancellationToken token = default(CancellationToken))
            => await currencyProcess.ImportAsync(importData.InsertItems, importData.UpdateItems, importData.DeleteItems, token);

        /// <summary>
        /// インポート時のチェック処理用 請求データ<see cref="Billing"/>で利用されている通貨一覧取得
        /// </summary>
        /// <param name="option">検索オプション 会社ID、通貨コード配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MasterData>>> GetImportItemsBilling(MasterSearchOption option, CancellationToken token)
            => (await currencyProcess.GetImportItemsBillingAsync(option.CompanyId, option.Codes)).ToArray();

        /// <summary>
        /// インポート時のチェック処理用 入金データ<see cref="Receipt"/>で利用されている通貨一覧取得
        /// </summary>
        /// <param name="option">検索オプション 会社ID、通貨コード配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MasterData>>> GetImportItemsReceipt(MasterSearchOption option, CancellationToken token)
            => (await currencyProcess.GetImportItemsReceiptAsync(option.CompanyId, option.Codes)).ToArray();

        /// <summary>
        /// インポート時のチェック処理用 相殺データ<see cref="Netting"/>で利用されている通貨一覧取得
        /// </summary>
        /// <param name="option">検索オプション 会社ID、通貨コード配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MasterData>>> GetImportItemsNetting(MasterSearchOption option, CancellationToken token)
            => (await currencyProcess.GetImportItemsNettingAsync(option.CompanyId, option.Codes)).ToArray();
    }
}
