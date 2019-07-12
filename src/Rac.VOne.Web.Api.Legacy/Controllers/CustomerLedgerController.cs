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
    ///  得意先消込台帳
    /// </summary>
    public class CustomerLedgerController : ApiControllerAuthorized
    {
        private readonly ICustomerLedgerProcessor customerLedgerProcessor;

        private readonly IHubContext hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public CustomerLedgerController(
            ICustomerLedgerProcessor customerLedgerProcessor
            )
        {
            this.customerLedgerProcessor = customerLedgerProcessor;

            this.hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< CustomerLedger >> GetR(CustomerLedgerSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                => (await customerLedgerProcessor.GetAsync(option, token, notifier)).ToArray());

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CustomerLedger>> Get(CustomerLedgerSearch option, CancellationToken token)
            => (await customerLedgerProcessor.GetAsync(option, token, null)).ToArray();

    }
}
