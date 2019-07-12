using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>決済代行ファイルフォーマット</summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentFileFormatController : ControllerBase
    {
        private readonly IPaymentFileFormatProcessor paymentFileFormatProcessor;
        /// <summary>constructor</summary>
        public PaymentFileFormatController(
            IPaymentFileFormatProcessor paymentFileFormatProcessor
            )
        {
            this.paymentFileFormatProcessor = paymentFileFormatProcessor;
        }

        /// <summary>取得</summary>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<PaymentFileFormat>>> Get(CancellationToken token)
            => (await paymentFileFormatProcessor.GetAsync(token)).ToArray();
    }
}
