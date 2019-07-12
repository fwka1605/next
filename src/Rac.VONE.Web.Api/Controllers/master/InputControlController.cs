using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  入力制御マスター （要否判定）
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InputControlController : ControllerBase
    {
        private readonly IInputControlProcessor inputControlProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public InputControlController(
            IInputControlProcessor inputControlProcessor
            )
        {
            this.inputControlProcessor = inputControlProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="control"></param>
        /// <param name="token">自動バインド</param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<InputControl>>> Get(InputControl control, CancellationToken token)
            => (await inputControlProcessor.GetAsync(control, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="controls">パラメータは これだけで良いかも</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<InputControl>>> Save(
            IEnumerable<InputControl> controls, CancellationToken token)
            => (await inputControlProcessor.SaveAsync(controls, token)).ToArray();

    }
}
