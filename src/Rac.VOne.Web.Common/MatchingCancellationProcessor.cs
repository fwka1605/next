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
    public class MatchingCancellationProcessor :
        IMatchingCancellationProcessor
    {
        private readonly IDbSystemDateTimeQueryProcessor dbSystemDatetimeQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyIdQueryProcessor;
        private readonly ICancelMatchingQueryProcessor cancelMatchingQueryProcessor;
        private readonly IDeleteTransactionDataQueryProcessor<MatchingHeader> deleteMatchingHeaderQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<Billing> deleteBillingByIdQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<Receipt> receiptGetByIdsQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<Receipt> deleteReceiptByIdQueryProcessor;
        private readonly IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor;
        private readonly IDeleteMatchingBillingDiscountQueryProcessor deleteMatchingBillingDiscountQueryProcessor;
        private readonly INettingQueryProcessor nettingQueryProcessor;
        private readonly IUpdateNettingQueryProcessor updateNettingQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MatchingCancellationProcessor(
            IDbSystemDateTimeQueryProcessor dbSystemDatetimeQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlGetByCompanyIdQueryProcessor,
            ICancelMatchingQueryProcessor cancelMatchingQueryProcessor,
            IDeleteTransactionDataQueryProcessor<MatchingHeader> deleteMatchingHeaderQueryProcessor,
            IDeleteTransactionQueryProcessor<Billing> deleteBillingByIdQueryProcessor,
            ITransactionalGetByIdsQueryProcessor<Receipt> receiptGetByIdsQueryProcessor,
            IDeleteTransactionQueryProcessor<Receipt> deleteReceiptByIdQueryProcessor,
            IDeleteReceiptQueryProcessor deleteReceiptQueryProcessor,
            IDeleteMatchingBillingDiscountQueryProcessor deleteMatchingBillingDiscountQueryProcessor,
            INettingQueryProcessor nettingQueryProcessor,
            IUpdateNettingQueryProcessor updateNettingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.dbSystemDatetimeQueryProcessor = dbSystemDatetimeQueryProcessor;
            this.applicationControlGetByCompanyIdQueryProcessor = applicationControlGetByCompanyIdQueryProcessor;
            this.cancelMatchingQueryProcessor = cancelMatchingQueryProcessor;
            this.deleteMatchingHeaderQueryProcessor = deleteMatchingHeaderQueryProcessor;
            this.deleteBillingByIdQueryProcessor = deleteBillingByIdQueryProcessor;
            this.receiptGetByIdsQueryProcessor = receiptGetByIdsQueryProcessor;
            this.deleteReceiptByIdQueryProcessor = deleteReceiptByIdQueryProcessor;
            this.deleteReceiptQueryProcessor = deleteReceiptQueryProcessor;
            this.deleteMatchingBillingDiscountQueryProcessor = deleteMatchingBillingDiscountQueryProcessor;
            this.nettingQueryProcessor = nettingQueryProcessor;
            this.updateNettingQueryProcessor = updateNettingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<MatchingResult> CancelAsync(IEnumerable<MatchingHeader> headers, int loginUserId,
            CancellationToken token = default(CancellationToken),
            IProgressNotifier notifier = null)
        {
            var headersArray = headers.ToArray();

            var updateAt = await dbSystemDatetimeQueryProcessor.GetAsync(token);

            List<Matching> details = null;
            List<Billing> billings = null;
            List<Receipt> receipts = null;

            var deleteMatchings = new List<Matching>();
            var deleteReceipts = new List<Receipt>();

            var companyId = headers.Select(x => x.CompanyId).First();
            var appControl = await applicationControlGetByCompanyIdQueryProcessor.GetAsync(companyId, token);
            var useCashOnDueDates = appControl.UseCashOnDueDates == 1;

            using (var scope = transactionScopeBuilder.Create())
            {

                foreach (var header in headers)
                {
                    if (token.IsCancellationRequested)
                        return new MatchingResult { MatchingErrorType = MatchingErrorType.ProcessCanceled };

                    #region １．消込データ（Matching）取得

                    details = (await cancelMatchingQueryProcessor.GetByHeaderIdAsync(header.Id, token)).ToList();

                    deleteMatchings.AddRange(details);

                    #endregion

                    #region 2．消込済請求データ取得
                    // billing deleteAt など matching 以外の情報が必要なので、 query で呼び出し必須
                    billings = (await cancelMatchingQueryProcessor.GetMatchedBillingsForCancelAsync(header.Id, token)).ToList();

                    #endregion

                    if (billings.Count == 0)
                    {
                        notifier?.Abort();
                        return new MatchingResult { MatchingErrorType = MatchingErrorType.NotExistBillingData };
                    }

                    //3．請求データチェック

                    if (billings.Any(e => e.DeleteAt.HasValue))
                    {
                        notifier?.Abort();
                        return new MatchingResult
                        {
                            MatchingErrorType = MatchingErrorType.BillingOmitted,
                            ErrorIndex = Array.IndexOf(headersArray, header),
                        };
                    }

                    if (useCashOnDueDates
                        && await cancelMatchingQueryProcessor.ExistAssignmentScheduledIncomeAsync(details.Select(x => x.Id).ToArray(), token))
                    {
                        notifier?.Abort();
                        return new MatchingResult
                        {
                            MatchingErrorType = MatchingErrorType.CashOnDueDateOmitted,
                            ErrorIndex = Array.IndexOf(headersArray, header),
                        };
                    }

                    #region ４．請求データ消込解除処理

                    // 期日現金管理用 請求ID -> 消込ID の Dictionary
                    Dictionary<long, long[]> billingIdToMatchingIds = null;
                    if (useCashOnDueDates)
                    {
                        billingIdToMatchingIds = details.GroupBy(x => x.BillingId)
                            .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToArray());
                    }

                    foreach (var billing in billings)
                    {
                        var item = billing;
                        var matchingAmount = billing.AssignmentAmount;
                        var bankFee = billing.BankTransferFee;
                        var taxDiff = billing.TaxDifference;
                        var discount = billing.DiscountAmount;
                        var amount = matchingAmount + bankFee + discount - (taxDiff < 0 ? taxDiff : 0M);

                        item.CompanyId = header.CompanyId;
                        item.RemainAmount = amount;
                        item.AssignmentAmount = amount;
                        item.UpdateBy = loginUserId;
                        item.UpdateAt = updateAt;

                        var updatedBilling = await cancelMatchingQueryProcessor.UpdateBillingForCancelMatchingAsync(item, token);
                        if (updatedBilling == null)
                        {
                            notifier?.Abort();
                            return new MatchingResult { MatchingErrorType = MatchingErrorType.CancelError };
                        }

                        await cancelMatchingQueryProcessor.UpdatePreviousBillingLogsAsync(header.Id, billing.Id, amount, loginUserId, updateAt, token);

                        if (useCashOnDueDates)
                        {
                            foreach (var matchingId in billingIdToMatchingIds[billing.Id])
                            {
                                var income = await cancelMatchingQueryProcessor.GetBillingScheduledIncomeAsync(matchingId, token);
                                if (income != null)
                                {
                                    await cancelMatchingQueryProcessor.DeleteBillingShceduledIncomeAsync(income.BillingId, token);
                                    await deleteBillingByIdQueryProcessor.DeleteAsync(income.BillingId, token);
                                }
                            }
                        }
                    }

                    #endregion

                    #region ５．相殺データ消込解除処理

                    var nettings = (await nettingQueryProcessor.GetByMatchingHeaderIdAsync(header.Id, token)).ToList();
                    var nettingReceiptIds = new List<long>();
                    foreach (var netting in nettings)
                    {
                        nettingReceiptIds.Add((long)netting.ReceiptId);
                        var cancelFlag = 1;
                        await updateNettingQueryProcessor.UpdateMatchingNettingAsync(header.CompanyId, 0, netting.Id, cancelFlag, token);
                    }

                    if (nettings.Any())
                    {
                        var receiptsForNetting = await receiptGetByIdsQueryProcessor.GetByIdsAsync(nettings.Select(n => (long)n.ReceiptId), token);
                        deleteReceipts.AddRange(receiptsForNetting);
                    }
                    #endregion

                    #region 7．消込済入金データ取得

                    receipts = (await cancelMatchingQueryProcessor.GetMatchedReceiptsForCancelAsync(header, token)).ToList();

                    #endregion

                    if (receipts.Count == 0)
                    {
                        notifier?.Abort();
                        return new MatchingResult { MatchingErrorType = MatchingErrorType.NotExistReceiptData };
                    }

                    #region 8．入金データチェック

                    if (receipts.Any(e => e.DeleteAt.HasValue))
                    {
                        notifier?.Abort();
                        return new MatchingResult
                        {
                            MatchingErrorType = MatchingErrorType.ReceiptOmitted,
                            ErrorIndex = Array.IndexOf(headersArray, header),
                        };
                    }

                    #endregion

                    #region 9．入金データ消込解除処理

                    var hasAdvanceReceivedOccured = details.Any(x => (x.AdvanceReceivedOccured == 1));
                    foreach (var receipt in receipts)
                    {
                        var prepare_receipt_update = receipt;

                        var receiptId = receipt.Id;

                        var amount = prepare_receipt_update.AssignmentAmount;

                        prepare_receipt_update.RemainAmount = amount;
                        prepare_receipt_update.AssignmentAmount = amount;
                        prepare_receipt_update.CompanyId = header.CompanyId;
                        prepare_receipt_update.UpdateBy = loginUserId;
                        prepare_receipt_update.UpdateAt = updateAt;

                        await cancelMatchingQueryProcessor.UpdateReceiptForCancelMatchingAsync(prepare_receipt_update, token);

                        // 入金残ログ洗替え
                        await cancelMatchingQueryProcessor.UpdatePreviousReceiptLogsAsync(header.Id, receiptId, amount, loginUserId, updateAt, token);

                        if (!hasAdvanceReceivedOccured) continue;
                        //前受データの削除
                        var maeuke_receipt_flg = details.Exists(x => ((x.AdvanceReceivedOccured == 1) && (x.ReceiptId == receipt.Id)));

                        if (!maeuke_receipt_flg) continue;
                        var originalReceiptId = receipt.Id;

                        //前受のテータ取得
                        var maeuke_Receipts = (await cancelMatchingQueryProcessor.GetByOriginalIdAsync(originalReceiptId)).ToList();

                        foreach (var maeuke_receipt in maeuke_Receipts)
                        {
                            int canel_flg = await deleteReceiptQueryProcessor.CancelAdvanceReceivedAsync(maeuke_receipt.Id, token);
                            if (canel_flg != 1) continue;
                            // データ同期用
                            deleteReceipts.Add(maeuke_receipt);

                            var originalReceipt = receipts.Find(x => (x.Id == originalReceiptId));
                            originalReceipt.RemainAmount = maeuke_receipt.ReceiptAmount;
                            originalReceipt.AssignmentAmount = 0; //消込額から減算は不要なので
                            originalReceipt.UpdateBy = loginUserId;
                            originalReceipt.UpdateAt = updateAt;

                            await cancelMatchingQueryProcessor.UpdateReceiptForCancelMatchingAsync(originalReceipt, token);
                        }
                    }
                    #endregion

                    #region 8.その他処理

                    #region 消込履歴データ検索・出力済データ（MatchingOutputed）の削除

                    await cancelMatchingQueryProcessor.DeleteMatchingOutputedAsync(header.Id, token);

                    #endregion

                    #region 消込歩引データ（MatchingBillingDiscount）の削除

                    foreach (var matching in details)
                    {
                        await deleteMatchingBillingDiscountQueryProcessor.DeleteByMatchingIdAsync(matching.Id, token);
                    }

                    #endregion

                    #region 消込データ（Matching）の削除

                    var deleteMatchingResult = await cancelMatchingQueryProcessor.DeleteMatchingAsync(header.Id, header.MatchingUpdateAt, token);
                    if (deleteMatchingResult <= 0)
                    {
                        notifier?.Abort();
                        return new MatchingResult
                        {
                            MatchingErrorType = MatchingErrorType.MatchingHeaderChanged,
                            ErrorIndex = Array.IndexOf(headersArray, header),
                        };
                    }

                    #endregion

                    #region MatchingHeaderの削除

                    var deleteResult = await deleteMatchingHeaderQueryProcessor.DeleteAsync(header, token);
                    if (deleteResult <= 0)
                    {
                        notifier?.Abort();
                        return new MatchingResult
                        {
                            MatchingErrorType = MatchingErrorType.MatchingHeaderChanged,
                            ErrorIndex = Array.IndexOf(headersArray, header),
                        };
                    }

                    #endregion

                    #region 相殺データから変換した入金データの削除
                    foreach (long receiptId in nettingReceiptIds)
                    {
                        await deleteReceiptByIdQueryProcessor.DeleteAsync(receiptId, token);
                    }
                    #endregion

                    #endregion

                    notifier?.UpdateState();
                }

                scope.Complete();

                return new MatchingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Matchings = deleteMatchings,
                    DeleteReceipts = deleteReceipts,
                };
            }
        }
    }
}
