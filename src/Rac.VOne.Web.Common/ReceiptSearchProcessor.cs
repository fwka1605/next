using System;
using System.Collections.Generic;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class ReceiptSearchProcessor : IReceiptSearchProcessor
    {
        private readonly IReceiptSearchQueryProcessor receiptSearchQueryProcessor;


        public ReceiptSearchProcessor(
          IReceiptSearchQueryProcessor receiptSearchQueryProcessor
            )
        {
            this.receiptSearchQueryProcessor = receiptSearchQueryProcessor;
        }

        public async Task<IEnumerable<Receipt>> GetAsync(ReceiptSearch option, CancellationToken token = default(CancellationToken))
            => await receiptSearchQueryProcessor.GetAsync(option, token);

    }
}
