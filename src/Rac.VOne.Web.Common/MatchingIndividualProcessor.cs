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
    public class MatchingIndividualProcessor :
        IMatchingIndividualProcessor
    {
        private readonly IMatchingSaveProcessor matchingSaveProcessor;
        private readonly IMatchingSolveProcessor matchingSolveProcessor;

        private readonly IMatchingQueryProcessor matchingQueryProcessor;
        private readonly INettingQueryProcessor nettingQueryProcessor;
        private readonly IUpdateNettingQueryProcessor updateNettingQueryProcessor;
        private readonly IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor;
        private readonly IUpdateKanaHistoryCustomerQueryProcessor updateKanaHistoryCustomerQueryProcessor;
        private readonly IUpdateKanaHistoryPaymentAgencyQueryProcessor updateKanaHistoryPaymentAgencyQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Customer> customerGetByIdsQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> paymentAgencyGetByIdsQueryProcessor;
        private readonly IAddCustomerFeeQueryProcessor addCustomerFeeQueryProcessor;
        private readonly IAddPaymentAgencyFeeQueryProcessor addPaymentAgencyFeeQueryProcessor;
        private readonly IUpdateCustomerQueryProcessor updateCustomerQueryProcessor;
        private readonly IAddCustomerGroupQueryProcessor addCustomerGroupQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor;
        private readonly IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;


        public MatchingIndividualProcessor(
            IMatchingSaveProcessor matchingSaveProcessor,
            IMatchingSolveProcessor matchingSolveProcessor,
            IMatchingQueryProcessor matchingQueryProcessor,
            INettingQueryProcessor nettingQueryProcessor,
            IUpdateNettingQueryProcessor updateNettingQueryProcessor,
            IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor,
            IUpdateKanaHistoryCustomerQueryProcessor updateKanaHistoryCustomerQueryProcessor,
            IUpdateKanaHistoryPaymentAgencyQueryProcessor updateKanaHistoryPaymentAgencyQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<Customer> customerGetByIdsQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> paymentAgencyGetByIdsQueryProcessor,
            IAddCustomerFeeQueryProcessor addCustomerFeeQueryProcessor,
            IAddPaymentAgencyFeeQueryProcessor addPaymentAgencyFeeQueryProcessor,
            IUpdateCustomerQueryProcessor updateCustomerQueryProcessor,
            IAddCustomerGroupQueryProcessor addCustomerGroupQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor,
            IDbSystemDateTimeQueryProcessor dbSystemDateTimeQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.matchingSaveProcessor = matchingSaveProcessor;
            this.matchingSolveProcessor = matchingSolveProcessor;
            this.matchingQueryProcessor = matchingQueryProcessor;
            this.nettingQueryProcessor = nettingQueryProcessor;
            this.updateNettingQueryProcessor = updateNettingQueryProcessor;
            this.addReceiptMemoQueryProcessor = addReceiptMemoQueryProcessor;
            this.updateKanaHistoryCustomerQueryProcessor = updateKanaHistoryCustomerQueryProcessor;
            this.updateKanaHistoryPaymentAgencyQueryProcessor = updateKanaHistoryPaymentAgencyQueryProcessor;
            this.customerGetByIdsQueryProcessor = customerGetByIdsQueryProcessor;
            this.paymentAgencyGetByIdsQueryProcessor = paymentAgencyGetByIdsQueryProcessor;
            this.addCustomerFeeQueryProcessor = addCustomerFeeQueryProcessor;
            this.addPaymentAgencyFeeQueryProcessor = addPaymentAgencyFeeQueryProcessor;
            this.updateCustomerQueryProcessor = updateCustomerQueryProcessor;
            this.addCustomerGroupQueryProcessor = addCustomerGroupQueryProcessor;
            this.applicationControlQueryProcessor = applicationControlQueryProcessor;
            this.dbSystemDateTimeQueryProcessor = dbSystemDateTimeQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        /// <summary>
        /// 消込登録前の金額検証処理 消込処理実施時の残額とDBの残額の検証
        /// </summary>
        /// <param name="source"></param>
        /// <param name="setter"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> ValidateMatchingDataAsync(
            MatchingSource source,
            Action<MatchingResult> setter,
            CancellationToken token = default(CancellationToken))
        {
            var billingIds = new HashSet<long>();
            var receiptIds = new HashSet<long>();
            var nettingIds = new HashSet<long>();

            foreach (var matching in source.Matchings)
            {
                if (!billingIds.Contains(matching.BillingId)) billingIds.Add(matching.BillingId);
                if (!matching.IsNetting && !receiptIds.Contains(matching.ReceiptId)) receiptIds.Add(matching.ReceiptId);
                if (matching.IsNetting && !nettingIds.Contains(matching.ReceiptId)) nettingIds.Add(matching.ReceiptId);
            }

            var billing = await matchingQueryProcessor.GetBillingRemainAmountAsync(billingIds, token);
            var dbBillingTotal = billing.RemainAmount;
            var dbDiscountTotal = billing.DiscountAmount;
            if (dbBillingTotal != source.BillingRemainTotal)
            {
                setter?.Invoke(new MatchingResult
                {
                    MatchingErrorType = MatchingErrorType.BillingRemainChanged,
                });
                return false;
            }
            if (dbDiscountTotal != source.BillingDiscountTotal)
            {
                setter?.Invoke(new MatchingResult
                {
                    MatchingErrorType = MatchingErrorType.BillingDiscountChanged,
                });
                return false;
            }

            var dbReceiptTotal = 0M;
            var dbNettingTotal = 0M;
            if (receiptIds.Any())
            {
                dbReceiptTotal = await matchingQueryProcessor.GetReceiptRemainAmountAsync(receiptIds, token);
            }

            if (nettingIds.Any())
            {
                dbNettingTotal = await matchingQueryProcessor.GetNettingRemainAmountAsync(nettingIds, token);
            }

            dbReceiptTotal = dbReceiptTotal + dbNettingTotal;


            if (dbReceiptTotal != source.ReceiptRemainTotal)
            {
                setter?.Invoke(new MatchingResult
                {
                    MatchingErrorType = MatchingErrorType.ReceiptRemainChanged,
                });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 学習履歴登録
        /// </summary>
        /// <param name="Matching"></param>
        /// <param name="CompanyId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="PaymentAgencyId"></param>
        /// <param name="LoginUserId"></param>
        /// <param name="UpdateAt"></param>
        private async Task SaveKanaLearningAsync(IEnumerable<Matching> Matching,
            int CompanyId, int CustomerId, int PaymentAgencyId, int LoginUserId, DateTime UpdateAt,
            CancellationToken token = default(CancellationToken))
        {
            if (CustomerId != 0)
            {
                var kana = (await customerGetByIdsQueryProcessor.GetByIdsAsync(new[] { CustomerId }, token)).FirstOrDefault()?.Kana ?? "";
                var items = Matching.Where(x => !string.IsNullOrEmpty(x.PayerName) && x.PayerName != kana)
                    .GroupBy(x => new { x.PayerName, x.SourceBankName, x.SourceBranchName })
                    .Select(g => new KanaHistoryCustomer
                    {
                        CompanyId = CompanyId,
                        PayerName = g.Key.PayerName,
                        SourceBankName = g.Key.SourceBankName ?? string.Empty,
                        SourceBranchName = g.Key.SourceBranchName ?? string.Empty,
                        CustomerId = CustomerId,
                        HitCount = g.Count(),
                        CreateAt = UpdateAt,
                        UpdateAt = UpdateAt,
                        CreateBy = LoginUserId,
                        UpdateBy = LoginUserId,
                    });
                foreach (var item in items)
                    await updateKanaHistoryCustomerQueryProcessor.UpdateAsync(item, token);
            }
            if (PaymentAgencyId != 0)
            {
                var kana = (await paymentAgencyGetByIdsQueryProcessor.GetByIdsAsync(new[] { PaymentAgencyId }, token)).FirstOrDefault()?.Kana ?? "";
                var items = Matching.Where(x => !string.IsNullOrEmpty(x.PayerName) && x.PayerName != kana)
                    .GroupBy(x => new { x.PayerName, x.SourceBankName, x.SourceBranchName })
                    .Select(g => new KanaHistoryPaymentAgency
                    {
                        CompanyId = CompanyId,
                        PayerName = g.Key.PayerName,
                        SourceBankName = g.Key.SourceBankName ?? string.Empty,
                        SourceBranchName = g.Key.SourceBranchName ?? string.Empty,
                        PaymentAgencyId = PaymentAgencyId,
                        HitCount = g.Count(),
                        CreateAt = UpdateAt,
                        UpdateAt = UpdateAt,
                        CreateBy = LoginUserId,
                        UpdateBy = LoginUserId,
                    });
                foreach (var item in items)
                    await updateKanaHistoryPaymentAgencyQueryProcessor.UpdateAsync(item, token);
            }
        }

        /// <summary>
        ///  相殺データ準備 相殺データより 入金データを作成し、<see cref="Matching.ReceiptId"/>を修正する処理
        /// </summary>
        /// <param name="matchings"></param>
        /// <param name="loginUserId"></param>
        /// <param name="updateAt"></param>
        private async Task<List<Receipt>> PrepareNettingDataAsync(
            MatchingSource source, int loginUserId, DateTime updateAt,
            CancellationToken token = default(CancellationToken))
        {
            var matchings = source.Matchings;
            var nettingIds = matchings.Where(x => x.IsNetting).Select(x => x.ReceiptId).Distinct();
            var result = new List<Receipt>();
            foreach (var nettingId in nettingIds)
            {
                var netting = await nettingQueryProcessor.GetByIdAsync(nettingId, token);
                var receipt = netting.ConvertToReceiptInput(loginUserId, updateAt);
                var receiptResult = await matchingQueryProcessor.SaveMatchingReceiptAsync(receipt, token);
                result.Add(receiptResult);

                if (!string.IsNullOrEmpty(netting.ReceiptMemo))
                    await addReceiptMemoQueryProcessor.SaveAsync(receiptResult.Id, netting.ReceiptMemo, token);
                await updateNettingQueryProcessor.UpdateMatchingNettingAsync(netting.CompanyId, receiptResult.Id, netting.Id, CancelFlg: 0, token: token);
                // performance issue
                foreach (var matching in matchings.Where(x => x.IsNetting && x.ReceiptId == nettingId))
                    matching.ReceiptId = receiptResult.Id;
                foreach (var rcpt in source.Receipts.Where(x => x.NettingId == nettingId))
                    rcpt.Id = receiptResult.Id;
            }
            return result;
        }

        private async Task SaveBankTransferFeeAsync(int customerId, int paymentAgencyId,
            int currencyId, decimal bankFee, DateTime updateAt, int loginUserId,
            CancellationToken token = default(CancellationToken))
        {
            if (bankFee == 0M) return;
            if (customerId != 0)
            {
                var fee = new CustomerFee();
                fee.CustomerId = customerId;
                fee.CurrencyId = currencyId;
                fee.Fee = bankFee;
                fee.NewFee = bankFee;
                fee.UpdateAt = updateAt;
                fee.CreateAt = updateAt;
                fee.CreateBy = loginUserId;
                fee.UpdateBy = loginUserId;
                await addCustomerFeeQueryProcessor.SaveAsync(fee, token);
            }
            if (paymentAgencyId != 0)
            {
                var fee = new PaymentAgencyFee();
                fee.PaymentAgencyId = paymentAgencyId;
                fee.CurrencyId = currencyId;
                fee.Fee = bankFee;
                fee.NewFee = bankFee;
                fee.UpdateAt = updateAt;
                fee.CreateAt = updateAt;
                fee.CreateBy = loginUserId;
                fee.UpdateBy = loginUserId;
                await addPaymentAgencyFeeQueryProcessor.SaveAsync(fee, token);
            }
        }

        public async Task<MatchingResult> MatchAsync(
            MatchingSource source,
            CancellationToken token = default(CancellationToken),
            IProgressNotifier notifier = null)
        {
            var matchings = source.Matchings;
            var loginUserId = source.LoginUserId;
            var companyId = source.CompanyId;
            var customerId = source.CustomerId ?? 0;
            var paymentAgencyId = source.PaymentAgencyId ?? 0;
            var childCustomerIds = source.ChildCustomerIds;
            var useKanaLearning = source.UseKanaLearning == 1;
            var useFeeLearning = source.UseFeeLearning == 1;

            var appControl = await applicationControlQueryProcessor.GetAsync(companyId, token);
            var updateAt = await dbSystemDateTimeQueryProcessor.GetAsync(token);
            source.UpdateAt = updateAt;

            var useAuthorization = appControl?.UseAuthorization == 1;
            var nettingReceipts = new List<Receipt>();
            // DBサーバーから取得する

            var billings = new List<Billing>();
            var receipts = new List<Receipt>();
            var matchingBillingDiscounts = new List<MatchingBillingDiscount>();
            var billingScheduledIncomes = new List<BillingScheduledIncome>();
            var billingDiscounts = new HashSet<long>();

            MatchingResult validateResult = null;
            if (!(await ValidateMatchingDataAsync(source,
                x => validateResult = x,
                token)))
            {
                return validateResult;
            }

            var currencyId = source.Matchings.First().CurrencyId;


            using (var scope = transactionScopeBuilder.Create())
            {

                #region 相殺データ変換
                nettingReceipts = await PrepareNettingDataAsync(source, loginUserId, updateAt, token);
                foreach (var r in nettingReceipts)
                {
                    var item = source.Receipts.First(x => x.Id == r.Id);
                    item.UpdateAt = r.UpdateAt;
                }
                #endregion

                #region matchingHeader
                var header = source.MatchingHeader;
                header.MatchingProcessType = 1;
                header.Approved = useAuthorization ? 0 : 1;
                header.CreateBy = loginUserId;
                header.UpdateBy = loginUserId;
                header.CreateAt = updateAt;
                header.UpdateAt = updateAt;
                #endregion

                #region 手数料学習
                var bankFee = header.BankTransferFee;
                if (useFeeLearning)
                    await SaveBankTransferFeeAsync(customerId, paymentAgencyId, currencyId, bankFee, updateAt, loginUserId, token);
                #endregion

                #region 債権代表者登録
                if (childCustomerIds.Any())
                {
                    var isParent = 1;
                    await updateCustomerQueryProcessor.UpdateIsParentAsync(isParent, loginUserId, new[] { customerId }, token);
                    foreach (var childId in childCustomerIds)
                    {
                        var group = new CustomerGroup();
                        group.ParentCustomerId = customerId;
                        group.ChildCustomerId = childId;
                        group.CreateAt = updateAt;
                        group.CreateBy = loginUserId;
                        group.UpdateAt = updateAt;
                        group.UpdateBy = loginUserId;
                        await addCustomerGroupQueryProcessor.SaveAsync(group, token);
                    }
                }
                #endregion

                #region  学習履歴の登録
                if (useKanaLearning)
                    await SaveKanaLearningAsync(matchings, companyId, customerId, paymentAgencyId, loginUserId, updateAt);
                #endregion

                var matchingResult = await matchingSaveProcessor.SaveAsync(source, appControl, token);

                if (matchingResult.ProcessResult.Result)
                    scope.Complete();

                matchingResult.NettingReceipts = nettingReceipts;

                return matchingResult;
            }
        }

    }
}
