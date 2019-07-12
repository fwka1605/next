using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System.Transactions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{

    public class PaymentAgencyMaster : IPaymentAgencyMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IPaymentAgencyProcessor paymentAgencyProcessor;
        private readonly IPaymentFileFormatProcessor paymentFileFormatProcessor;
        private readonly ILogger logger;

        public PaymentAgencyMaster(
            IAuthorizationProcessor authorizationProcessor,
            IPaymentAgencyProcessor paymentAgencyProcessor,
            IPaymentFileFormatProcessor paymentFileFormatProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.paymentAgencyProcessor = paymentAgencyProcessor;
            this.paymentFileFormatProcessor = paymentFileFormatProcessor;
            logger = logManager.GetLogger(typeof(PaymentAgencyMaster));
        }

        public async Task<PaymentAgenciesResult> GetItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await paymentAgencyProcessor.GetAsync(new MasterSearchOption { CompanyId = CompanyId, }, token)).ToList();
                return new PaymentAgenciesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentAgencies = result,
                };
            }, logger);
        }

        public async Task<PaymentAgenciesResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await paymentAgencyProcessor.GetAsync(new MasterSearchOption {
                    CompanyId   = CompanyId,
                    Codes       = Code,
                }, token) ).ToList();
                return new PaymentAgenciesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentAgencies = result,
                };
            }, logger);
        }

         public async Task<PaymentAgencyResult> SaveAsync(string SessionKey, PaymentAgency PaymentAgency)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await paymentAgencyProcessor.SaveAsync(PaymentAgency, token);
                return new PaymentAgencyResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentAgency = result,
                };

            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await paymentAgencyProcessor.DeleteAsync(Id, token);

                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<PaymentAgenciesResult> GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await paymentAgencyProcessor.GetAsync(new MasterSearchOption { Ids = Id, }, token)).ToList();
                return new PaymentAgenciesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentAgencies = result,
                };
            }, logger);
        }

        public async Task<PaymentFileFormatResult> GetFormatItemsAsync(string SessionKey)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await paymentFileFormatProcessor.GetAsync(token)).ToList();
                return new PaymentFileFormatResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PaymentFileFormats = result,
                };

            }, logger);
        }
    }
}
