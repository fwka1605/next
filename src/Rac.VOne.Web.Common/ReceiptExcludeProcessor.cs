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
    public class ReceiptExcludeProcessor :
        IReceiptExcludeProcessor
    {
        private readonly IReceiptExcludeQueryProcessor receiptExcludeQueryProcessor;
        private readonly IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor;
        private readonly IUpdateReceiptQueryProcessor updateReceiptQueryProcessor;
        private readonly IDeleteReceiptExcludeQueryProcessor deleteReceiptExcludeQueryProcessor;
        private readonly IUpdateReceiptHeaderQueryProcessor updateReceiptHeaderQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<ReceiptExclude> receiptExcludeGetByIdsQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReceiptExcludeProcessor(
           IReceiptExcludeQueryProcessor receiptExcludeQueryProcessor,
           IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor,
           IUpdateReceiptQueryProcessor updateReceiptQueryProcessor,
           IDeleteReceiptExcludeQueryProcessor deleteReceiptExcludeQueryProcessor,
           IUpdateReceiptHeaderQueryProcessor updateReceiptHeaderQueryProcessor,
           ITransactionalGetByIdsQueryProcessor<ReceiptExclude> receiptExcludeGetByIdsQueryProcessor,
           ITransactionScopeBuilder transactionScopeBuilder
           )
        {
            this.receiptExcludeQueryProcessor = receiptExcludeQueryProcessor;
            this.addReceiptExcludeQueryProcessor = addReceiptExcludeQueryProcessor;
            this.updateReceiptQueryProcessor = updateReceiptQueryProcessor;
            this.deleteReceiptExcludeQueryProcessor = deleteReceiptExcludeQueryProcessor;
            this.updateReceiptHeaderQueryProcessor = updateReceiptHeaderQueryProcessor;
            this.receiptExcludeGetByIdsQueryProcessor = receiptExcludeGetByIdsQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<bool> ExistExcludeCategoryAsync(int ExcludeCategoryId, CancellationToken token = default(CancellationToken))
            => await receiptExcludeQueryProcessor.ExistExcludeCategoryAsync(ExcludeCategoryId, token);

        public async Task<IEnumerable<ReceiptExclude>> GetByReceiptIdAsync(long Id, CancellationToken token = default(CancellationToken))
            => await receiptExcludeQueryProcessor.GetByReceiptIdAsync(Id, token);

        /// <summary>対象外 登録/戻し処理</summary>
        /// <param name="receiptExclude"><see cref="ReceiptExclude"/>の配列</param>
        /// <returns>
        /// 登録時、ReceiptExclude の削除後、新規登録
        /// 分割対象外対応
        /// ReceiptId で グループ化
        /// 最初に引き渡される ExcludeFlag によって、登録・削除処理が変わる
        /// 同時実行制御 で、エラーメッセージ表示のため、 ReceiptExcludeResult を返す
        /// </returns>
        public async Task<ReceiptExcludeResult> SaveAsync(IEnumerable<ReceiptExclude> excludes, CancellationToken token = default(CancellationToken))
        {
            var result = new ReceiptExcludeResult {
                ProcessResult       = new ProcessResult(),
                ReceiptExclude      = new ReceiptExclude[] { },
            };
            var saveResult = new List<ReceiptExclude>();

            var loginUserId = excludes.Select(c => c.UpdateBy).FirstOrDefault();

            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var group in excludes.GroupBy(x => x.ReceiptId))
                {
                    var firstExclude    = group.First();
                    var excludeFlag     = firstExclude.ExcludeFlag;
                    var categoryId      = firstExclude.ExcludeCategoryId;
                    var amount          = group.Sum(x => x.ExcludeAmount);
                    var updateAt        = firstExclude.ReceiptUpdateAt;

                    var updateItem      = new Receipt {
                        Id                  = group.Key,
                        ExcludeFlag         = excludeFlag,
                        ExcludeAmount       = amount,
                        ExcludeCategoryId   = categoryId,
                        UpdateBy            = loginUserId,
                        UpdateAt            = updateAt, /* Receipt.UpdateAt が変更されていると、更新件数が 0 になる */
                    };

                    var resultUpdateReceipt = 0;

                    if (excludeFlag == 1)
                    {
                        await deleteReceiptExcludeQueryProcessor.DeleteAsync(group.Key, token);
                        foreach (var item in group)
                            saveResult.Add( await addReceiptExcludeQueryProcessor.SaveAsync(item, token));
                        resultUpdateReceipt = await updateReceiptQueryProcessor.UpdateExcludeAmountAsync(updateItem, token);
                        await updateReceiptHeaderQueryProcessor.UpdateAsync(new ReceiptHeaderUpdateOption { ReceiptId = group.Key, UpdateBy = loginUserId }, token);
                    }
                    else /* (excludeFlag == 0) */
                    {
                        await deleteReceiptExcludeQueryProcessor.DeleteAsync(group.Key, token);
                        resultUpdateReceipt = await updateReceiptQueryProcessor.UpdateExcludeAmountAsync(updateItem, token);
                    }

                    if (resultUpdateReceipt == 0)
                    {
                        result.ProcessResult.ErrorCode = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated;
                        return result;
                    }
                }

                scope.Complete();

                result.ReceiptExclude = saveResult.ToArray();
                result.ProcessResult.Result = true;

            }
            return result;
        }

        // 入金データ入力画面・削除処理
        public async Task<int> DeleteAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
            => await deleteReceiptExcludeQueryProcessor.DeleteAsync(ReceiptId, token);

        public async Task<IEnumerable<ReceiptExclude>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken))
            => await receiptExcludeGetByIdsQueryProcessor.GetByIdsAsync(ids, token);
    }
}
