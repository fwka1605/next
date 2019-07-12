using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  勘定科目マスター
    /// </summary>
    public class AccountTitleController : ApiControllerAuthorized
    {
        private readonly IAccountTitleProcessor accountTitleProcessor;
        private readonly IAccountTitleFileImportProcessor accountTitleImportProcessor;

        /// <summary>コンストラクタ</summary>
        public AccountTitleController(
            IAccountTitleProcessor accountTitleProcessor,
            IAccountTitleFileImportProcessor accountTitleImportProcessor
            )
        {
            this.accountTitleProcessor = accountTitleProcessor;
            this.accountTitleImportProcessor = accountTitleImportProcessor;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインドされる CancellationToken</param>
        /// <returns>AccountTitlesResult(ProcessResult：True/False、AccountTitles：AccountTitlesのリスト)</returns>
        [HttpPost]
        public async Task<IEnumerable<AccountTitle>> Get(AccountTitleSearch option, CancellationToken token)
            => (await accountTitleProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="accountTitle">保存するAccountTitle</param>
        /// <param name="token">自動バインド</param>
        /// <returns>AccountTitlesResult(ProcessResult：True/False、AccountTitle：保存したAccountTitle)</returns>
        [HttpPost]
        public async Task<AccountTitle> Save(AccountTitle accountTitle, CancellationToken token)
            => await accountTitleProcessor.SaveAsync(accountTitle, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="accountTitle">削除するAccountTitleのIDを設定したモデル</param>
        /// <param name="token">自動バインド</param>
        /// <returns>AccountTitlesResult(ProcessResult：True/False、Count：削除したAccountTitleの件数)</returns>
        [HttpPost]
        public async Task<int> Delete(AccountTitle accountTitle, CancellationToken token)
            => await accountTitleProcessor.DeleteAsync(accountTitle.Id, token);

        /// <summary>
        /// インポート処理
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await accountTitleImportProcessor.ImportAsync(source, token);

        /// <summary>
        /// 区分マスター上で利用されているデータを取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsForCategory(MasterSearchOption option, CancellationToken token)
            => (await accountTitleProcessor.GetImportItemsCategoryAsync(option.CompanyId, option.Codes, token)).ToArray();


        /// <summary>
        /// 歩引マスター上で利用されているデータを取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsForCustomerDiscount(MasterSearchOption option, CancellationToken token)
            => (await accountTitleProcessor.GetImportItemsCustomerDiscountAsync(option.CompanyId, option.Codes, token)).ToArray();

        /// <summary>
        /// 請求データ 借方科目で利用されているデータを取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsForDebitBilling(MasterSearchOption option, CancellationToken token)
            => (await accountTitleProcessor.GetImportItemsDebitBillingAsync(option.CompanyId, option.Codes, token)).ToArray();

        /// <summary>
        /// 請求データ 貸方科目で利用されているデータを取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsForCreditBilling(MasterSearchOption option, CancellationToken token)
            => (await accountTitleProcessor.GetImportItemsCreditBillingAsync(option.CompanyId, option.Codes, token)).ToArray();


    }
}
