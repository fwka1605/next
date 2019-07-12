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
    /// 決済代行会社手数料マスター
    /// </summary>
    public class PaymentAgencyFeeController : ApiControllerAuthorized
    {
        private IPaymentAgencyFeeProcessor paymentAgencyFeeProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public PaymentAgencyFeeController(
            IPaymentAgencyFeeProcessor paymentAgencyFeeProcessor
            )
        {
            this.paymentAgencyFeeProcessor = paymentAgencyFeeProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<PaymentAgencyFee>> GetItems(PaymentAgencyFeeSearch option, CancellationToken token)
            => (await paymentAgencyFeeProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="fees"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<PaymentAgencyFee>> Save(IEnumerable<PaymentAgencyFee> fees, CancellationToken token)
            => (await paymentAgencyFeeProcessor.SaveAsync(fees, token)).ToArray();

    }
}
