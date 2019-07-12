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
    /// 決済代行会社 マスター
    /// </summary>
    public class PaymentAgencyController : ApiControllerAuthorized
    {
        //private readonly IPaymentFileFormatProcessor paymentFileFormatProcessor;
        private readonly IPaymentAgencyProcessor paymentAgencyProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public PaymentAgencyController(
            IPaymentAgencyProcessor paymentAgencyProcessor
            //IPaymentFileFormatProcessor paymentFileFormat
            )
        {
            this.paymentAgencyProcessor = paymentAgencyProcessor;
            //paymentFileFormatProcessor = paymentFileFormat;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">会社ID, コード配列 あるいは ID 配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<PaymentAgency>> GetItems(MasterSearchOption option, CancellationToken token)
            => (await paymentAgencyProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="agency"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PaymentAgency> Save(PaymentAgency agency, CancellationToken token)
            => await paymentAgencyProcessor.SaveAsync(agency, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="agency">ID を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(PaymentAgency agency, CancellationToken token)
            => await paymentAgencyProcessor.DeleteAsync(agency.Id, token);



        ///// <summary>
        ///// TODO:適切な Contoller へ移動
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public PaymentFileFormatResult GetFormatItems()
        //    => paymentFileFormatProcessor.Get().ToArray();

    }
}
