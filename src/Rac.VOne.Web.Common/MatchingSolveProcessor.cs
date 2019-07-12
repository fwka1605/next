using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class MatchingSolveProcessor : IMatchingSolveProcessor
    {
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor;

        public MatchingSolveProcessor(
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor
            )
        {
            this.applicationControlQueryProcessor = applicationControlQueryProcessor;
        }

        private Dictionary<int, decimal> GetBillingPair(List<Billing> billingItems,
            int useScheduledPayment, int useDeclaredAmount,
            ref int? doCreateAdvaceReceived,
            ref decimal billingTotal,
            ref bool isAllMinusBilling )
        {
            var pair = new Dictionary<int, decimal>();
            foreach (var billing in billingItems)
            {
                if (useScheduledPayment == 1
                    && useDeclaredAmount == 1)
                {
                    billing.OffsetAmount = billing.RemainAmount - billing.TargetAmount;
                    doCreateAdvaceReceived = 0;
                }

                var amount = (doCreateAdvaceReceived != 1
                    || billing.TargetAmount <= billing.RemainAmount)
                    ? billing.TargetAmount
                    : billing.RemainAmount;
                billingTotal += (amount - billing.DiscountAmount);
                pair.Add(billingItems.IndexOf(billing), amount);
                if (billing.RemainAmount > 0M) isAllMinusBilling = false;
            }
            return pair;
        }

        private Dictionary<int, decimal> GetReceiptPair(List<Receipt> receiptItems,
            ref decimal receiptTotal,
            ref bool isAllMinusReceipt)
        {
            var pair = new Dictionary<int, decimal>();
            foreach (var receipt in receiptItems)
            {
                pair.Add(receiptItems.IndexOf(receipt), receipt.RemainAmount);
                receiptTotal += receipt.RemainAmount;
                if (receipt.RemainAmount > 0M) isAllMinusReceipt = false;
            }
            return pair;
        }

        private IEnumerable<BillingScheduledIncome> ConvertMatchingToScheduled(IEnumerable<Matching> matchings)
            => matchings?.Where(x => x.UseCashOnDueDates != 0)
            .Select(x => new BillingScheduledIncome
            {
                MatchingId = x.Id,
                BillingId = x.BillingId,
            });

        private IEnumerable<MatchingBillingDiscount> ConvertBillingToDiscounts(Billing billing, long matchingId)
        {
            if (billing.DiscountAmount == 0M) yield return null;
            if (billing.DiscountAmount1 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = matchingId,
                    DiscountType = 1,
                    DiscountAmount = billing.DiscountAmount1,
                };
            if (billing.DiscountAmount2 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = matchingId,
                    DiscountType = 2,
                    DiscountAmount = billing.DiscountAmount2,
                };
            if (billing.DiscountAmount3 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = matchingId,
                    DiscountType = 3,
                    DiscountAmount = billing.DiscountAmount3,
                };
            if (billing.DiscountAmount4 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = matchingId,
                    DiscountType = 4,
                    DiscountAmount = billing.DiscountAmount4,
                };
            if (billing.DiscountAmount5 != 0M)
                yield return new MatchingBillingDiscount
                {
                    MatchingId = matchingId,
                    DiscountType = 5,
                    DiscountAmount = billing.DiscountAmount5,
                };
        }

        public async Task<MatchingSource> SolveAsync(
            MatchingSource source,
            CollationSearch option,
            ApplicationControl control = null,
            CancellationToken token = default(CancellationToken))
        {
            if (control == null)
                control = await applicationControlQueryProcessor.GetAsync(option.CompanyId, token);

            var useCashOnDueDates = control.UseCashOnDueDates;
            var useScheduledPayment = control.UseScheduledPayment;
            var useDeclaredAmount = control.UseDeclaredAmount;
            int? doCreateAdvanceReceived = option.DoTransferAdvanceReceived ? 1 : 0;
            var taxDifference = source.TaxDifference;
            var bankTransferFee = source.BankTransferFee;


            var billingItems = source.Billings;
            var receiptItems = source.Receipts;

            if (!(billingItems?.Any() ?? false))
                throw new ArgumentNullException("Billings");

            if (!(receiptItems?.Any() ?? false))
                throw new ArgumentNullException("Receipts");

            int companyId;
            int currencyId;
            {
                var billing = billingItems.First();
                companyId = billing.CompanyId;
                currencyId = billing.CurrencyId;
            }

            var isAllMinusBilling = true;
            var billingTotal = 0M;

            var billingPair = GetBillingPair(billingItems, useScheduledPayment, useDeclaredAmount,
                ref doCreateAdvanceReceived, ref billingTotal, ref isAllMinusBilling);

            var isAllMinusReceipt = true;
            var receiptTotal = 0M;
            var receiptPair = GetReceiptPair(receiptItems, ref receiptTotal, ref isAllMinusReceipt);

            var isAllMinus = isAllMinusBilling && isAllMinusReceipt;
            var taxDiff = taxDifference;
            var bankFee = bankTransferFee;
            var isEqual = (billingTotal == receiptTotal + bankFee - taxDiff);
            Func<decimal, decimal, decimal> amountSolver = Math.Min;
            if (isAllMinus) amountSolver = Math.Max;
            var header = new MatchingHeader
            {
                Id = 1,
                CompanyId = companyId,
                CurrencyId = currencyId,
                BankTransferFee = bankFee,
                TaxDifference = taxDiff,
            };

            var recordedAt = option.AdvanceReceivedRecordedAt;

            var matchings = new List<Matching>();
            var billingScheduledIncomes = new List<BillingScheduledIncome>();
            var matchingBillingDiscounts = new List<MatchingBillingDiscount>();
            var billingDiscounts = new HashSet<long>();
            var discountTotal = 0M;
            var beforBillingRemainTotal = 0M;
            var beforReceiptRemainTotal = 0M;

            var bindex = 0;
            var rindex = 0;
            var nextBilling = true;
            var nextReceipt = true;
            Billing bill = null;
            Receipt rcpt = null;
            while (bindex < billingItems.Count
                && rindex < receiptItems.Count
                && (nextBilling || nextReceipt))
            {
                if (token.IsCancellationRequested)
                    throw new OperationCanceledException();

                if (nextBilling)
                    bill = billingItems[bindex];
                if (nextReceipt)
                    rcpt = receiptItems[rindex];
                var isLastBill = bindex == billingItems.Count - 1;
                var isLastRcpt = rindex == receiptItems.Count - 1;

                var discount = bill.DiscountAmount;
                var billTaxDiff = (0M < taxDiff) ? taxDiff : 0M;
                var rcptTaxDiff = (0M < taxDiff) ? 0M : -taxDiff;

                var matching = new Matching();
                matching.Id = matchings.Count;
                matching.CompanyId = companyId;
                matching.CurrencyId = currencyId;
                matching.MatchingHeaderId = header.Id;
                matching.ReceiptId = rcpt.NettingId ?? rcpt.Id;
                matching.PayerName = rcpt.PayerName;
                matching.SourceBankName = rcpt.SourceBankName;
                matching.SourceBranchName = rcpt.SourceBranchName;
                matching.IsNetting = rcpt.NettingId.HasValue;
                matching.RecordedAt = (rcpt.OriginalReceiptId.HasValue ? recordedAt : null) ?? rcpt.RecordedAt;
                matching.ReceiptHeaderId = rcpt.ReceiptHeaderId;
                if (useCashOnDueDates == 1 && rcpt.UseCashOnDueDates == 1)
                    matching.UseCashOnDueDates = 1;

                matching.BillingId = bill.Id;
                if (useScheduledPayment == 1 && useDeclaredAmount == 1)
                    matching.OffsetAmount = bill.OffsetAmount;

                if (discount != 0M)
                {
                    #region billing discount
                    matching.DiscountAmount1 = bill.DiscountAmount1;
                    matching.DiscountAmount2 = bill.DiscountAmount2;
                    matching.DiscountAmount3 = bill.DiscountAmount3;
                    matching.DiscountAmount4 = bill.DiscountAmount4;
                    matching.DiscountAmount5 = bill.DiscountAmount5;
                    if (!billingDiscounts.Contains(bill.Id))
                        billingDiscounts.Add(bill.Id);
                    matchingBillingDiscounts.AddRange(ConvertBillingToDiscounts(bill, matching.Id));
                    discountTotal += discount;
                    bill.DiscountAmount = 0M;
                    bill.DiscountAmount1 = 0M;
                    bill.DiscountAmount2 = 0M;
                    bill.DiscountAmount3 = 0M;
                    bill.DiscountAmount4 = 0M;
                    bill.DiscountAmount5 = 0M;
                    #endregion
                }
                matching.BankTransferFee = bankFee;
                matching.TaxDifference = taxDiff;
                matching.Memo = bill.Memo;
                var billAmount = billingPair[bindex] - rcptTaxDiff - bankFee - discount;
                var rcptAmount = receiptPair[rindex] - billTaxDiff;

                matching.Amount
                    = (isEqual &&  isLastRcpt && !isLastBill) ? billAmount
                    : (isEqual && !isLastRcpt &&  isLastBill) ? rcptAmount
                    : amountSolver(billAmount, rcptAmount);

                header.Amount += matching.Amount;

                if (matching.Amount == 0M && taxDiff == 0M && bankFee == 0M && discount == 0M)
                {
                    break;
                }

                var billAssignAmount = (matching.Amount + rcptTaxDiff + bankFee + discount);
                var rcptAssignAmount = (matching.Amount + billTaxDiff);
                if (nextBilling)
                {
                    beforBillingRemainTotal += bill.RemainAmount;
                    bill.AssignmentAmount = billAssignAmount;
                }
                else
                {
                    bill.AssignmentAmount += billAssignAmount;
                }

                if (nextReceipt)
                {
                    beforReceiptRemainTotal += rcpt.RemainAmount;
                    rcpt.AssignmentAmount = rcptAssignAmount;
                }
                else
                {
                    rcpt.AssignmentAmount += rcptAssignAmount;
                }

                billingPair[bindex] -= billAssignAmount;
                receiptPair[rindex] -= rcptAssignAmount;

                nextBilling = billingPair[bindex] == 0M && !isLastBill;
                nextReceipt = receiptPair[rindex] == 0M && !isLastRcpt;

                matchings.Add(matching);


                if (!isEqual)
                {
                    if (!nextBilling && isLastBill && receiptPair[rindex] > 0M && !rcpt.OriginalReceiptId.HasValue)
                    {
                        matching.AdvanceReceivedOccured = 1;
                    }
                    if (!nextBilling && isLastBill && receiptPair[rindex] != 0M)
                    {
                        foreach (var m in matchings.Where(x => x.ReceiptId == rcpt.Id))
                            m.ReceiptRemain = receiptPair[rindex];
                    }
                    if (!nextReceipt && isLastRcpt && billingPair[bindex] != 0M)
                    {
                        foreach (var m in matchings.Where(x => x.BillingId == bill.Id))
                            m.BillingRemain = billingPair[bindex];
                    }
                    if (billingPair[bindex] == 0M && isLastBill
                     || receiptPair[rindex] == 0M && isLastRcpt) break;
                }

                if (nextBilling) bindex++;
                if (nextReceipt) rindex++;

                taxDiff = 0M;
                bankFee = 0M;
            }

            var remainType = 0;

            var billingRemainTotal = billingPair.Sum(x => x.Value);
            var receiptRemainTotal = receiptPair.Sum(x => x.Value);
            if (billingRemainTotal == 0M && receiptRemainTotal == 0M)
            {
                remainType = 0;
            }
            else if (billingRemainTotal != 0M)
            {
                remainType = 1;
            }
            else
            {
                if (!rcpt.OriginalReceiptId.HasValue
                    && option.UseAdvanceReceived
                    && receiptPair[rindex] > 0M)
                    remainType = 3;
                else
                    remainType = 2;
            }

            if (useCashOnDueDates == 1)
                billingScheduledIncomes.AddRange(ConvertMatchingToScheduled(matchings));


            billingItems = billingItems.Take(bindex + 1).ToList();
            receiptItems = receiptItems.Take(rindex + 1).ToList();
            header.BillingCount = billingItems.Count;
            header.ReceiptCount = receiptItems.Count;

            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            return new MatchingSource
            {
                RemainType = remainType,
                Matchings = matchings,
                Billings = billingItems,
                Receipts = receiptItems,
                BillingDiscounts = billingDiscounts,
                MatchingBillingDiscounts = matchingBillingDiscounts,
                MatchingHeader = header,
                BillingScheduledIncomes = billingScheduledIncomes,
                BillingRemainTotal = beforBillingRemainTotal,
                ReceiptRemainTotal = beforReceiptRemainTotal,
                BillingDiscountTotal = discountTotal
            };
        }
    }
}
