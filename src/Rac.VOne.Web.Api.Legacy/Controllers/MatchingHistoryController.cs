using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Api.Legacy.Hubs;
using Microsoft.AspNet.SignalR;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// 消込履歴
    /// </summary>
    public class MatchingHistoryController : ApiControllerAuthorized
    {

        private readonly IMatchingHistorySearchProcessor matchingHistorySearchProcessor;
        private readonly IMatchingOutputedProcessor matchingOutputedProcessor;

        private readonly IHubContext hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public MatchingHistoryController(
            IMatchingHistorySearchProcessor matchingHistorySearchProcessor,
            IMatchingOutputedProcessor matchingOutputedProcessor
            )
        {
            this.matchingHistorySearchProcessor = matchingHistorySearchProcessor;
            this.matchingOutputedProcessor = matchingOutputedProcessor;

            this.hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< MatchingHistory >> GetR(MatchingHistorySearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await matchingHistorySearchProcessor.GetAsync(option, token, notifier)).ToArray());

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MatchingHistory>> Get(MatchingHistorySearch option, CancellationToken token)
            => (await matchingHistorySearchProcessor.GetAsync(option, token, null)).ToArray();

        /// <summary>
        /// 消込履歴 出力フラグ更新
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SaveOutputAtR([FromBody] long[] ids)
            => await hubContext.DoAsync(async token => {
                await matchingOutputedProcessor.SaveOutputAtAsync(ids, token);
                return true;
            });

        /// <summary>
        /// 消込履歴 出力フラグ更新
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SaveOutputAt([FromBody] long[] ids, CancellationToken token)
        {
            await matchingOutputedProcessor.SaveOutputAtAsync(ids, token);
            return true;
        }

    }
}
