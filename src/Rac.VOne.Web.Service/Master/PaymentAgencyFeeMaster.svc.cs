using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class PaymentAgencyFeeMaster : IPaymentAgencyFeeMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IPaymentAgencyFeeProcessor paymentAgencyFeeProcessor;
        private readonly ILogger logger;

        public PaymentAgencyFeeMaster(
            IAuthorizationProcessor authorizationProcessor,
            IPaymentAgencyFeeProcessor paymentAgencyFeeProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.paymentAgencyFeeProcessor = paymentAgencyFeeProcessor;
            logger = logManager.GetLogger(typeof(PaymentAgencyFeeMaster));
        }

        public async Task<PaymentAgencyFeesResult> GetAsync(string SessionKey, int PaymentAgencyId, int CurrencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await paymentAgencyFeeProcessor.GetAsync(new PaymentAgencyFeeSearch {
                    PaymentAgencyId = PaymentAgencyId,
                    CurrencyId      = CurrencyId,
                }, token)).ToList();
                return new PaymentAgencyFeesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentAgencyFees = result,
                };
            }, logger);
        }

        public async Task<PaymentAgencyFeesResult> GetForExportAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await paymentAgencyFeeProcessor.GetAsync(new PaymentAgencyFeeSearch { CompanyId = CompanyId, }, token)).ToList();
                return new PaymentAgencyFeesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentAgencyFees = result,
                };
            }, logger);
        }

        public async Task<PaymentAgencyFeesResult> SaveAsync(string SessionKey, int PaymentAgencyId, int CurrencyId, PaymentAgencyFee[] PaymentAgencyFee)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await paymentAgencyFeeProcessor.SaveAsync(PaymentAgencyFee, token)).ToList();
                return new PaymentAgencyFeesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentAgencyFees = result,
                };
            }, logger);
        }

    }
}
