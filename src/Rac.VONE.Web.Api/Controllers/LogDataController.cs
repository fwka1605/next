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
    ///  操作ログ
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogDataController : ControllerBase
    {
        private readonly ILogDataProcessor logDataProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public LogDataController(
            ILogDataProcessor logDataProcessor
            )
        {
            this.logDataProcessor = logDataProcessor;
        }

        /// <summary>
        /// 配列 取得 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< LogData >>> GetItems(LogDataSearch option, CancellationToken token)
            => (await logDataProcessor.GetItemsAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="log"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Log(LogData log, CancellationToken token)
            => await logDataProcessor.LogAsync(log, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< LogData >> GetStats([FromBody] int companyId, CancellationToken token)
            => await logDataProcessor.GetStatsAsync(companyId, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteAll([FromBody] int companyId, CancellationToken token)
            => await logDataProcessor.DeleteAllAsync(companyId, token);

    }
}
