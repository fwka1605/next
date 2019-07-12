using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  締め処理設定
    ///  web common の実装が よろしくない
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClosingSettingController : ControllerBase
    {
        private readonly IClosingSettingProcessor closingSettingProcessor;

        /// <summary>constructor</summary>
        public ClosingSettingController(
            IClosingSettingProcessor closingSettingProcessor
            )
        {
            this.closingSettingProcessor = closingSettingProcessor;
        }

        /// <summary>締め処理設定の取得</summary>
        /// <param name="setting">会社IDを指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<ClosingSetting>> Get(ClosingSetting setting, CancellationToken token)
            => await closingSettingProcessor.GetAsync(setting.CompanyId, token);

        /// <summary>締め処理設定の登録</summary>
        /// <param name="setting">締め処理設定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<ClosingSetting>> Save(ClosingSetting setting, CancellationToken token)
            => await closingSettingProcessor.SaveAsync(setting, token);


    }
}
