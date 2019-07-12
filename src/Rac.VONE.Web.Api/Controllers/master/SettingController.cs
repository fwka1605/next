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
    /// 設定マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingProcessor settingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public SettingController(
            ISettingProcessor settingProcessor
            )
        {
            this.settingProcessor = settingProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Setting>>> GetItems([FromBody] IEnumerable<string> ItemIds, CancellationToken token)
            => (await settingProcessor.GetAsync(ItemIds, token)).ToArray();

    }
}
