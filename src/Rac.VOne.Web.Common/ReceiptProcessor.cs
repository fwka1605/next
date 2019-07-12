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
    public class ReceiptProcessor : IReceiptProcessor
    {

        private readonly IReceiptQueryProcessor receiptQueryProcessor;
        private readonly IReceiptExistsQueryProcessor receiptExistsQueryProcessor;
        private readonly IUpdateReceiptQueryProcessor updateReceiptQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<Receipt> deleteReceiptByIdQueryProcessor;
        private readonly IAddReceiptQueryProcessor addReceiptQueryProcessor;
        private readonly IDeleteReceiptExcludeQueryProcessor deleteReceiptExcludeQueryProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<Receipt> receiptGetByIdsQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlProcessor;
        private readonly IAdvanceReceivedBackupQueryProcessor advanceReceivedBackupQueryProcessor;
        private readonly IReceiptMemoQueryProcessor receiptMemoQueryProcessor;
        private readonly IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor;
        private readonly IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor;
        private readonly ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor;

        private readonly IKanaHistoryCustomerQueryProcessor kanaHistoryCustomerQueryProcessor;
        private readonly IAddKanaHistoryCustomerQueryProcessor addKanaHistoryCustomerQueryProcessor;
        private readonly IMatchingQueryProcessor matchingQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public ReceiptProcessor(

            IReceiptQueryProcessor receiptQueryProcessor,
            IReceiptExistsQueryProcessor receiptExistsQueryProcessor,
            IUpdateReceiptQueryProcessor updateReceiptQueryProcessor,
            IDeleteTransactionQueryProcessor<Receipt> deleteReceiptByIdQueryProcessor,
            IAddReceiptQueryProcessor addReceiptQueryProcessor,
            IDeleteReceiptExcludeQueryProcessor deleteReceiptExcludeQueryProcessor,
            ITransactionalGetByIdsQueryProcessor<Receipt> receiptGetByIdsQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlProcessor,
            IAdvanceReceivedBackupQueryProcessor advanceReceivedBackupQueryProcessor,
            IReceiptMemoQueryProcessor receiptMemoQueryProcessor,
            IAddReceiptMemoQueryProcessor addReceiptMemoQueryProcessor,
            IDeleteReceiptMemoQueryProcessor deleteReceiptMemoQueryProcessor,
            ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor,
            IKanaHistoryCustomerQueryProcessor kanaHistoryCustomerQueryProcessor,
            IAddKanaHistoryCustomerQueryProcessor addKanaHistoryCustomerQueryProcessor,
            IMatchingQueryProcessor matchingQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.receiptQueryProcessor = receiptQueryProcessor;
            this.receiptExistsQueryProcessor = receiptExistsQueryProcessor;
            this.updateReceiptQueryProcessor = updateReceiptQueryProcessor;
            this.deleteReceiptByIdQueryProcessor = deleteReceiptByIdQueryProcessor;
            this.addReceiptQueryProcessor = addReceiptQueryProcessor;
            this.deleteReceiptExcludeQueryProcessor = deleteReceiptExcludeQueryProcessor;
            this.receiptGetByIdsQueryProcessor = receiptGetByIdsQueryProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.advanceReceivedBackupQueryProcessor = advanceReceivedBackupQueryProcessor;
            this.receiptMemoQueryProcessor = receiptMemoQueryProcessor;
            this.addReceiptMemoQueryProcessor = addReceiptMemoQueryProcessor;
            this.deleteReceiptMemoQueryProcessor = deleteReceiptMemoQueryProcessor;
            this.categoryByCodeQueryProcessor = categoryByCodeQueryProcessor;
            this.kanaHistoryCustomerQueryProcessor = kanaHistoryCustomerQueryProcessor;
            this.addKanaHistoryCustomerQueryProcessor = addKanaHistoryCustomerQueryProcessor;
            this.matchingQueryProcessor = matchingQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        #region IReceiptExistQueryProcessor
        public async Task<bool> ExistReceiptCategoryAsync(int ReceiptCategoryId, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistReceiptCategoryAsync(ReceiptCategoryId, token);

        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistCustomerAsync(CustomerId, token);

        public async Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistCurrencyAsync(CurrencyId, token);

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistSectionAsync(SectionId, token);


        public async Task<bool> ExistCompanyAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistCompanyAsync(CompanyId, token);
        public async Task<bool> ExistOriginalReceiptAsync(long ReceiptId, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistOriginalReceiptAsync(ReceiptId, token);

        public async Task<bool> ExistNonApportionedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistNonApportionedAsync(CompanyId, ClosingFrom, ClosingTo, token);

        public async Task<bool> ExistNonOutputedAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistNonOutputedAsync(CompanyId, ClosingFrom, ClosingTo, token);

        public async Task<bool> ExistNonAssignmentAsync(int CompanyId, DateTime ClosingFrom, DateTime ClosingTo, CancellationToken token = default(CancellationToken))
            => await receiptExistsQueryProcessor.ExistNonAssignmentAsync(CompanyId, ClosingFrom, ClosingTo, token);

        #endregion

        public async Task<IEnumerable<Receipt>> GetByIdsAsync(IEnumerable<long> ReceiptId, CancellationToken token = default(CancellationToken))
            => await receiptGetByIdsQueryProcessor.GetByIdsAsync(ReceiptId, token);


        public async Task<Receipt> GetReceiptAsync(long id, CancellationToken token = default(CancellationToken))
            => await receiptQueryProcessor.GetReceiptAsync(id, token);


        public Task<IEnumerable<int>> ReceiptImportDuplicationCheckAsync(int CompanyId, IEnumerable<ReceiptImportDuplication> ReceiptImportDuplication, IEnumerable<ImporterSettingDetail> ImporterSettingDetail,
            CancellationToken token = default(CancellationToken))
            => receiptQueryProcessor.ReceiptImportDuplicationCheckAsync(CompanyId, ReceiptImportDuplication, ImporterSettingDetail, token);


        public async Task<ReceiptInputsResult> SaveAsync(ReceiptSaveItem item, CancellationToken token = default(CancellationToken))
        {
            var items = item.Receipts;
            var result = new List<ReceiptInput>();
            using (var scope = transactionScopeBuilder.Create())
            {
                var first = items.First();
                var registered = first.Id != 0L;
                var updateAll = first.InputType == 2; // InputType = 2（入力）
                var saveKanaHistory = first.LearnKanaHistory;

                if (registered)
                {
                    result = new List<ReceiptInput>();
                    var input = updateAll
                        ? await updateReceiptQueryProcessor.UpdateReceiptInputAsync(first, token)
                        : await updateReceiptQueryProcessor.UpdateCustomerIdAsync(first, token);
                    if (input == null)
                    {
                        return new ReceiptInputsResult
                        {
                            ProcessResult = new ProcessResult
                            {
                                ErrorCode = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated,
                                ErrorMessage = "データの取得後に、データの更新が発生しています",
                            },
                        };
                    }

                    result.Add(input);

                    if ((!updateAll) && saveKanaHistory && input.CustomerId.HasValue)
                    {
                        var kana = new KanaHistoryCustomer {
                            CompanyId           = input.CompanyId,
                            PayerName           = input.PayerName,
                            SourceBankName      = input.SourceBankName ?? string.Empty,
                            SourceBranchName    = input.SourceBranchName ?? string.Empty,
                            CustomerId          = input.CustomerId.Value,
                            HitCount            = 1,
                            CreateBy            = input.CreateBy,
                            UpdateBy            = input.UpdateBy,
                        };

                        var exist = await kanaHistoryCustomerQueryProcessor.ExistAsync(kana, token);
                        if (!exist)
                            await addKanaHistoryCustomerQueryProcessor.SaveAsync(kana, token);
                    }
                }
                else
                {
                    result = (await addReceiptQueryProcessor.SaveReceiptInputAsync(items, token)).ToList();
                }

                for (int i = 0; i < result.Count; i++)
                {
                    var receiptId = result[i].Id;
                    var memo = items[i].Memo;

                    if (!string.IsNullOrWhiteSpace(memo))
                    {
                        await addReceiptMemoQueryProcessor.SaveAsync(receiptId, memo, token);
                        result[i].Memo = memo;
                    }
                }

                if (item.ClientKey != null && item.ParentCustomerId.HasValue )
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        var r = result[i];
                        await matchingQueryProcessor.SaveWorkReceiptTargetAsync(item.ClientKey, r.Id,
                            r.CompanyId, r.CurrencyId, r.PayerName, "", "", r.PayerCode, r.SourceBankName, r.SourceBranchName, r.CollationKey, r.CustomerId, token);
                        await matchingQueryProcessor.SaveWorkCollationAsync(item.ClientKey, r.Id, item.ParentCustomerId.Value, 0, r.CustomerId ?? 0,
                            r.PayerName, r.PayerCode, "", "", r.SourceBankName, r.SourceBranchName, r.CollationKey, r.ReceiptAmount, token);
                    }
                }

                scope.Complete();
            }
            return new ReceiptInputsResult
            {
                ProcessResult = new ProcessResult { Result = true },
                ReceiptInputs = result,
            };
        }

        public async Task<int> DeleteAsync(long id, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                result = await deleteReceiptExcludeQueryProcessor.DeleteAsync(id, token);
                result = await deleteReceiptByIdQueryProcessor.DeleteAsync(id, token);
                scope.Complete();
            }
            return result;
        }

        public async Task<CountResult> OmitAsync(OmitSource source, CancellationToken token = default(CancellationToken))
        {
            var result = new CountResult {
                ProcessResult = new ProcessResult(),
            };
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var item in source.Transactions)
                {
                    var updateResult = await updateReceiptQueryProcessor.OmitAsync(source.DoDelete ? 1 : 0, source.LoginUserId, item, token);
                    if (updateResult == 0)
                    {
                        result.ProcessResult.ErrorCode = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated;
                        return result;
                    }
                    result.Count += updateResult;
                }
                result.ProcessResult.Result = true;
                scope.Complete();
            }
            return result;
        }
    }
}
