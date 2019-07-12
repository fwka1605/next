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
    ///  入力制御マスター （要否判定）
    /// </summary>
    public class InputControlController : ApiControllerAuthorized
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
        public async Task<IEnumerable<InputControl>> Get(InputControl control, CancellationToken token)
            => (await inputControlProcessor.GetAsync(control, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="controls">パラメータは これだけで良いかも</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<InputControl>> Save(
            IEnumerable<InputControl> controls, CancellationToken token)
            => (await inputControlProcessor.SaveAsync(controls, token)).ToArray();

    }
}
