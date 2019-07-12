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
    /// マネーフォワードクラウド請求書 関連
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MFBillingController : ControllerBase
    {
        private readonly IMFBillingProcessor mfBillingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public MFBillingController(
            IMFBillingProcessor mfBillingProcessor
            )
        {
            this.mfBillingProcessor = mfBillingProcessor;
        }

        /// <summary>
        /// 登録 ※パラメータ
        /// </summary>
        /// <param name="source">Billings, Customers を設定 </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Billing>>> Save(MFBillingSource source, CancellationToken token)
            => (await mfBillingProcessor.SaveAsync(source, token)).ToArray();

        /// <summary>MFC請求書 連携用の <see cref="MFBilling"/> を取得
        /// 入金ステータス変更 MFC請求書の id を取得するため
        /// </summary>
        /// <param name="source">
        /// CompanyId を指定 あるいは
        /// Ids, IsMatched true : 消込済 / false : 未消込 を指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< MFBilling >>> Get(MFBillingSource source, CancellationToken token)
            => (await mfBillingProcessor.GetAsync(source, token)).ToArray();
    }
}
