using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  銀行口座マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountProcessor bankAccountProcessor;
        private readonly IBankAccountFileImportProcessor bankAccountImportProcessor;

        /// <summary>constuctor</summary>
        public BankAccountController(
            IBankAccountProcessor bankAccountProcessor,
            IBankAccountFileImportProcessor bankAccountImportProcessor
            )
        {
            this.bankAccountProcessor = bankAccountProcessor;
            this.bankAccountImportProcessor = bankAccountImportProcessor;
        }

        /// <summary>
        /// 銀行口座マスター取得 複数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetItems(BankAccountSearch option, CancellationToken token)
            => (await bankAccountProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 銀行口座マスター保存
        /// </summary>
        /// <param name="account">銀行口座</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<BankAccount>> Save(BankAccount account, CancellationToken token)
            => await bankAccountProcessor.SaveAsync(account, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="account">銀行口座ID BankAccount.Id を指定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(BankAccount account, CancellationToken token)
            => await bankAccountProcessor.DeleteAsync(account.Id, token);

        /// <summary>
        /// 区分マスター 紐づき確認
        /// </summary>
        /// <param name="CategoryId">区分ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistCategory([FromBody] int CategoryId, CancellationToken token)
            => await bankAccountProcessor.ExistCategoryAsync(CategoryId, token);

        /// <summary>
        /// 入金部門 の紐づき確認
        /// </summary>
        /// <param name="SectionId">入金部門ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistSection([FromBody] int SectionId, CancellationToken token)
            => await bankAccountProcessor.ExistSectionAsync(SectionId, token);

        /// <summary>
        /// インポート処理
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await bankAccountImportProcessor.ImportAsync(source, token);

    }
}
