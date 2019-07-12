using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// WebApiSetting 操作用コントロール
    /// </summary>
    public class WebApiSettingController : ApiControllerAuthorized
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
        public async Task<int> SaveAsync(WebApiSetting setting, CancellationToken token)
            => await webApiSettingProcessor.SaveAsync(setting, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="setting">会社ID、WebAPI のタイプ を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteAsync(WebApiSetting setting, CancellationToken token)
            => await webApiSettingProcessor.DeleteAsync(setting.CompanyId, setting.ApiTypeId, token);

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="setting">会社ID、WebAPI のタイプ を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiSetting> GetByIdAsync(WebApiSetting setting, CancellationToken token)
            => await webApiSettingProcessor.GetByIdAsync(setting.CompanyId, setting.ApiTypeId, token);

    }
}
