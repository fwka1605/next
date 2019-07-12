using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using System.Linq;

namespace Rac.VOne.Web.Common
{
    public class AdvanceReceivedSplitProcessor : IAdvanceReceivedSplitProcessor
    {
        private readonly IReceiptQueryProcessor receiptQueryProcessor;
        private readonly IReceiptMemoQueryProcessor receiptMemoQueryProcessor;
        private readonly IAdvanceReceivedBackupQueryProcessor advanceReceivedBackupQueryProcessor;
        private readonly IAddReceiptQueryProcessor addReceiptQueryProcessor;
        private readonly IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor;
        private readonly ITransactionalGetByIdQueryProcessor<Receipt> receiptGetByIdQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<Receipt> deleteReceiptByIdQueryProcessor;
        private readonly IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor;
        private readonly ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public AdvanceReceivedSplitProcessor(
            IReceiptQueryProcessor receiptQueryProcessor,
            IReceiptMemoQueryProcessor receiptMemoQueryProcessor,
            IAdvanceReceivedBackupQueryProcessor advanceReceivedBackupQueryProcessor,
            IAddReceiptQueryProcessor addReceiptQueryProcessor,
            IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor,
            ITransactionalGetByIdQueryProcessor<Receipt> receiptGetByIdQueryProcessor,
            IDeleteTransactionQueryProcessor<Receipt> deleteReceiptByIdQueryProcessor,
            IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor,
            ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.receiptQueryProcessor = receiptQueryProcessor;
            this.receiptMemoQueryProcessor = receiptMemoQueryProcessor;
            this.advanceReceivedBackupQueryProcessor = advanceReceivedBackupQueryProcessor;
            this.addReceiptQueryProcessor = addReceiptQueryProcessor;
            this.addReceiptMemoQueryProcessor = addReceiptMemoQueryProcessor;
            this.receiptGetByIdQueryProcessor = receiptGetByIdQueryProcessor;
            this.deleteReceiptByIdQueryProcessor = deleteReceiptByIdQueryProcessor;
            this.deleteReceiptMemoQueryProcessor = deleteReceiptMemoQueryProcessor;
            this.categoryByCodeQueryProcessor = categoryByCodeQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        /// <summary>
        /// 前受振替（分割）処理
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <remarks>
        /// 前受入金 仕訳出力済 の場合に、入金データを 別テーブル AdvanceReceivedBackup へ退避
        /// 条件 前受振替処理の対象となる 入金データは、未消込のデータのみ
        /// 分割前、分割後で、ID が 変更されるので、 関連テーブル ReceiptMemo などは 削除する
        /// ReceiptExclude は、未消込なので、存在しない
        /// </remarks>
        public async Task<int> SplitAsync(AdvanceReceivedSplitSource source, CancellationToken token = default(CancellationToken))
        {
            var result = 0;

            using (var scope = transactionScopeBuilder.Create())
            {

                var advanceReceipts = (await receiptQueryProcessor.GetAdvanceReceiptsAsync(source.OriginalReceiptId, token))
                    .Where(ar => ar.AssignmentFlag == 0).ToArray();

                var receiptMemoDictionary = (await receiptMemoQueryProcessor.GetItemsAsync(advanceReceipts.Select(ar => ar.Id).ToArray(), token))
                    .ToDictionary(memo => memo.ReceiptId);

                var advanceReceivedBackup = await advanceReceivedBackupQueryProcessor.GetByOriginalReceiptIdAsync(source.OriginalReceiptId, token);
                if (advanceReceivedBackup == null
                    && advanceReceipts.Count() != 0 && advanceReceipts.First().OutputAt.HasValue)
                {
                    ReceiptMemo receiptMemo;

                    foreach (var receipt in advanceReceipts)
                    {
                        advanceReceivedBackup = receipt.ConvertToAdvanceReceivedBackup(id
                            => receiptMemoDictionary.TryGetValue(id, out receiptMemo) ? receiptMemo.Memo : "");
                        await advanceReceivedBackupQueryProcessor.SaveAsync(advanceReceivedBackup, token);
                    }
                }

                foreach (var memo in receiptMemoDictionary.Values)
                {
                    await deleteReceiptMemoQueryProcessor.DeleteAsync(memo.ReceiptId, token);
                }
                foreach (var receipt in advanceReceipts)
                {
                    await deleteReceiptByIdQueryProcessor.DeleteAsync(receipt.Id, token);
                }

                var originalReceipt = await receiptGetByIdQueryProcessor.GetByIdAsync(source.OriginalReceiptId, token);
                var advanceReceiptCategoryId = (await categoryByCodeQueryProcessor.GetAsync(new CategorySearch {
                    CompanyId       = source.CompanyId,
                    CategoryType    = Rac.VOne.Common.CategoryType.Receipt,
                    Codes           = new[] { "99" },
                }, token)).First().Id;

                foreach (var split in source.Items)
                {
                    var receipt = split.ConvertToReceipt(originalReceipt, advanceReceiptCategoryId, source.LoginUserId);
                    receipt = await addReceiptQueryProcessor.SaveAsync(receipt, token: token);
                    if (!string.IsNullOrEmpty(split.Memo))
                    {
                        await addReceiptMemoQueryProcessor.SaveAsync(receipt.Id, split.Memo, token);
                    }
                }

                result = source.Items.Length;

                scope.Complete();
            }
            return result;
        }

        /// <summary>
        /// 前受振替（分割） 取消
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<int> CancelAsync(AdvanceReceivedSplitSource source, CancellationToken token = default(CancellationToken))
        {
            var result = 0;

            using (var scope = transactionScopeBuilder.Create())
            {
                var advanceReceipts = (await receiptQueryProcessor.GetAdvanceReceiptsAsync(source.OriginalReceiptId, token))
                    .Where(ar => ar.AssignmentFlag == 0).ToArray();

                var receiptMemoDictionary = (await receiptMemoQueryProcessor.GetItemsAsync(advanceReceipts.Select(ar => ar.Id).ToArray(), token))
                    .ToDictionary(memo => memo.ReceiptId);

                foreach (var memo in receiptMemoDictionary.Values)
                {
                    await deleteReceiptMemoQueryProcessor.DeleteAsync(memo.ReceiptId, token);
                }
                foreach (var receipt in advanceReceipts)
                {
                    await deleteReceiptByIdQueryProcessor.DeleteAsync(receipt.Id, token);
                }

                var backup = await advanceReceivedBackupQueryProcessor.GetByOriginalReceiptIdAsync(source.OriginalReceiptId);
                if (backup == null)
                {
                    throw new InvalidOperationException($"振替取消処理: 書き戻すためのAdvanceReceivedBackupデータが見つかりません。OriginalReceiptId = {source.OriginalReceiptId}");
                }

                var restoringReceipt = backup.ConvertToReceipt(source.LoginUserId);

                var restoredReceipt = await addReceiptQueryProcessor.SaveAsync(restoringReceipt, specifyCreateAt: true, token: token);

                if (!string.IsNullOrEmpty(backup.Memo))
                {
                    await addReceiptMemoQueryProcessor.SaveAsync(restoredReceipt.Id, backup.Memo, token);
                }

                result = await advanceReceivedBackupQueryProcessor.DeleteAsync(source.OriginalReceiptId, token);

                scope.Complete();
            }

            return result;
        }

    }
}
