using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class MfAggrTransactionProcessor : IMfAggrTransactionProcessor
    {
        private readonly IMfAggrTransactionQueryProcessor mfAggrTransactionQueryProcessor;
        private readonly IAddMfAggrTransactionQueryProcessor addMfAggrTransactionQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<MfAggrTransaction> deleteMfAggrTransactionQueryProcessor;

        private readonly IAddReceiptQueryProcessor addReceiptQueryProcessor;
        private readonly IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor;
        private readonly IMfAggrAccountProcessor mfAggrAccountProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MfAggrTransactionProcessor(
            IMfAggrTransactionQueryProcessor    mfAggrTransactionQueryProcessor,
            IAddMfAggrTransactionQueryProcessor addMfAggrTransactionQueryProcessor,
            IDeleteTransactionQueryProcessor<MfAggrTransaction> deleteMfAggrTransactionQueryProcessor,
            IAddReceiptQueryProcessor addReceiptQueryProcessor,
            IAddReceiptExcludeQueryProcessor addReceiptExcludeQueryProcessor,
            IMfAggrAccountProcessor mfAggrAccountProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.mfAggrTransactionQueryProcessor        = mfAggrTransactionQueryProcessor;
            this.addMfAggrTransactionQueryProcessor     = addMfAggrTransactionQueryProcessor;
            this.deleteMfAggrTransactionQueryProcessor  = deleteMfAggrTransactionQueryProcessor;
            this.addReceiptQueryProcessor               = addReceiptQueryProcessor;
            this.addReceiptExcludeQueryProcessor        = addReceiptExcludeQueryProcessor;
            this.mfAggrAccountProcessor                 = mfAggrAccountProcessor;
            this.transactionScopeBuilder                = transactionScopeBuilder;
        }

        public async Task<int> DeleteAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = 0;
                foreach (var id in ids)
                    result += await deleteMfAggrTransactionQueryProcessor.DeleteAsync(id, token);
                scope.Complete();
                return result;
            }
        }

        public Task<IEnumerable<MfAggrTransaction>> GetAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken))
            => mfAggrTransactionQueryProcessor.GetAsync(option, token);

        public Task<IEnumerable<long>> GetIdsAsync(CancellationToken token = default(CancellationToken))
            => mfAggrTransactionQueryProcessor.GetIdsAsync(token);

        public Task<IEnumerable<MfAggrTransaction>> GetLastOneAsync(MfAggrTransactionSearch option, CancellationToken token = default(CancellationToken))
            => mfAggrTransactionQueryProcessor.GetLastOneAsync(option, token);

        public async Task<int> SaveAsync(IEnumerable<MfAggrTransaction> transactions, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = 0;
                var accounts = (await mfAggrAccountProcessor.GetAsync(token)).ToArray();
                var accountDic = accounts.ToDictionary(x => x.Id);
                var subAccountDic = accounts.SelectMany(x => x.SubAccounts).ToDictionary(x => x.Id);

                MfAggrAccount getAccount(long id)
                    => accountDic.TryGetValue(id, out var account) ? account : null;

                MfAggrSubAccount getSubAccount(long id)
                    => subAccountDic.TryGetValue(id, out var subAccount) ? subAccount : null;

                foreach (var transaction in transactions)
                {
                    if (transaction.IsIncome)
                    {
                        var receipt = transaction.ConvertReceipt(getAccount, getSubAccount);
                        var saved = await addReceiptQueryProcessor.SaveAsync(receipt, token: token);
                        transaction.ReceiptId = saved.Id;
                        if (saved.ExcludeCategoryId.HasValue)
                        {
                            var exclude = new ReceiptExclude {
                                ReceiptId           = saved.Id,
                                ExcludeAmount       = saved.ExcludeAmount,
                                ExcludeCategoryId   = saved.ExcludeCategoryId,
                                //RecordedAt          = saved.RecordedAt,
                                CreateBy            = saved.CreateBy,
                                UpdateBy            = saved.CreateBy,
                            };
                            await addReceiptExcludeQueryProcessor.SaveAsync(exclude, token);
                        }

                    }
                    await addMfAggrTransactionQueryProcessor.AddAsync(transaction, token);
                    result++;
                }
                scope.Complete();
                return result;
            }
        }
    }
}
