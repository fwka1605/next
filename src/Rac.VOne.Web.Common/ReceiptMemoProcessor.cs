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
    public class ReceiptMemoProcessor: IReceiptMemoProcessor
    {
        private readonly IReceiptMemoQueryProcessor receiptMemoQueryProcessor;
        private readonly IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor;
        private readonly IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor;

        public ReceiptMemoProcessor(
            IReceiptMemoQueryProcessor receiptMemoQueryProcessor,
            IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor,
            IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor
           )
        {
            this.receiptMemoQueryProcessor = receiptMemoQueryProcessor;
            this.addReceiptMemoQueryProcessor = addReceiptMemoQueryProcessor;
            this.deleteReceiptMemoQueryProcessor = deleteReceiptMemoQueryProcessor;
        }

        public async Task<ReceiptMemo> GetAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
            => await receiptMemoQueryProcessor.GetAsync(ReceiptId, token);

        public async Task<IEnumerable<ReceiptMemo>> GetItemsAsync(IEnumerable<long> receiptIds, CancellationToken token = default(CancellationToken))
            => await receiptMemoQueryProcessor.GetItemsAsync(receiptIds, token);

        public async Task<ReceiptMemo> SaveAsync(long ReceiptId, string Memo, CancellationToken token = default(CancellationToken))
            => await addReceiptMemoQueryProcessor.SaveAsync(ReceiptId, Memo, token);

        public async Task<int> DeleteAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
            => await deleteReceiptMemoQueryProcessor.DeleteAsync(ReceiptId, token);




    }
}
