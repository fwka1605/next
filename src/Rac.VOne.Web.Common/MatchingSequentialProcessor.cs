using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class MatchingSequentialProcessor : IMatchingSequentialProcessor
    {
        #region web common interfaces
        private readonly IMatchingSaveProcessor matchingSaveProcessor;
        private readonly IMatchingSolveProcessor matchingSolveProcessor;
        #endregion
        private readonly IMatchingQueryProcessor matchingQueryProcessor;
        private readonly IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor;
        private readonly IUpdateNettingQueryProcessor updateNettingQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<MatchingOrder> matchingOrderQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor;
        private readonly IAddCustomerFeeQueryProcessor addCustomerFeeQueryProcessor;
        private readonly IAddPaymentAgencyFeeQueryProcessor addPaymentAgencyFeeQueryProcessor;
        private readonly IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MatchingSequentialProcessor(
            IMatchingSolveProcessor matchingSolveProcessor,
            IMatchingSaveProcessor matchingSaveProcessor,
            IMatchingQueryProcessor matchingQueryProcessor,
            IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor,
            IUpdateNettingQueryProcessor updateNettingQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<MatchingOrder> matchingOrderQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor,
            IAddCustomerFeeQueryProcessor addCustomerFeeQueryProcessor,
            IAddPaymentAgencyFeeQueryProcessor addPaymentAgencyFeeQueryProcessor,
            IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.matchingSolveProcessor = matchingSolveProcessor;
            this.matchingSaveProcessor = matchingSaveProcessor;
            this.matchingQueryProcessor = matchingQueryProcessor;
            this.addReceiptMemoQueryProcessor = addReceiptMemoQueryProcessor;
            this.updateNettingQueryProcessor = updateNettingQueryProcessor;
            this.matchingOrderQueryProcessor = matchingOrderQueryProcessor;
            this.applicationControlQueryProcessor = applicationControlQueryProcessor;
            this.addCustomerFeeQueryProcessor = addCustomerFeeQueryProcessor;
            this.addPaymentAgencyFeeQueryProcessor = addPaymentAgencyFeeQueryProcessor;
            this.dbSystemDateTimeQueryProcessor = dbSystemDateTimeQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        private async Task SaveBankTransferFeeAsync(Collation collation,
            CollationSearch option,
            DateTime updateAt,
            CancellationToken token = default(CancellationToken))
        {
            if (collation.CustomerId > 0)
            {
                var fee = new CustomerFee {
                    CustomerId  = collation.CustomerId,
                    CurrencyId  = collation.CurrencyId,
                    Fee         = collation.BankTransferFee,
                    NewFee      = collation.BankTransferFee,
                    UpdateAt    = updateAt,
                    CreateAt    = updateAt,
                    CreateBy    = option.LoginUserId,
                    UpdateBy    = option.LoginUserId,
                };
                await addCustomerFeeQueryProcessor.SaveAsync(fee, token);
            }
            if (collation.PaymentAgencyId > 0)
            {
                var fee = new PaymentAgencyFee {
                    PaymentAgencyId = collation.PaymentAgencyId,
                    CurrencyId      = collation.CurrencyId,
                    Fee             = collation.BankTransferFee,
                    NewFee          = collation.BankTransferFee,
                    UpdateAt        = updateAt,
                    CreateAt        = updateAt,
                    CreateBy        = option.LoginUserId,
                    UpdateBy        = option.LoginUserId,
                };
                await addPaymentAgencyFeeQueryProcessor.SaveAsync(fee, token);
            }

        }

        public async Task<MatchingResult> MatchAsync(
            IEnumerable<Collation> collations,
            CollationSearch option,
            CancellationToken token = default(CancellationToken),
            IProgressNotifier notifier = null)
        {
            var appControl = await applicationControlQueryProcessor.GetAsync(option.CompanyId, token);
            var nettingReceipts = new List<Receipt>();

            var updateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);
            var matchingOrders = await matchingOrderQueryProcessor.GetItemsAsync(option.CompanyId, token);
            var matchingBillingOrder = matchingOrders.Where(x => x.TransactionCategory == 1 && x.Available == 1).ToArray();
            var matchingReceiptOrder = matchingOrders.Where(x => x.TransactionCategory == 2 && x.Available == 1).ToArray();

            var matchings = new List<Matching>();
            var advanceReceiveds = new List<AdvanceReceived>();
            List<Billing> billings = null;
            List<Netting> nettings = null;
            List<Receipt> receipts = null;
            var index = 0;

            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var collation in collations)
                {
                    // 請求データ取得
                    var billingSearchOption = new MatchingBillingSearch
                    {
                        ClientKey = option.ClientKey,
                        ParentCustomerId = collation.CustomerId,
                        PaymentAgencyId = collation.PaymentAgencyId,
                        CurrencyId = collation.CurrencyId,
                    };

                    billings = (await matchingQueryProcessor.GetBillingsForSequentialMatchingAsync(billingSearchOption, matchingBillingOrder, token)).ToList();
                    var billingAmount = billings.Sum(item => (item.RemainAmount - item.DiscountAmount - item.OffsetAmount));
                    var billingCount = billings.Count;

                    // 請求データチェック
                    if (billingAmount != collation.BillingAmount
                        || billingCount != collation.BillingCount)
                    {
                        notifier?.Abort();
                        return new MatchingResult
                        {
                            MatchingErrorType = MatchingErrorType.BillingRemainChanged,
                            ErrorIndex = index,
                        };
                    }

                    // 相殺データ取得
                    nettings = (await matchingQueryProcessor.SearchMatchingNettingAsync(option, collation, token)).ToList();

                    // 相殺データ登録
                    var createdReceipstFromNetting = new List<Receipt>();
                    foreach (var netting in nettings)
                    {
                        var receipt = netting.ConvertToReceiptInput(option.LoginUserId, updateAt);
                        var nettingReceipt = await matchingQueryProcessor.SaveMatchingReceiptAsync(receipt, token);
                        if (!string.IsNullOrEmpty(netting.ReceiptMemo))
                            await addReceiptMemoQueryProcessor.SaveAsync(nettingReceipt.Id, netting.ReceiptMemo, token);
                        netting.ReceiptId = nettingReceipt.Id;
                        createdReceipstFromNetting.Add(nettingReceipt);
                    }

                    // 入金データ取得
                    var receiptSearch = new MatchingReceiptSearch
                    {
                        ClientKey = option.ClientKey,
                        CompanyId = option.CompanyId,
                        CurrencyId = collation.CurrencyId,
                        ParentCustomerId = collation.CustomerId,
                        PaymentAgencyId = collation.PaymentAgencyId,
                        UseScheduledPayment = appControl.UseScheduledPayment,
                    };

                    receipts = (await matchingQueryProcessor.GetReceiptsForSequentialMatchingAsync(receiptSearch, matchingReceiptOrder, token)).ToList();

                    var receiptAmount = receipts.Sum(item => (item.RemainAmount));
                    var receiptCount = receipts.Count();
                    var hasAdvanceReceived = receipts.Exists(item => item.UseAdvanceReceived == 1);

                    // 入金データチェック
                    if (receiptAmount != collation.ReceiptAmount
                        || receiptCount != collation.ReceiptCount)
                    {
                        notifier?.Abort();
                        return new MatchingResult
                        {
                            MatchingErrorType = MatchingErrorType.ReceiptRemainChanged,
                            ErrorIndex = index,
                        };
                    }

                    foreach (var netting in nettings)
                    {
                        await updateNettingQueryProcessor.UpdateMatchingNettingAsync(netting.CompanyId, netting.ReceiptId.Value, netting.Id, CancelFlg: 0, token: token);

                        foreach (var receipt in receipts
                        .Where(x => x.NettingId.HasValue && x.NettingId == netting.Id))
                            receipt.Id = netting.ReceiptId.Value;
                    }

                    // 前受処理日付取得
                    var recordedAt = option.GetRecordedAt(hasAdvanceReceived ? billings : null);
                    option.AdvanceReceivedRecordedAt = recordedAt;

                    var requestSource = new MatchingSource
                    {
                        Billings = billings,
                        Receipts = receipts,
                        BankTransferFee = collation.BankTransferFee,
                        TaxDifference = collation.TaxDifference,
                    };
                    var source = await matchingSolveProcessor.SolveAsync(requestSource, option, appControl, token);

                    foreach (var netting in nettings)
                    {
                        foreach (var matching in source.Matchings.Where(x => x.IsNetting && x.ReceiptId == netting.Id))
                            matching.ReceiptId = netting.ReceiptId.Value;
                    }

                    if (collation.UseFeeLearning == 1 && collation.BankTransferFee != 0M)
                        await SaveBankTransferFeeAsync(collation, option, updateAt, token);

                    // データの更新処理
                    int? customerId = collation.CustomerId;
                    if (customerId == 0)
                        customerId = null;

                    int? paymentAgencyId = collation.PaymentAgencyId;
                    if (paymentAgencyId == 0)
                        paymentAgencyId = null;

                    source.LoginUserId = option.LoginUserId;
                    source.UpdateAt = updateAt;
                    source.MatchingProcessType = 0;
                    source.CustomerId = customerId;
                    source.PaymentAgencyId = paymentAgencyId;
                    source.AdvanceReceivedCustomerId = option.DoTransferAdvanceReceived ? customerId : null;
                    source.ClientKey = option.ClientKey;

                    foreach (var r in createdReceipstFromNetting)
                    {
                        var item = receipts.First(x => x.Id == r.Id);
                        item.UpdateAt = r.UpdateAt;
                    }

                    var matchingResult = await matchingSaveProcessor.SaveAsync(source, appControl, token);
                    if (!matchingResult.ProcessResult.Result)
                    {
                        notifier?.Abort();
                        return matchingResult;
                    }
                    matchings.AddRange(matchingResult.Matchings);
                    advanceReceiveds.AddRange(matchingResult.AdvanceReceiveds);
                    nettingReceipts.AddRange(createdReceipstFromNetting);

                    notifier?.UpdateState();
                    index++;
                }

                scope.Complete();
            }


            return new MatchingResult
            {
                ProcessResult = new ProcessResult { Result = true },
                Matchings = matchings,
                AdvanceReceiveds = advanceReceiveds,
                NettingReceipts = nettingReceipts
            };
        }

    }
}
