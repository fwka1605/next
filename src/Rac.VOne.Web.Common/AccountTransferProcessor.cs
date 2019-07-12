using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Common
{
    public class AccountTransferProcessor : IAccountTransferProcessor
    {
        private readonly IAccountTransferDetailQueryProcessor accountTransferDetailQueryProcessor;
        private readonly IAccountTransferLogQueryProcessor accountTransferLogQueryProcessor;
        private readonly IAddAccountTransferLogQueryProcessor addAccountTransferLogQueryProcessor;
        private readonly IAddAccountTransferDetailQueryProcessor addAccountTransferDetailQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<AccountTransferLog> deleteAccountTransferLogQueryProcessor;
        private readonly IDeleteAccountTransferDetailQueryProcessor deleteAccountTransferDetailQueryProcessor;
        private readonly IUpdateBillingAccountTransferLogQueryProcessor updateBillingAccountTransferLogQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<Currency> currencyGetByIdQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public AccountTransferProcessor(
            IAccountTransferDetailQueryProcessor accountTransferDetailQueryProcessor,
            IAccountTransferLogQueryProcessor accountTransferLogQueryProcessor,
            IAddAccountTransferLogQueryProcessor addAccountTransferLogQueryProcessor,
            IAddAccountTransferDetailQueryProcessor addAccountTransferDetailQueryProcessor,
            IDeleteTransactionQueryProcessor<AccountTransferLog> deleteAccountTransferLogQueryProcessor,
            IDeleteAccountTransferDetailQueryProcessor deleteAccountTransferDetailQueryProcessor,
            IUpdateBillingAccountTransferLogQueryProcessor updateBillingAccountTransferLogQueryProcessor,
            IMasterGetByCodesQueryProcessor<Currency> currencyGetByIdQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.accountTransferDetailQueryProcessor = accountTransferDetailQueryProcessor;
            this.accountTransferLogQueryProcessor = accountTransferLogQueryProcessor;
            this.addAccountTransferLogQueryProcessor = addAccountTransferLogQueryProcessor;
            this.addAccountTransferDetailQueryProcessor = addAccountTransferDetailQueryProcessor;
            this.deleteAccountTransferLogQueryProcessor = deleteAccountTransferLogQueryProcessor;
            this.deleteAccountTransferDetailQueryProcessor = deleteAccountTransferDetailQueryProcessor;
            this.updateBillingAccountTransferLogQueryProcessor = updateBillingAccountTransferLogQueryProcessor;
            this.currencyGetByIdQueryProcessor = currencyGetByIdQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        /// <summary>
        /// 出力履歴 削除（取消処理）
        /// 請求データの出力フラグクリア
        /// 出力明細削除
        /// 出力履歴削除
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<int> CancelAsync(IEnumerable<AccountTransferLog> logs, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                var loginUserId = logs.First().CreateBy;
                foreach (var log in logs)
                {
                    await updateBillingAccountTransferLogQueryProcessor.CancelAsync(log.Id, loginUserId, token);
                    await deleteAccountTransferDetailQueryProcessor.DeleteAsync(log.Id, token);
                    result += await deleteAccountTransferLogQueryProcessor.DeleteAsync(log.Id, token);
                }

                scope.Complete();
            }
            return result;
        }

        public async Task<IEnumerable<AccountTransferDetail>> ExtractAsync(AccountTransferSearch option, CancellationToken token = default(CancellationToken))
        {
            option.CurrencyId = (await currencyGetByIdQueryProcessor.GetByCodesAsync(option.CompanyId, new[] { DefaultCurrencyCode }, token)).First().Id;
            return await accountTransferDetailQueryProcessor.GetAsync(option, token);
        }


        public async Task<IEnumerable<AccountTransferLog>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await accountTransferLogQueryProcessor.GetAsync(CompanyId, token);


        public async Task<IEnumerable<AccountTransferDetail>> SaveAsync(IEnumerable<AccountTransferDetail> details, CancellationToken token = default(CancellationToken))
        {
            var result = new List<AccountTransferDetail>();

            var aggregate = details?.FirstOrDefault()?.Aggregate ?? false;

            using (var scope = transactionScopeBuilder.Create())
            {
                // 明細から ヘッダ情報取得し、登録
                var source = aggregate ? details.AggregateByKey() : details;
                var log = source.ConvertToLog(details.GetAccumulate());
                var logEntity = await addAccountTransferLogQueryProcessor.AddAsync(log, token);
                foreach (var detail in details)
                    detail.AccountTransferLogId = logEntity.Id;
                if (aggregate)
                    foreach (var detail in source)
                        detail.AccountTransferLogId = logEntity.Id;

                // 請求データのフラグ更新
                foreach (var detail in details)
                    await updateBillingAccountTransferLogQueryProcessor.UpdateAsync(detail, token);
                // 履歴データ作成
                foreach (var x in source)
                    result.Add( await addAccountTransferDetailQueryProcessor.AddAsync(x, token));

                scope.Complete();
            }
            return result;
        }

    }

}
