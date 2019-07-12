using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Common
{
    public class BillingProcessor :
        IBillingProcessor
    {
        private readonly IBillingQueryProcessor billingQueryProcessor;
        private readonly IBillingJournalizingQueryProcessor billingJournalizingQueryProcessor;
        private readonly IBillingExistsQueryProcessor billingExistsQueryProcessor;
        private readonly IAddBillingQueryProcessor addBillingQueryProcessor;
        private readonly IUpdateBillingQueryProcessor updatebillingQueryProcessor;
        private readonly ICurrencyQueryProcessor currencyQueryProcessor;
        private readonly ICustomerQueryProcessor customerQueryProcessor;

        private readonly IAccountTitleQueryProcessor accountTitlteByIdCodeQueryProcessor;
        private readonly IDepartmentByCodeQueryProcessor departmentByCodeQueryProcessor;
        private readonly IStaffQueryProcessor staffQueryProcessor;
        private readonly ICategoriesQueryProcessor categoryQueryProcessor;
        private readonly IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor;
        private readonly IBillingMemoProcessor billingMemoProcessor;
        private readonly ITransactionalGetByIdsQueryProcessor<Billing> billingGetByIdsQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<Billing> deleteBillingQueryProcessor;
        private readonly IMasterGetIdByCodeQueryProcessor<Currency> currencyGetIdByCodeQueryProcessor;

        private readonly IDeleteBillingDiscountQueryProcessor deleteBillingDiscountQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> getApplicationControlByCompanyQueryProcessor;
        private readonly IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor;
        private readonly IDeleteBillingDivisionContractQueryProcessor deleteBillingDivisionContractQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public BillingProcessor(
            IBillingQueryProcessor billingQueryProcessor,
            IBillingJournalizingQueryProcessor billingJournalizingQueryProcessor,
            IBillingExistsQueryProcessor billingExistsQueryProcessor,
            IAddBillingQueryProcessor addBillingQueryProcessor,
            IUpdateBillingQueryProcessor updatebillingQueryProcessor,
            ICurrencyQueryProcessor currencyQueryProcessor,
            ICustomerQueryProcessor customerQueryProcessor,
            IAccountTitleQueryProcessor accountTitlteByIdCodeQueryProcessor,
            IDepartmentByCodeQueryProcessor departmentByCodeQueryProcessor,
            IStaffQueryProcessor staffQueryProcessor,
            ICategoriesQueryProcessor categoryQueryProcessor,
            IImporterSettingDetailQueryProcessor importerSettingDetailQueryProcessor,
            IBillingMemoProcessor billingMemoProcessor,
            ITransactionalGetByIdsQueryProcessor<Billing> billingGetByIdsQueryProcessor,
            IDeleteTransactionQueryProcessor<Billing> deleteBillingQueryProcessor,
            IMasterGetIdByCodeQueryProcessor<Currency> currencyGetIdByCodeQueryProcessor,
            IDeleteBillingDiscountQueryProcessor deleteBillingDiscountQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> getApplicationControlByCompanyQueryProcessor,
            IUpdateBillingDivisionContractQueryProcessor updateBillingDivisionContractQueryProcessor,
            IDeleteBillingDivisionContractQueryProcessor deleteBillingDivisionContractQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingQueryProcessor = billingQueryProcessor;
            this.billingJournalizingQueryProcessor = billingJournalizingQueryProcessor;
            this.billingExistsQueryProcessor = billingExistsQueryProcessor;
            this.addBillingQueryProcessor = addBillingQueryProcessor;
            this.updatebillingQueryProcessor = updatebillingQueryProcessor;
            this.currencyQueryProcessor = currencyQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
            this.accountTitlteByIdCodeQueryProcessor = accountTitlteByIdCodeQueryProcessor;
            this.departmentByCodeQueryProcessor = departmentByCodeQueryProcessor;
            this.staffQueryProcessor = staffQueryProcessor;
            this.categoryQueryProcessor = categoryQueryProcessor;
            this.importerSettingDetailQueryProcessor = importerSettingDetailQueryProcessor;
            this.billingMemoProcessor = billingMemoProcessor;
            this.billingGetByIdsQueryProcessor = billingGetByIdsQueryProcessor;
            this.deleteBillingQueryProcessor = deleteBillingQueryProcessor;
            this.currencyGetIdByCodeQueryProcessor = currencyGetIdByCodeQueryProcessor;
            this.deleteBillingDiscountQueryProcessor = deleteBillingDiscountQueryProcessor;
            this.getApplicationControlByCompanyQueryProcessor = getApplicationControlByCompanyQueryProcessor;
            this.updateBillingDivisionContractQueryProcessor = updateBillingDivisionContractQueryProcessor;
            this.deleteBillingDivisionContractQueryProcessor = deleteBillingDivisionContractQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public Task<bool> ExistDebitAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistDebitAccountTitleAsync(AccountTitleId, token);

        public Task<bool> ExistCreditAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistCreditAccountTitleAsync(AccountTitleId, token);
        public Task<bool> ExistStaffAsync(int StaffId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistStaffAsync(StaffId, token);

        public Task<bool> ExistCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistCollectCategoryAsync(CategoryId, token);

        public Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistDepartmentAsync(DepartmentId, token);

        public Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistCustomerAsync(CustomerId, token);

        public Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistCurrencyAsync(CurrencyId, token);

        public Task<bool> ExistBillingCategoryAsync(int CategoryId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistBillingCategoryAsync(CategoryId, token);

        public Task<bool> ExistDestinationAsync(int DestinationId, CancellationToken token = default(CancellationToken))
            => billingExistsQueryProcessor.ExistDestinationAsync(DestinationId, token);



        public Task<IEnumerable<Billing>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken))
            => billingGetByIdsQueryProcessor.GetByIdsAsync(ids, token);

        public Task<IEnumerable<Billing>> GetByBillingInputIdAsync(long BillingInputId, CancellationToken token = default(CancellationToken))
            => billingQueryProcessor.GetByBillingInputIdAsync(BillingInputId, token);


        public Task<IEnumerable<int>> BillingImportDuplicationCheckAsync(int CompanyId, IEnumerable<BillingImportDuplicationWithCode> BillingImportDuplication,
            IEnumerable<ImporterSettingDetail> ImporterSettingDetail, CancellationToken token = default(CancellationToken))
            => billingQueryProcessor.BillingImportDuplicationCheckAsync(CompanyId, BillingImportDuplication, ImporterSettingDetail, token);

        public Task<Billing> UpdateForResetInvoiceCodeAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken))
          => billingQueryProcessor.UpdateForResetInvoiceCodeAsync(BillingInputIds, token);

        public Task<Billing> UpdateForResetInputIdAsync(IEnumerable<long> BillingInputIds, CancellationToken token = default(CancellationToken))
          => billingQueryProcessor.UpdateForResetInputIdAsync(BillingInputIds, token);
        public Task<IEnumerable<Billing>> UpdateBillingForPublishAsync(BillingInvoiceForPublish billingInvoiceForPublish, bool doUpdateInvoiceCode, CancellationToken token = default(CancellationToken))
            => billingQueryProcessor.UpdateForPublishAsync(billingInvoiceForPublish, doUpdateInvoiceCode, token);

        private async Task<int> DeleteInnerAsync(long billingId, CancellationToken token)
        {
            var current = (await billingGetByIdsQueryProcessor.GetByIdsAsync(new[] { billingId }, token)).FirstOrDefault();
            if (current == null) throw new Exception("請求データが存在しないため、削除できません。");
            if (current.AssignmentFlag > 0
                || current.OutputAt != null
                || current.InputType == 3
                || current.ResultCode == 0)
                throw new Exception("削除不可能な請求データです。");

            return await deleteBillingQueryProcessor.DeleteAsync(billingId, token);
        }


        public async Task<int> DeleteAsync(BillingDeleteSource source, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            var app = await getApplicationControlByCompanyQueryProcessor.GetAsync(source.CompanyId, token);
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var id in source.Ids)
                {
                    if (app.RegisterContractInAdvance == 1)
                        await updateBillingDivisionContractQueryProcessor.UpdateWithBillingIdAsync(id, source.LoginUserId, token);
                    else
                        await deleteBillingDivisionContractQueryProcessor.DeleteWithBillingIdAsync(id, token);

                    if (app.UseDiscount == 1)
                        await deleteBillingDiscountQueryProcessor.DeleteAsync(id, token);

                    result += await DeleteInnerAsync(id, token);
                }
                scope.Complete();
            }
            return result;
        }

        public async Task<CountResult> OmitAsync(OmitSource source, CancellationToken token = default(CancellationToken))
        {
            var result = new CountResult { ProcessResult = new ProcessResult() };
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var transaction in source.Transactions)
                {
                    var omitResult = await billingQueryProcessor.OmitAsync(source.DoDelete ? 1 : 0, source.LoginUserId, transaction);
                    if (omitResult == 0)
                    {
                        result.ProcessResult.ErrorCode = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated;
                        return result;
                    }
                    result.Count += omitResult;
                }
                result.ProcessResult.Result = true;
                scope.Complete();
            }
            return result;
        }
    }
}
