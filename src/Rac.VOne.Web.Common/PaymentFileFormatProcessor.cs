using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class PaymentFileFormatProcessor : IPaymentFileFormatProcessor
    {

        private readonly IPaymentFileFormatQueryProcessor paymentFileFormatQueryProcessor;
        public PaymentFileFormatProcessor(
           IPaymentFileFormatQueryProcessor paymentFileFormatQueryProcessor)

        {
            this.paymentFileFormatQueryProcessor = paymentFileFormatQueryProcessor;

        }
        public async Task<IEnumerable<PaymentFileFormat>> GetAsync(CancellationToken token = default(CancellationToken))
            => await paymentFileFormatQueryProcessor.GetAsync(token);

    }
}
