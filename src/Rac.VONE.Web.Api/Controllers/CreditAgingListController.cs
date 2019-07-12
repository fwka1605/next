using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Api.Hubs;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  債権総額管理表
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CreditAgingListController : ControllerBase
    {
        private readonly ICreditAgingListProcessor creditAgingListProcessor;
        private readonly IHubContext<ProgressHub>  hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public CreditAgingListController(
            ICreditAgingListProcessor creditAgingListProcessor,
            IHubContext<ProgressHub> hubContext
            )
        {
            this.creditAgingListProcessor = creditAgingListProcessor;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< CreditAgingList >>> GetR(CreditAgingListSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await creditAgingListProcessor.GetAsync(option, notifier, token)).ToArray());

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CreditAgingList>>> Get(CreditAgingListSearch option, CancellationToken token)
            => (await creditAgingListProcessor.GetAsync(option, null, token)).ToArray();

    }
}
