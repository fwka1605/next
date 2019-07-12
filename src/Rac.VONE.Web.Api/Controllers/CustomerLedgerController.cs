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
    ///  得意先消込台帳
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerLedgerController : ControllerBase
    {
        private readonly ICustomerLedgerProcessor customerLedgerProcessor;
        private readonly IHubContext<ProgressHub> hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public CustomerLedgerController(
            ICustomerLedgerProcessor customerLedgerProcessor,
            IHubContext<ProgressHub> hubContext
            )
        {
            this.customerLedgerProcessor = customerLedgerProcessor;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< CustomerLedger >>> GetR(CustomerLedgerSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await customerLedgerProcessor.GetAsync(option, token, notifier)).ToArray());

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerLedger>>> Get(CustomerLedgerSearch option, CancellationToken token)
            => (await customerLedgerProcessor.GetAsync(option, token, null)).ToArray();

    }
}
