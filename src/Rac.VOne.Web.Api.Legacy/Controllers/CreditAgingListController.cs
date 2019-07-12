using Microsoft.AspNet.SignalR;
using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Api.Legacy.Hubs;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  債権総額管理表
    /// </summary>
    public class CreditAgingListController : ApiControllerAuthorized
    {
        private readonly ICreditAgingListProcessor creditAgingListProcessor;

        private readonly IHubContext hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public CreditAgingListController(
            ICreditAgingListProcessor creditAgingListProcessor
            )
        {
            this.creditAgingListProcessor = creditAgingListProcessor;

            this.hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< CreditAgingList >> GetR(CreditAgingListSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await creditAgingListProcessor.GetAsync(option, notifier, token)).ToArray());

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CreditAgingList>> Get(CreditAgingListSearch option, CancellationToken token)
            => (await creditAgingListProcessor.GetAsync(option, null, token)).ToArray();

    }
}
