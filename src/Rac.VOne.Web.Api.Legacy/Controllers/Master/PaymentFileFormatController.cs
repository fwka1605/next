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
    /// <summary>決済代行ファイルフォーマット</summary>
    public class PaymentFileFormatController : ApiControllerAuthorized
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
        public async Task<IEnumerable<PaymentFileFormat>> Get(CancellationToken token)
            => (await paymentFileFormatProcessor.GetAsync(token)).ToArray();
    }
}
