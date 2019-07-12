using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class ReceiptSectionTransferProcessor : IReceiptSectionTransferProcessor
    {
        private readonly IReceiptSectionTransferQueryProcessor receiptSectionTransferQueryProcessor;
        private readonly IDeleteReceiptSectionTransferQueryProcessor deleteReceiptSectionTransferQueryProcessor;
        private readonly IAddReceiptSectionTransferQueryProcessor addReceiptSectionTransferQueryProcessor;
        private readonly IUpdateReceiptSectionTransferQueryProcessor updateReceiptSectionTransferQueryProcessor;

        private readonly ITransactionalGetByIdQueryProcessor<Receipt> receiptGetByIdQueryProcessor;
        private readonly IAddReceiptQueryProcessor addReceiptQueryProcessor;
        private readonly IUpdateReceiptQueryProcessor updateReceiptQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<Receipt> deleteReceiptQueryProcessor;
        private readonly IReceiptMemoQueryProcessor receiptMemoQueryProcessor;
        private readonly IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor;
        private readonly IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor;
        private readonly IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReceiptSectionTransferProcessor(
            IReceiptSectionTransferQueryProcessor receiptSectionTransferQueryProcessor,
            IDeleteReceiptSectionTransferQueryProcessor deleteReceiptSectionTransferQueryProcessor,
            IAddReceiptSectionTransferQueryProcessor addReceiptSectionTransferQueryProcessor,
            IUpdateReceiptSectionTransferQueryProcessor updateReceiptSectionTransferQueryProcessor,
            ITransactionalGetByIdQueryProcessor<Receipt> receiptGetByIdQueryProcessor,
            IAddReceiptQueryProcessor addReceiptQueryProcessor,
            IUpdateReceiptQueryProcessor updateReceiptQueryProcessor,
            IDeleteTransactionQueryProcessor<Receipt> deleteReceiptQueryProcessor,
            IReceiptMemoQueryProcessor receiptMemoQueryProcessor,
            IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor,
            IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor,
            IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.receiptSectionTransferQueryProcessor = receiptSectionTransferQueryProcessor;
            this.deleteReceiptSectionTransferQueryProcessor = deleteReceiptSectionTransferQueryProcessor;
            this.addReceiptSectionTransferQueryProcessor = addReceiptSectionTransferQueryProcessor;
            this.updateReceiptSectionTransferQueryProcessor = updateReceiptSectionTransferQueryProcessor;
            this.receiptGetByIdQueryProcessor = receiptGetByIdQueryProcessor;
            this.addReceiptQueryProcessor = addReceiptQueryProcessor;
            this.updateReceiptQueryProcessor = updateReceiptQueryProcessor;
            this.deleteReceiptQueryProcessor = deleteReceiptQueryProcessor;
            this.receiptMemoQueryProcessor = receiptMemoQueryProcessor;
            this.addReceiptMemoQueryProcessor = addReceiptMemoQueryProcessor;
            this.deleteReceiptMemoQueryProcessor = deleteReceiptMemoQueryProcessor;
            this.dbSystemDateTimeQueryProcessor = dbSystemDateTimeQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        public async Task<IEnumerable<ReceiptSectionTransfer>> GetReceiptSectionTransferForPrintAsync(int CompanyId, CancellationToken token = default(CancellationToken))
             => await receiptSectionTransferQueryProcessor.GetReceiptSectionTransferForPrintAsync(CompanyId, token);

        public async Task<IEnumerable<ReceiptSectionTransfer>> UpdateReceiptSectionTransferPrintFlagAsync(int CompanyId, CancellationToken token = default(CancellationToken))
             => await updateReceiptSectionTransferQueryProcessor.UpdateReceiptSectionTransferPrintFlagAsync(CompanyId, token);

        /// <summary>
        /// 振替取消処理
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <remarks>
        /// source.Id -> destination.Id
        /// source.Id へ 入金ID の登録があるものは、振替元なので取消不可
        /// source, destination 同一の場合は取消対象
        /// destination.Id が合致する ReceiptSectionTransfer を取得し、取消処理を実施する
        /// 振替後の 入金データが 消込済 の場合は 取消不可
        /// /*取消不可となる条件の不足が気になる*/
        /// </remarks>
        private async Task<ReceiptSectionTransfersResult> CancelAsync(ReceiptSectionTransfer transfer, int loginUserId, CancellationToken token)
        {
            var result = new ReceiptSectionTransfersResult();
            var receipt = await receiptGetByIdQueryProcessor.GetByIdAsync(transfer.ReceiptId, token);
            if (receipt.AssignmentFlag != 0)
            {
                result.NotClearFlag = true; /* false */
                return result;
            }
            var receiptTransferSearch = new ReceiptSectionTransfer {
                SourceReceiptId = transfer.ReceiptId
            };
            var transferDB = (await receiptSectionTransferQueryProcessor.GetItemsAsync(receiptTransferSearch, token)).ToList();
            if (transferDB?.Any() ?? false)
            {
                result.TransferFlag = true; // property name was no sence.
                return result;
            }
            receiptTransferSearch = new ReceiptSectionTransfer {
                DestinationReceiptId = transfer.ReceiptId
            };
            transferDB = (await receiptSectionTransferQueryProcessor.GetItemsAsync(receiptTransferSearch, token)).ToList();
            if (transferDB == null)
            {
                return result;
            }
            var first = transferDB.Where(x => x.UpdateAt == transferDB.Max(t => t.UpdateAt)).First();
            await deleteReceiptSectionTransferQueryProcessor.DeleteAsync(first, token);

            if (first.DestinationReceiptId == first.SourceReceiptId)
            {
                var updateReceipt = await updateReceiptQueryProcessor.UpdateReceiptSectionAsync(first.SourceSectionId, loginUserId, first.SourceReceiptId, token);
                result.UpdateReceipts.Add(updateReceipt);
            }
            else
            {
                var updateReceipt = await updateReceiptQueryProcessor.UpdateReceiptAmountAsync(first.DestinationAmount, loginUserId, first.SourceReceiptId, token);
                result.UpdateReceipts.Add(updateReceipt);
                var deleteReceipt = await receiptGetByIdQueryProcessor.GetByIdAsync(transfer.ReceiptId, token);
                await deleteReceiptQueryProcessor.DeleteAsync(transfer.ReceiptId, token);
                result.DeleteReceipts.Add(deleteReceipt);
            }
            //result.ProcessResult.Result = true;
            return result;
        }

        /// <summary>
        /// 振替元 と 先 が同じ
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="loginUserId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<ReceiptSectionTransfersResult> TransferSameAmountAsync(ReceiptSectionTransfer transfer, int loginUserId, CancellationToken token)
        {
            var result = new ReceiptSectionTransfersResult();
            result.UpdateReceipts.Add(await updateReceiptQueryProcessor.UpdateReceiptSectionAsync(transfer.DestinationSectionId, loginUserId, transfer.ReceiptId, token));

            if (!string.IsNullOrWhiteSpace(transfer.TransferMemo))
                await addReceiptMemoQueryProcessor.SaveAsync(transfer.ReceiptId, transfer.TransferMemo, token);
            else
            {
                var receiptMemo = await receiptMemoQueryProcessor.GetAsync(transfer.ReceiptId, token);
                if (receiptMemo != null)
                    await deleteReceiptMemoQueryProcessor.DeleteAsync(transfer.ReceiptId, token);
            }
            transfer.DestinationReceiptId = transfer.ReceiptId;
            var transferDB = await receiptSectionTransferQueryProcessor.GetItemByReceiptIdAsync(transfer, token);
            if (transferDB == null)
            {
                var saveItem = new ReceiptSectionTransfer
                {
                    SourceReceiptId         = transfer.ReceiptId,
                    DestinationReceiptId    = transfer.ReceiptId,
                    SourceSectionId         = transfer.SourceSectionId,
                    DestinationSectionId    = transfer.DestinationSectionId,
                    SourceAmount            = transfer.SourceAmount,
                    DestinationAmount       = transfer.DestinationAmount,
                    UpdateBy                = loginUserId,
                    CreateBy                = loginUserId,
                    PrintFlag               = 0,
                };
                result.ReceiptSectionTransfers.Add( await addReceiptSectionTransferQueryProcessor.SaveAsync(saveItem, token));
            }
            else if (transferDB.SourceSectionId == transfer.DestinationSectionId)
            {
                await deleteReceiptSectionTransferQueryProcessor.DeleteAsync(transferDB, token);
            }
            else
            {
                await updateReceiptSectionTransferQueryProcessor.UpdateDestinationSectionAsync(transfer.DestinationSectionId, loginUserId, transferDB.SourceReceiptId, transferDB.DestinationReceiptId, token);
            }
            return result;
        }

        /// <summary>
        /// 一つの入金データを 複数に分割
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="loginUserId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<ReceiptSectionTransfersResult> DivideAsync(ReceiptSectionTransfer transfer, int loginUserId, CancellationToken token)
        {
            var result = new ReceiptSectionTransfersResult();
            var receipt = await receiptGetByIdQueryProcessor.GetByIdAsync( transfer.ReceiptId , token);
            if (receipt == null) return result;

            receipt.SectionId           = transfer.DestinationSectionId;
            receipt.ReceiptAmount       = transfer.DestinationAmount;
            receipt.RemainAmount        = transfer.DestinationAmount;
            receipt.AssignmentAmount    = 0;
            receipt.AssignmentFlag      = 0;
            receipt.OutputAt            = null;
            receipt.MailedAt            = null;
            receipt.OriginalReceiptId   = null;
            receipt.ExcludeFlag         = 0;
            receipt.ExcludeCategoryId   = null;
            receipt.ExcludeAmount       = 0;
            receipt.DeleteAt            = null;
            receipt.CreateBy            = loginUserId;
            receipt.UpdateBy            = loginUserId;

            var firstResult = await addReceiptQueryProcessor.SaveAsync(receipt, token: token);
            result.InsertReceipts.Add(firstResult);

            if (!string.IsNullOrWhiteSpace(transfer.TransferMemo))
                await addReceiptMemoQueryProcessor.SaveAsync(firstResult.Id, transfer.TransferMemo, token);

            receipt.SectionId           = transfer.SourceSectionId;
            receipt.ReceiptAmount       = transfer.SourceAmount - transfer.DestinationAmount;
            receipt.RemainAmount        = transfer.SourceAmount - transfer.DestinationAmount;

            var secondResult = await addReceiptQueryProcessor.SaveAsync(receipt, token: token);
            result.InsertReceipts.Add(secondResult);

            var receiptMemo = await receiptMemoQueryProcessor.GetAsync(transfer.ReceiptId, token);
            if (receiptMemo != null)
                await addReceiptMemoQueryProcessor.SaveAsync(secondResult.Id, receiptMemo.Memo, token);

            var updateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);

            result.UpdateReceipts.Add( await updateReceiptQueryProcessor.UpdateOriginalRemainAsync(transfer.ReceiptId, loginUserId, updateAt, token));

            var saveItem = new ReceiptSectionTransfer
            {
                SourceReceiptId             = transfer.ReceiptId,
                DestinationReceiptId        = firstResult.Id,
                SourceSectionId             = transfer.SourceSectionId,
                DestinationSectionId        = transfer.DestinationSectionId,
                SourceAmount                = transfer.SourceAmount,
                DestinationAmount           = transfer.DestinationAmount,
                PrintFlag                   = 0,
                CreateBy                    = loginUserId,
                UpdateBy                    = loginUserId
            };

            result.ReceiptSectionTransfers.Add( await addReceiptSectionTransferQueryProcessor.SaveAsync(saveItem, token));


            saveItem.DestinationReceiptId   = secondResult.Id;
            saveItem.DestinationSectionId   = transfer.SourceSectionId;
            saveItem.DestinationAmount      = transfer.SourceAmount - transfer.DestinationAmount;

            result.ReceiptSectionTransfers.Add( await addReceiptSectionTransferQueryProcessor.SaveAsync(saveItem, token));

            return result;
        }

        /// <summary>
        /// 振替 および 取消の実施
        /// ReceiptSectionTransfer および Receipt の操作を行う 取消不可の場合に、プロパティに値を設定して返す必要がある
        /// </summary>
        /// <param name="transfers"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ReceiptSectionTransfersResult> SaveAsync(IEnumerable<ReceiptSectionTransfer> transfers, CancellationToken token = default(CancellationToken))
        {
            var results = new ReceiptSectionTransfersResult();
            using (var scope = transactionScopeBuilder.Create())
            {
                var loginUserId = transfers.First().UpdateBy;
                foreach (var transfer in transfers)
                {
                    ReceiptSectionTransfersResult result = null;
                    if (transfer.CancelFlag)
                    {
                        result = await CancelAsync(transfer, loginUserId, token);
                        //if (!result.ProcessResult.Result)
                        //    return result;
                    }
                    else if (transfer.DestinationAmount == transfer.SourceAmount)
                    {
                        result = await TransferSameAmountAsync(transfer, loginUserId, token);
                    }
                    else
                    {
                        result = await DivideAsync(transfer, loginUserId, token);
                    }
                    results.InsertReceipts.AddRange(result.InsertReceipts);
                    results.UpdateReceipts.AddRange(result.UpdateReceipts);
                    results.DeleteReceipts.AddRange(result.DeleteReceipts);
                    results.ReceiptSectionTransfers.AddRange(result.ReceiptSectionTransfers);
                }
                //results.ProcessResult.Result = true;
                scope.Complete();
            }
            return results;
        }


    }
}
