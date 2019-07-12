using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// WebApiSetting 操作用コントロール
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebApiSettingController : ControllerBase
    {
        private readonly IWebApiSettingProcessor webApiSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="webApiSettingProcessor"></param>
        public WebApiSettingController(
            IWebApiSettingProcessor webApiSettingProcessor
            )
        {
            this.webApiSettingProcessor = webApiSettingProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> SaveAsync(WebApiSetting setting, CancellationToken token)
            => await webApiSettingProcessor.SaveAsync(setting, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="setting">会社ID、WebAPI のタイプ を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteAsync(WebApiSetting setting, CancellationToken token)
            => await webApiSettingProcessor.DeleteAsync(setting.CompanyId, setting.ApiTypeId, token);

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="setting">会社ID、WebAPI のタイプ を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<WebApiSetting>> GetByIdAsync(WebApiSetting setting, CancellationToken token)
            => await webApiSettingProcessor.GetByIdAsync(setting.CompanyId, setting.ApiTypeId, token);

    }
}
