using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class PaymentAgencyProcessor : IPaymentAgencyProcessor
    {
        private readonly IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> paymentAgencyGetByIdsQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<PaymentAgency> masterGetByCodesQueryProcessor;
        private readonly IAddPaymentAgencyQueryProcessor addPaymentAgencyQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<PaymentAgency> deletePaymentAgencyQueryProcessor;

        public PaymentAgencyProcessor(
            IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> paymentAgencyGetByIdsQueryProcessor,
            IMasterGetByCodesQueryProcessor<PaymentAgency> masterGetByCodesQueryProcessor,
            IAddPaymentAgencyQueryProcessor addPaymentAgencyQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<PaymentAgency> deletePaymentAgencyQueryProcessor
            )
        {
            this.paymentAgencyGetByIdsQueryProcessor = paymentAgencyGetByIdsQueryProcessor;
            this.masterGetByCodesQueryProcessor = masterGetByCodesQueryProcessor;
            this.addPaymentAgencyQueryProcessor = addPaymentAgencyQueryProcessor;
            this.deletePaymentAgencyQueryProcessor = deletePaymentAgencyQueryProcessor;
        }
        public async Task<IEnumerable<PaymentAgency>> GetAsync(MasterSearchOption option, CancellationToken token = default(CancellationToken))
        {
            if (option.Ids?.Any() ?? false) return await paymentAgencyGetByIdsQueryProcessor.GetByIdsAsync(option.Ids, token);
            return await masterGetByCodesQueryProcessor.GetByCodesAsync(option.CompanyId, option.Codes, token);
        }

        public async Task<PaymentAgency> SaveAsync(PaymentAgency paymentAgency, CancellationToken token = default(CancellationToken))
            => await addPaymentAgencyQueryProcessor.SaveAsync(paymentAgency, token);

        public async Task<int> DeleteAsync(int paymentAgencyId, CancellationToken token = default(CancellationToken))
            => await deletePaymentAgencyQueryProcessor.DeleteAsync(paymentAgencyId, token);

    }
}
