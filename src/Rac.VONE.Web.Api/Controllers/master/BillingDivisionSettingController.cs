using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  長期前受契約マスター 設定
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillingDivisionSettingController : ControllerBase
    {
        private readonly IBillingDivisionSettingProcessor billingDivisionSettingProcessor;

        /// <summary>constructor</summary>
        public BillingDivisionSettingController(
            IBillingDivisionSettingProcessor billingDivisionSettingProcessor
            )
        {
            this.billingDivisionSettingProcessor = billingDivisionSettingProcessor;
        }

        /// <summary>長期前受契約マスター設定取得</summary>
        /// <param name="setting">CompanyId を設定した BillingDivisoinSetting</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<BillingDivisionSetting>> Get(BillingDivisionSetting setting, CancellationToken token)
            => await billingDivisionSettingProcessor.GetAsync(setting.CompanyId, token);
    }
}
