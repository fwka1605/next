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
    /// 定期請求
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PeriodicBillingController : ControllerBase
    {
        private readonly IPeriodicBillingProcesssor periodicBillingProcesssor;

        /// <summary>
        /// constructor
        /// </summary>
        public PeriodicBillingController(
            IPeriodicBillingProcesssor periodicBillingProcesssor
            )
        {
            this.periodicBillingProcesssor = periodicBillingProcesssor;
        }

        /// <summary>
        /// 作成
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Billing>>> Create(IEnumerable<PeriodicBillingSetting> settings, CancellationToken token)
            => (await periodicBillingProcesssor.CreateAsync(settings, token)).ToArray();

    }
}
