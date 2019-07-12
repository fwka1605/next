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
    /// 設定マスター
    /// </summary>
    public class SettingController : ApiControllerAuthorized
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
        public async Task<IEnumerable<Setting>> GetItems([FromBody] IEnumerable<string> ItemIds, CancellationToken token)
            => (await settingProcessor.GetAsync(ItemIds, token)).ToArray();

    }
}
