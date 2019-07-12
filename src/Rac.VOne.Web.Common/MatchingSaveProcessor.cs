using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class MatchingSaveProcessor : IMatchingSaveProcessor
    {
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor;
        private readonly IMatchingQueryProcessor matchingQueryProcessor;
        private readonly IAddMatchingQueryProcessor addMatchingQueryProcessor;
        private readonly IAddReceiptQueryProcessor addReceiptQueryProcessor;
        private readonly IUpdateReceiptQueryProcessor updateReceiptQueryProcessor;
        private readonly IAddMatchingBillingDiscountQueryProcessor addMatchingBillingDiscountQueryProcessor;
        private readonly ITransactionalGetByIdQueryProcessor<Billing> billingGetByIdQueryProcessor;
        private readonly ITransactionalGetByIdQueryProcessor<Receipt> receiptGetByIdQueryProcessor;
        private readonly IAddBillingScheduledIncomeQueryProcessor addBillingScheduledIncomeQueryProcessor;
        private readonly IUpdateBillingDiscountQueryProcessor updateBillingDiscountQueryProcessor;

        public MatchingSaveProcessor(
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor,
            IMatchingQueryProcessor matchingQueryProcessor,
            IAddMatchingQueryProcessor addMatchingQueryProcessor,
            IAddReceiptQueryProcessor addReceiptQueryProcessor,
            IUpdateReceiptQueryProcessor updateReceiptQueryProcessor,
            IAddMatchingBillingDiscountQueryProcessor addMatchingBillingDiscountQueryProcessor,
            ITransactionalGetByIdQueryProcessor<Billing> billingGetByIdQueryProcessor,
            ITransactionalGetByIdQueryProcessor<Receipt> receiptGetByIdQueryProcessor,
            IAddBillingScheduledIncomeQueryProcessor addBillingScheduledIncomeQueryProcessor,
            IUpdateBillingDiscountQueryProcessor updateBillingDiscountQueryProcessor
            )
        {
            this.applicationControlQueryProcessor = applicationControlQueryProcessor;
            this.matchingQueryProcessor = matchingQueryProcessor;
            this.addMatchingQueryProcessor = addMatchingQueryProcessor;
            this.addReceiptQueryProcessor = addReceiptQueryProcessor;
            this.updateReceiptQueryProcessor = updateReceiptQueryProcessor;
            this.addMatchingBillingDiscountQueryProcessor = addMatchingBillingDiscountQueryProcessor;
            this.billingGetByIdQueryProcessor = billingGetByIdQueryProcessor;
            this.receiptGetByIdQueryProcessor = receiptGetByIdQueryProcessor;
            this.addBillingScheduledIncomeQueryProcessor = addBillingScheduledIncomeQueryProcessor;
            this.updateBillingDiscountQueryProcessor = updateBillingDiscountQueryProcessor;
        }

        public async Task<MatchingResult> SaveAsync(
            MatchingSource source,
            ApplicationControl applicationControl,
            CancellationToken token = default(CancellationToken))
        {
            var companyId = source.Billings.First().CompanyId;
            if (applicationControl == null)
                applicationControl = await applicationControlQueryProcessor.GetAsync(companyId, token);

            var billings = source.Billings;
            var receipts = source.Receipts;
            var matchings = source.Matchings;
            var header = source.MatchingHeader;
            var matchingBillingDiscounts = source.MatchingBillingDiscounts;
            var billingDiscounts = source.BillingDiscounts;
            var billingScheduledIncomes = source.BillingScheduledIncomes;
            var loginUserId = source.LoginUserId;
            var updateAt = source.UpdateAt;
            var matchingProcessType = source.MatchingProcessType;
            var customerId = source.CustomerId;
            var paymentAgencyId = source.PaymentAgencyId;
            var advanceReceivedCustomerId = source.AdvanceReceivedCustomerId;

            var matchingResults = new List<Matching>();
            var advanceReceivedResults = new List<AdvanceReceived>();
            var clientKey = source.ClientKey;

            foreach (var billing in billings)
            {
                billing.UpdateBy = loginUserId;
                billing.NewUpdateAt = updateAt;
                if (applicationControl.UseDeclaredAmount == 0)
                    billing.OffsetAmount = 0;
                var result = await addMatchingQueryProcessor.UpdateBillingForMatchingAsync(billing, token);
                if (result != 1)
                    return new MatchingResult { MatchingErrorType = MatchingErrorType.BillingRemainChanged };
            }

            foreach (var receipt in receipts)
            {
                receipt.UpdateBy = loginUserId;
                receipt.NewUpdateAt = updateAt;
                var result = await addMatchingQueryProcessor.UpdateReceiptForMatchingAsync(receipt, token);
                if (result != 1)
                    return new MatchingResult { MatchingErrorType = MatchingErrorType.ReceiptRemainChanged };
                if (receipt.ReceiptHeaderId.HasValue)
                {
                    var receiptHeaderId = receipt.ReceiptHeaderId.Value;
                    await matchingQueryProcessor.UpdateReceiptHeaderAsync(receiptHeaderId, loginUserId, updateAt, token);
                }
            }

            header.MatchingProcessType = matchingProcessType;
            header.CustomerId = customerId;
            header.PaymentAgencyId = paymentAgencyId;
            header.Memo = header.Memo ?? string.Empty;
            header.Approved = (applicationControl.UseAuthorization == 1) ? 0 : 1;
            header.CreateBy = loginUserId;
            header.UpdateBy = loginUserId;
            header.CreateAt = updateAt;
            header.UpdateAt = updateAt;

            var headerResult = await addMatchingQueryProcessor.SaveMatchingHeaderAsync(header, token);

            if (headerResult == null)
                return new MatchingResult { MatchingErrorType = MatchingErrorType.DBError };

            var advanceRecievedReciptId = new HashSet<long>();

            foreach (var matching in matchings)
            {
                matching.MatchingHeaderId = headerResult.Id;
                matching.CreateBy = loginUserId;
                matching.CreateAt = updateAt;
                matching.UpdateBy = loginUserId;
                matching.UpdateAt = updateAt;
                if (!advanceReceivedCustomerId.HasValue
                    && matching.AdvanceReceivedOccured == 1)
                    matching.AdvanceReceivedOccured = 0;

                var matchingResult = await addMatchingQueryProcessor.SaveMatchingAsync(matching, token);

                matchingResults.Add(matchingResult);

                if (applicationControl.UseDiscount == 1)
                {
                    foreach (var matchingBillingDiscount in matchingBillingDiscounts.Where(x => x.MatchingId == matching.Id))
                    {
                        matchingBillingDiscount.MatchingId = matchingResult.Id;
                        await addMatchingBillingDiscountQueryProcessor.SaveAsync(matchingBillingDiscount, token);
                    }
                }

                if (advanceReceivedCustomerId.HasValue
                    && matching.ReceiptRemain > 0M
                    && !advanceRecievedReciptId.Contains(matching.ReceiptId))
                {
                    var originalReceiptId = matching.ReceiptId;
                    advanceRecievedReciptId.Add(originalReceiptId);

                    var receiptItem = receipts.Find(item => (item.Id == originalReceiptId));
                    if (matchingProcessType == 1 && advanceReceivedCustomerId != 0)
                    {
                        receiptItem.CustomerId = advanceReceivedCustomerId;
                    }
                    var advReceipt = await addReceiptQueryProcessor.AddAdvanceReceivedAsync(
                        originalReceiptId, advanceReceivedCustomerId, loginUserId, updateAt, updateAt, token);

                    if (advReceipt != null)
                    {
                        advanceReceivedResults.Add(new AdvanceReceived
                        {
                            ReceiptId = advReceipt.Id,
                            OriginalReceiptId = advReceipt.OriginalReceiptId.Value,
                            UpdateAt = advReceipt.UpdateAt,
                            ReceiptCategoryId = advReceipt.ReceiptCategoryId,
                            LoginUserId = advReceipt.UpdateBy,
                        });
                        var receipt = (await matchingQueryProcessor.SearchReceiptByIdAsync(new long[] { advReceipt.Id }, token)).First();
                        await matchingQueryProcessor.SaveWorkReceiptTargetAsync(clientKey, receipt.Id,
                            receipt.CompanyId, receipt.CurrencyId, receipt.PayerName,
                            receipt.BankCode, receipt.BranchCode, receipt.PayerCode,
                            receipt.SourceBankName, receipt.SourceBranchName, receipt.CollationKey, receipt.CustomerId,
                            token);

                        await matchingQueryProcessor.SaveWorkCollationAsync(clientKey, advReceipt.Id, customerId ?? 0, paymentAgencyId ?? 0, advanceReceivedCustomerId ?? 0,
                            receipt.PayerName, receipt.PayerCode,
                            receipt.BankCode, receipt.BranchCode,
                            receipt.SourceBankName, receipt.SourceBranchName, receipt.CollationKey, receipt.ReceiptAmount,
                            token);
                    }

                    await updateReceiptQueryProcessor.UpdateOriginalRemainAsync(originalReceiptId, loginUserId, updateAt, token);

                    if (matchingResult != null)
                    {
                        var advanceReceipt_matching = matching;
                        advanceReceipt_matching.Id = matchingResult.Id;
                        await addMatchingQueryProcessor.UpdateMatchingAsync(advanceReceipt_matching, token);
                    }
                }

                #region 期日入金データ消込
                if (matching.UseCashOnDueDates == 1)
                {
                    Billing b = null;
                    Receipt r = null;
                    if (matchingProcessType == 0)
                    {
                        b = billings.FirstOrDefault(x => x.Id == matching.BillingId);
                        r = receipts.FirstOrDefault(x => x.Id == matching.ReceiptId);
                    }
                    else
                    {
                        b = await billingGetByIdQueryProcessor.GetByIdAsync(matching.BillingId, token);
                        r = await receiptGetByIdQueryProcessor.GetByIdAsync(matching.ReceiptId, token);
                    }
                    var newbill = b.ConvertScheduledIncome(r, matching.Amount);
                    newbill.CreateBy = loginUserId;
                    newbill.CreateAt = updateAt;
                    newbill.UpdateBy = loginUserId;
                    newbill.UpdateAt = updateAt;
                    var scheduled_billing_result = await addMatchingQueryProcessor.SaveMatchingBillingAsync(newbill, token);

                    foreach (var income in billingScheduledIncomes.Where(x => x.MatchingId == matching.Id))
                    {
                        income.BillingId = scheduled_billing_result.Id;
                        income.MatchingId = matchingResult.Id;
                        income.CreateBy = loginUserId;
                        income.CreateAt = updateAt;
                        await addBillingScheduledIncomeQueryProcessor.SaveAsync(income);
                    }
                }
                #endregion
            }

            if (applicationControl.UseDiscount == 1) //歩引き対応
            {
                foreach (var billingId in billingDiscounts)
                {
                    await updateBillingDiscountQueryProcessor.UpdateAssignmentFlagAsync(billingId, AssignmentFlag: 1, token: token);
                }
            }
            return new MatchingResult
            {
                ProcessResult = new ProcessResult { Result = true },
                Matchings = matchingResults,
                AdvanceReceiveds = advanceReceivedResults,
            };
        }

    }
}
