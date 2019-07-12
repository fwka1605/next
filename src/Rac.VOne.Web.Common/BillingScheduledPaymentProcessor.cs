using System;
using System.Collections.Generic;
using System.Text;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Rac.VOne.Web.Common
{
    public class BillingScheduledPaymentProcessor : IBillingScheduledPaymentProcessor
    {
        private readonly IBillingQueryProcessor billingQueryProcessor;
        private readonly IUpdateBillingQueryProcessor updatebillingQueryProcessor;
        private readonly IImporterSettingDetailProcessor importerSettingDetailProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;


        public BillingScheduledPaymentProcessor(
            IBillingQueryProcessor billingQueryProcessor,
            IUpdateBillingQueryProcessor updatebillingQueryProcessor,
            IImporterSettingDetailProcessor importerSettingDetailProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingQueryProcessor = billingQueryProcessor;
            this.updatebillingQueryProcessor = updatebillingQueryProcessor;
            this.importerSettingDetailProcessor = importerSettingDetailProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        /// <summary>
        /// 振分け予定額が比較対象より値が大きいときtrueを返す。振分け予定額が負の場合は比較対象より値が小さいときtrueを返す。
        /// </summary>
        /// <param name="dueAmount"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsGreaterThan(decimal dueAmount, decimal target) => dueAmount >= 0 ? dueAmount > target : dueAmount < target;

        public async Task<IEnumerable<Billing>> SaveAsync(IEnumerable<Billing> billings, CancellationToken token = default(CancellationToken))
        {
            var result = new List<Billing>();
            var dueAtUpdateTargets = new Dictionary<long, Billing>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var billing in billings)
                {
                    if (billing.BillingInputId.HasValue && !dueAtUpdateTargets.ContainsKey(billing.BillingInputId.Value))
                        dueAtUpdateTargets.Add(billing.BillingInputId.Value, billing);
                    var updateItem = new Billing {
                        Id                      = billing.Id,
                        CompanyId               = billing.CompanyId,
                        DueAt                   = billing.DueAt,
                        OffsetAmount            = billing.OffsetAmount,
                        ScheduledPaymentKey     = billing.ScheduledPaymentKey,
                        UpdateBy                = billing.UpdateBy,
                    };
                    result.Add(await updatebillingQueryProcessor.UpdateScheduledPaymentAsync(updateItem, token));
                }
                foreach (var billing in dueAtUpdateTargets.Values)
                {
                    var updates = (await updatebillingQueryProcessor.UpdateScheduledPaymentForDueAtAsync(billing, token))
                        .Where(x => !result.Any(y => y.Id == x.Id)).ToArray();
                    result.AddRange(updates);
                }
                scope.Complete();
            }
            return result;
        }

        public async Task<IEnumerable<ScheduledPaymentImport>> ImportAsync(BillingScheduledPaymentImportSource source, CancellationToken token = default(CancellationToken))
        {
            var targetBillingIds = new long[] { };
            var targetBillingCustomerIds = new int[] { };
            var result = new List<ScheduledPaymentImport>();

            using (var scope = transactionScopeBuilder.Create())
            {

                if (source.Details == null) source.Details = (await importerSettingDetailProcessor.GetAsync(new ImporterSetting
                {
                    Id = source.ImporterSettingId,
                }, token)).ToArray();

                foreach (var item in source.Items)
                {

                    //取込キーに合致する請求データ一覧を取得する
                    var billings = (await billingQueryProcessor.GetDataForScheduledPaymentAsync(item, source.Details, token)).ToArray();

                    var checkedAmount = item.AssignmentAmount;
                    var resultCount = 0;
                    var count = 0;

                    if (item.CustomerFlg == 0)
                    {
                        targetBillingIds = targetBillingIds.Concat(billings.Select(x => x.Id)).ToArray();
                        targetBillingCustomerIds = targetBillingCustomerIds.Concat(billings.Select(x => x.ParentCustomerId != 0 ? x.ParentCustomerId : x.CustomerId)).ToArray();
                    }

                    //予定額の更新データの作成
                    foreach (var billing in billings)
                    {
                        count++;
                        billing.ScheduledPaymentKey = item.ScheduledPaymentKey;
                        var lastBill = (count == billings.Count());

                        //処理方法:更新 の場合
                        if (item.UpdateFlg == 1)
                        {
                            if (checkedAmount == 0)
                            {
                                billing.OffsetAmount = billing.RemainAmount;
                            }
                            else if (!lastBill
                                 && IsGreaterThan(checkedAmount, billing.RemainAmount))
                            {
                                checkedAmount -= billing.RemainAmount;
                                billing.OffsetAmount = 0;
                            }
                            else
                            {
                                billing.OffsetAmount = billing.RemainAmount - checkedAmount;
                                checkedAmount = 0;
                            }

                            resultCount = await updatebillingQueryProcessor.UpdateForScheduledPaymentAsync(billing, token);
                            continue;
                        }

                        //処理方法:加算 の場合
                        // 請求残と違算の符号が反転している or 違算 0 は振分け不可
                        var disable = (Math.Sign(billing.RemainAmount) * Math.Sign(billing.OffsetAmount) == -1
                            || billing.OffsetAmount == 0);
                        if (checkedAmount == 0
                           || (!lastBill && disable))
                        {
                            resultCount = await updatebillingQueryProcessor.UpdateForScheduledPaymentAsync(billing, token);
                            continue;
                        }

                        if (!lastBill
                           && IsGreaterThan(checkedAmount, billing.OffsetAmount))
                        {
                            checkedAmount -= billing.OffsetAmount;
                            billing.OffsetAmount = 0;
                        }
                        else
                        {
                            billing.OffsetAmount -= checkedAmount;
                            checkedAmount = 0;
                        }

                        resultCount = await updatebillingQueryProcessor.UpdateForScheduledPaymentAsync(billing, token);
                    }
                }

                if (source.Items.First().CustomerFlg == 0)
                {
                    await updatebillingQueryProcessor.UpdateForScheduledPaymentSameCustomersAsync(source.CompanyId,
                        source.LoginUserId,
                        source.Items.First().AssignmentFlag,
                        targetBillingIds.Distinct().ToArray(),
                        targetBillingCustomerIds.Distinct().ToArray(), token);
                }
                scope.Complete();
            }

            return result;
        }

        public async Task<IEnumerable<Billing>> GetAsync(BillingScheduledPaymentImportSource source, CancellationToken token = default(CancellationToken))
        {
            var result = new Dictionary<long, Billing>();

            if (source.Details == null) source.Details = (await importerSettingDetailProcessor.GetAsync(
                new ImporterSetting { Id = source.ImporterSettingId }, token)).ToArray();
 
            foreach (var item in source.Items)
            {
                var billings = (await billingQueryProcessor.GetItemsForScheduledPaymentImportAsync(item, source.Details, token)).ToArray();
                var billing = billings.FirstOrDefault(x
                    => item.AssignmentAmount > 0M && x.RemainAmount < 0M
                    || item.AssignmentAmount < 0M && x.RemainAmount > 0M) ?? billings.FirstOrDefault();

                if (!result.ContainsKey(billing.Id)) result.Add(billing.Id, billing);
            }
            return result.Values;
        }
    }
}
