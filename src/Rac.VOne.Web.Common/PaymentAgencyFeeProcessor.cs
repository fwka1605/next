using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class PaymentAgencyFeeProcessor :IPaymentAgencyFeeProcessor
    {
        private readonly IPaymentAgencyFeeQueryProcessor paymentAgencyFeeQueryProcessor;
        private readonly IAddPaymentAgencyFeeQueryProcessor addPaymentAgencyFeeQueryProcessor;
        private readonly IDeletePaymentAgencyFeeQueryProcessor deletePaymentAgencyFeeQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public PaymentAgencyFeeProcessor(
            IPaymentAgencyFeeQueryProcessor paymentAgencyFeeQueryProcessor,
            IAddPaymentAgencyFeeQueryProcessor addPaymentAgencyFeeQueryProcessor,
            IDeletePaymentAgencyFeeQueryProcessor deletePaymentAgencyFeeQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )

        {
            this.addPaymentAgencyFeeQueryProcessor = addPaymentAgencyFeeQueryProcessor;
            this.paymentAgencyFeeQueryProcessor = paymentAgencyFeeQueryProcessor;
            this.deletePaymentAgencyFeeQueryProcessor = deletePaymentAgencyFeeQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<PaymentAgencyFee>> GetAsync(PaymentAgencyFeeSearch option, CancellationToken token = default(CancellationToken))
            => await paymentAgencyFeeQueryProcessor.GetAsync(option, token);

        public async Task<IEnumerable<PaymentAgencyFee>> SaveAsync(IEnumerable<PaymentAgencyFee> fees, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<PaymentAgencyFee>();
                var fee1st = fees.First();
                var agencyId = fee1st.PaymentAgencyId;
                var currencyId = fee1st.CurrencyId;

                foreach (var fee in fees.Where(f => f.Fee != null && f.Fee != f.NewFee))
                    await deletePaymentAgencyFeeQueryProcessor.DeleteAsync(agencyId, currencyId, fee.Fee.Value, token);

                foreach (var fee in fees.Where(f => f.Fee != f.NewFee && f.NewFee != 0))
                {
                    fee.Fee = fee.NewFee.Value;
                    result.Add(await addPaymentAgencyFeeQueryProcessor.SaveAsync(fee, token));
                }
                scope.Complete();

                return result;
            }
        }
    }
}
