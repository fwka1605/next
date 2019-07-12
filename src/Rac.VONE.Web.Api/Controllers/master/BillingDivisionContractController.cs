using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  長期前受契約マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillingDivisionContractController : ControllerBase
    {
        private readonly IBillingDivisionContractProcessor billingDivisionContractProcessor;

        /// <summary>constructor</summary>
        public BillingDivisionContractController(
            IBillingDivisionContractProcessor billingDivisionContractProcessor
            )
        {
            this.billingDivisionContractProcessor = billingDivisionContractProcessor;
        }


        /// <summary>長期前受契約マスター取得</summary>
        /// <param name="option">会社ID、得意先ID 単一を指定する</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BillingDivisionContract>>> GetItems(BillingDivisionContractSearch option, CancellationToken token)
            => (await billingDivisionContractProcessor.GetAsync(option, token)).ToArray();

    }
}
