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
    public class ReceiptHeaderProcessor : IReceiptHeaderProcessor
    {
        private readonly IReceiptHeaderQueryProcessor receiptHeaderQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<ReceiptHeader> receiptHeaderGetByIdsQueryProcessor;
        private readonly IUpdateReceiptHeaderQueryProcessor updateReceiptHeaderQueryProcessor;

        public ReceiptHeaderProcessor(
            IReceiptHeaderQueryProcessor receiptHeaderQueryProcessor,
            ITransactionalGetByIdsQueryProcessor<ReceiptHeader> receiptHeaderGetByIdsQueryProcessor,
            IUpdateReceiptHeaderQueryProcessor updateReceiptHeaderQueryProcessor
            )
        {
            this.receiptHeaderQueryProcessor = receiptHeaderQueryProcessor;
            this.receiptHeaderGetByIdsQueryProcessor = receiptHeaderGetByIdsQueryProcessor;
            this.updateReceiptHeaderQueryProcessor = updateReceiptHeaderQueryProcessor;
        }


        public Task<IEnumerable<ReceiptHeader>> GetItemsAsync(int companyId, CancellationToken token = default(CancellationToken))
            => receiptHeaderQueryProcessor.GetItemsAsync(companyId, token);

        public Task<IEnumerable<ReceiptHeader>> GetByIdsAsync(IEnumerable<long> Ids, CancellationToken token = default(CancellationToken))
            => receiptHeaderGetByIdsQueryProcessor.GetByIdsAsync(Ids, token);

        public Task<int> UpdateAsync(ReceiptHeaderUpdateOption option, CancellationToken token = default(CancellationToken))
            => updateReceiptHeaderQueryProcessor.UpdateAsync(option, token);


    }
}
