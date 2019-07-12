using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class AdvanceReceivedProcessor : IAdvanceReceivedProcessor
    {
        private readonly IReceiptQueryProcessor receiptQueryProcessor;
        private readonly IAddReceiptQueryProcessor addReceiptQueryProcessor;
        private readonly IUpdateReceiptQueryProcessor updateReceiptQueryProcessor;
        private readonly IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor;
        private readonly IDbFunctionProcessor dbFunctionProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public AdvanceReceivedProcessor(
            IReceiptQueryProcessor receiptQueryProcessor,
            IAddReceiptQueryProcessor addReceiptQueryProcessor,
            IUpdateReceiptQueryProcessor updateReceiptQueryProcessor,
            IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor,
            IDbFunctionProcessor dbFunctionProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.receiptQueryProcessor = receiptQueryProcessor;
            this.addReceiptQueryProcessor = addReceiptQueryProcessor;
            this.updateReceiptQueryProcessor = updateReceiptQueryProcessor;
            this.deleteReceiptQueryProcessor = deleteReceiptQueryProcessor;
            this.dbFunctionProcessor = dbFunctionProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Receipt>> GetAdvanceReceiptsAsync(long originalReceiptId, CancellationToken token = default(CancellationToken))
            => await receiptQueryProcessor.GetAdvanceReceiptsAsync(originalReceiptId, token);


        public async Task<AdvanceReceivedResult> SaveAsync(IEnumerable<AdvanceReceived> receiveds, CancellationToken token = default(CancellationToken))
        {
            var result = new AdvanceReceivedResult {
                ProcessResult           = new ProcessResult(),
                AdvancedReceiveItems    = new List<AdvanceReceived>(),
            };
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var item in receiveds)
                {
                    var updateAt = await dbFunctionProcessor.GetDbDateTimeAsync(token);
                    var x = await addReceiptQueryProcessor.AddAdvanceReceivedAsync(item.OriginalReceiptId, item.CustomerId, item.LoginUserId, item.OriginalUpdateAt, updateAt, token);
                    if (x == null)
                    {
                        result.ProcessResult.ErrorCode = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated;
                        return result;
                    }
                    result.AdvancedReceiveItems.Add(new AdvanceReceived {
                        ReceiptId           = x.Id,
                        OriginalReceiptId   = x.OriginalReceiptId.Value,
                        UpdateAt            = x.UpdateAt,
                        ReceiptCategoryId   = x.ReceiptCategoryId,
                        LoginUserId         = x.UpdateBy,
                    });
                    await updateReceiptQueryProcessor.UpdateOriginalRemainAsync(item.OriginalReceiptId, item.LoginUserId, updateAt, token);
                }
                result.ProcessResult.Result = true;

                scope.Complete();
            }

            return result;
        }

        public async Task<AdvanceReceivedResult> CancelAsync(IEnumerable<AdvanceReceived> receiveds, CancellationToken token = default(CancellationToken))
        {
            var result = new AdvanceReceivedResult {
                ProcessResult           = new ProcessResult(),
                AdvancedReceiveItems    = new List<AdvanceReceived>(),
            };
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var item in receiveds)
                {
                    var receipt = await updateReceiptQueryProcessor.UpdateCancelAdvancedReceivedAsync(item.ReceiptId, item.OriginalReceiptId, item.LoginUserId, item.OriginalUpdateAt, token);
                    if (receipt == null)
                    {
                        result.ProcessResult.ErrorCode = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated;
                        return result;
                    }

                    result.AdvancedReceiveItems.Add(new AdvanceReceived {
                        ReceiptId           = receipt.Id,
                        ReceiptCategoryId   = receipt.ReceiptCategoryId,
                        LoginUserId         = receipt.UpdateBy,
                        UpdateAt            = receipt.UpdateAt,
                    });
                    await deleteReceiptQueryProcessor.CancelAdvanceReceivedAsync(item.ReceiptId, token);
                }
                result.ProcessResult.Result = true;

                scope.Complete();
            }
            return result;
        }
    }
}
