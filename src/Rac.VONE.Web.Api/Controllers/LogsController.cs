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
    ///  ログ DB登録用
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsProcessor logsProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public LogsController(
            ILogsProcessor logsProcessor
            )
        {
            this.logsProcessor = logsProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="log"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> SaveErrorLog(Logs log, CancellationToken token)
            => await logsProcessor.SaveAsync(log, token);

    }
}
