using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  長期前受契約マスター 設定
    /// </summary>
    public class BillingDivisionSettingController : ApiControllerAuthorized
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
        public async Task<BillingDivisionSetting> Get(BillingDivisionSetting setting, CancellationToken token)
            => await billingDivisionSettingProcessor.GetAsync(setting.CompanyId, token);
    }
}
