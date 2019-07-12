using NLog;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class BillingService : IBillingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IBillingProcessor billingProcessor;
        private readonly IBillingSaveProcessor billingSaveProcessor;
        private readonly IBillingSaveForInputProcessor billingSaveForInputProcessor;
        private readonly IBillingJournalizingProcessor billingJournalizingProcessor;
        private readonly IBillingSearchProcessor billingSearchProcessor;
        private readonly IBillingMemoProcessor billingMemoProcessor;
        private readonly IBillingDueAtModifyProcessor billingDueAtModifyProcessor;
        private readonly IBillingDiscountProcessor billingDiscountProcessor;
        private readonly IBillingImporterProcessor billingImporterProcessor;
        private readonly IBillingScheduledPaymentProcessor billingScheduledPaymentProcessor;
        private readonly IBillingAccountTransferProcessor billingAccountTransferProcessor;
        private readonly ILogger logger;

        public BillingService(
            IAuthorizationProcessor authorizationProcessor,
            IBillingProcessor billingProcessor,
            IBillingSaveProcessor billingSaveProcessor,
            IBillingSaveForInputProcessor billingSaveForInputProcessor,
            IBillingJournalizingProcessor billingJournalizingProcessor,
            IBillingSearchProcessor billingSearchProcessor,
            IBillingMemoProcessor billingMemoProcessor,
            IBillingDueAtModifyProcessor billingDueAtModifyProcessor,
            IBillingDiscountProcessor billingDiscountProcessor,
            IBillingImporterProcessor billingImporterProcessor,
            IBillingScheduledPaymentProcessor billingScheduledPaymentProcessor,
            IBillingAccountTransferProcessor billingAccountTransferProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.billingProcessor = billingProcessor;
            this.billingSaveProcessor = billingSaveProcessor;
            this.billingSaveForInputProcessor = billingSaveForInputProcessor;
            this.billingJournalizingProcessor = billingJournalizingProcessor;
            this.billingSearchProcessor = billingSearchProcessor;
            this.billingMemoProcessor = billingMemoProcessor;
            this.billingDueAtModifyProcessor = billingDueAtModifyProcessor;
            this.billingDiscountProcessor = billingDiscountProcessor;
            this.billingImporterProcessor = billingImporterProcessor;
            this.billingScheduledPaymentProcessor = billingScheduledPaymentProcessor;
            this.billingAccountTransferProcessor = billingAccountTransferProcessor;
            logger = logManager.GetLogger(typeof(BillingService));
        }

        #region create (crud の順)

        public async Task<BillingsResult> SaveAsync(string SessionKey, Billing[] billings)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingSaveProcessor.SaveItemsAsync(billings, token)).ToList();
                return new BillingsResult() { Billings = result, ProcessResult = new ProcessResult() { Result = true } };
            }, logger);
        }

        public async Task<BillingsResult> SaveForInputAsync(string SessionKey, Billing[] billings)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = (await billingSaveForInputProcessor.SaveAsync(billings, token)).ToList();
                return new BillingsResult { Billings = result, ProcessResult = new ProcessResult { Result = true } };
            }, logger);
        }

        public async Task<CountResult> SaveDiscountAsync(string SessionKey, Web.Models.BillingDiscount BillingDiscount)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingDiscountProcessor.SaveAsync(BillingDiscount, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<CountResult> SaveMemoAsync(string SessionKey, long Id, string Memo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingMemoProcessor.SaveMemoAsync(Id, Memo, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        #endregion

        #region read

        public async Task<BillingResult> GetAsync(string SessionKey, long[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingProcessor.GetByIdsAsync(Id, token)).ToList();
                return new BillingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billing = result.ToArray(),
                };
            }, logger);
        }

        public async Task<BillingDiscountsResult> GetDiscountAsync(string SessionKey, long BillingId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingDiscountProcessor.GetAsync(BillingId, token)).ToList();
                return new BillingDiscountsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingDiscounts = result,
                };
            }, logger);
        }

        public async Task<BillingsResult> GetItemsAsync(string SessionKey, BillingSearch BillingSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingSearchProcessor.GetAsync(BillingSearch, token)).ToList();
                return new BillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = result,
                };
            }, logger);
        }

        public async Task<BillMemoResult> GetMemoAsync(string SessionKey, long Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingMemoProcessor.GetMemoAsync(Id, token);
                return new BillMemoResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingMemo = result,
                };
            }, logger);
        }

        #region exist check

        public async Task<ExistResult> ExistAccountTitleAsync(string SessionKey, int AccountTitleId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistDebitAccountTitleAsync(AccountTitleId, token) ||
                    await billingProcessor.ExistCreditAccountTitleAsync(AccountTitleId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCollectCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistCategoryAsync(CategoryId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistCustomerAsync(CustomerId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistBillingCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistBillingCategoryAsync(CategoryId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistStaffAsync(string SessionKey, int StaffId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistStaffAsync(StaffId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCurrencyAsync(string SessionKey, int CurrencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistCurrencyAsync(CurrencyId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistDepartmentAsync(DepartmentId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistDestinationAsync(string SessionKey, int DestinationId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingProcessor.ExistDestinationAsync(DestinationId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        #endregion

        #endregion

        #region update

        public async Task<CountResult> OmitAsync(string SessionKey, int doDelete, int loginUserId, Transaction[] transactions)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token
                => await billingProcessor.OmitAsync(new OmitSource {
                    DoDelete        = doDelete == 1,
                    LoginUserId     = loginUserId,
                    Transactions    = transactions,
                }, token), logger);
        }

        public async Task<BillingsResult> InputScheduledPaymentAsync(string sessionKey, Billing[] billings)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = (await billingScheduledPaymentProcessor.SaveAsync(billings, token)).ToList();
                return new BillingsResult {
                    Billings = result,
                    ProcessResult = new ProcessResult { Result = true }
                };
            }, logger);
        }

        #endregion

        #region delete

        public async Task<CountResult> DeleteAsync(string SessionKey, long[] Id, int UseLongTermAdvanceReceived,
            int RegisterContractInAdvance, int UseDiscount, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var companyId = (await billingProcessor.GetByIdsAsync(new[] { Id.First() }, token)).First().CompanyId;
                var source = new BillingDeleteSource {
                    Ids             = Id,
                    CompanyId       = companyId,
                    LoginUserId     = LoginUserId,
                };
                var result = await billingProcessor.DeleteAsync(source, token);
                return new CountResult() { Count = result, ProcessResult = new ProcessResult() { Result = true } };
            }, logger);
        }

        public async Task<CountResult> DeleteByInputIdAsync(string SessionKey, long InputId, int UseLongTermAdvanceReceived,
            int RegisterContractInAdvance, int UseDiscount, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var billings = (await billingProcessor.GetByBillingInputIdAsync(InputId)).ToArray();
                var source = new BillingDeleteSource {
                    Ids         = billings.Select(x => x.Id).ToArray(),
                    CompanyId   = billings.First().CompanyId,
                    LoginUserId = LoginUserId,
                };

                var result = await billingProcessor.DeleteAsync(source, token);
                return new CountResult() { Count = result, ProcessResult = new ProcessResult() { Result = true } };
            }, logger);
        }

        public async Task<CountResult> DeleteDiscountAsync(string SessionKey, long BillingId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingDiscountProcessor.DeleteAsync(BillingId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteMemoAsync(string SessionKey, long Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingMemoProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        #endregion

        #region dueat modify

        public async Task<BillingDueAtModifyResults> GetDueAtModifyItemsAsync(string sessionKey, BillingSearch option)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var billings = (await billingDueAtModifyProcessor.GetAsync(option, token)).ToList();
                return new BillingDueAtModifyResults
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = billings,
                };
            }, logger);

        public async Task<BillingsResult> UpdateDueAtAsync(string sessionKey, BillingDueAtModify[] billings)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token => {
                var results = (await billingDueAtModifyProcessor.UpdateAsync(billings, token)).ToList();
                return new BillingsResult {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = results,
                };
            }, logger);


        #endregion

        #region import

        public async Task<ScheduledPaymentImportResult> ImportScheduledPaymentAsync(string SessionKey, int CompanyId, int LoginUserId,
            int ImporterSettingId, ScheduledPaymentImport[] SchedulePayment)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingScheduledPaymentProcessor.ImportAsync(new BillingScheduledPaymentImportSource {
                    CompanyId           = CompanyId,
                    LoginUserId         = LoginUserId,
                    ImporterSettingId   = ImporterSettingId,
                    Items               = SchedulePayment,
                }, token)).ToArray();
                return new ScheduledPaymentImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ScheduledPaymentImport = result,
                };
            }, logger);
        }

        public async Task<BillingsResult> GetItemsForScheduledPaymentImportAsync(string SessionKey, int CompanyId, ScheduledPaymentImport[] SchedulePayment,
            ImporterSettingDetail[] details)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var billing = (await billingScheduledPaymentProcessor.GetAsync(new BillingScheduledPaymentImportSource {
                    CompanyId       = CompanyId,
                    Details         = details,
                    Items           = SchedulePayment,
                }, token)).ToList();

                return new BillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = billing,
                };
            }, logger);
        }

        public async Task<BillingImportResult> ImportAsync(string SessionKey, int CompanyId, int LoginUserId, int ImporterSettingId, BillingImport[] billingImport)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingImporterProcessor.ImportAsync(new BillingImportItems {
                        CompanyId           = CompanyId,
                        LoginUserId         = LoginUserId,
                        ImporterSettingId   = ImporterSettingId,
                        Items               = billingImport,
                    }, token);
                return result;
            }, logger);
        }

        public async Task<BillingImportDuplicationResult> BillingImportDuplicationCheckAsync(string SessionKey, int CompanyId, BillingImportDuplicationWithCode[] BillingImportDuplication,
            ImporterSettingDetail[] ImporterSettingDetail)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingProcessor.BillingImportDuplicationCheckAsync(CompanyId, BillingImportDuplication, ImporterSettingDetail, token)).ToArray();
                return new BillingImportDuplicationResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    RowNumbers = result,
                };
            }, logger);
        }

        #endregion

        #region account transfer

        public async Task<BillingsResult> GetAccountTransferMatchingTargetListAsync(string sessionKey, int companyId, int paymentAgencyId, DateTime transferDate)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = (await billingAccountTransferProcessor.GetAsync(companyId, paymentAgencyId, transferDate, token)).ToList();

                return new BillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = result,
                };
            }, logger);
        }

        public async Task<AccountTransferImportResult> ImportAccountTransferResultAsync(string sessionKey, AccountTransferImportData[] importDataList, int? dueDateOffset, int? collectCategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                foreach (var data in importDataList)
                {
                    data.DueDateOffset      = dueDateOffset;
                    data.CollectCategoryId  = collectCategoryId;
                }

                await billingAccountTransferProcessor.ImportAsync(importDataList, token);
                return new AccountTransferImportResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);
        }

        #endregion

        #region journalizing

        public async Task<JournalizingSummariesResult> GetBillingJournalizingSummaryAsync(string SessionKey, int OutputFlg, int CompanyId, int CurrencyId,
            DateTime? BilledAtFrom, DateTime? BilledAtTo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingJournalizingProcessor.GetSummaryAsync(new BillingJournalizingOption {
                    CompanyId       = CompanyId,
                    CurrencyId      = CurrencyId,
                    IsOutuptted     = OutputFlg == 0,
                    BilledAtFrom    = BilledAtFrom,
                    BilledAtTo      = BilledAtTo,
                }, token)).ToList();
                return new JournalizingSummariesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    JournalizingsSummaries = result,
                };
            }, logger);
        }

        public async Task<BillingJournalizingsResult> ExtractBillingJournalizingAsync(string SessionKey, int CompanyId, DateTime? BilledAtFrom, DateTime? BilledAtTo,
            int CurrencyId, DateTime[] OutputAt)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingJournalizingProcessor.ExtractAsync(new BillingJournalizingOption {
                    CompanyId       = CompanyId,
                    CurrencyId      = CurrencyId,
                    BilledAtFrom    = BilledAtFrom,
                    BilledAtTo      = BilledAtTo,
                    OutputAt        = OutputAt ?? new DateTime[] { },
                    IsOutuptted     = (OutputAt?.Any() ?? false),
                }, token)).ToList();
                return new BillingJournalizingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingJournalizings = result,
                };
            }, logger);
        }

        public async Task<BillingsResult> UpdateOutputAtAsync(string SessionKey, int CompanyId, DateTime? BilledAtFrom, DateTime? BilledAtTo, int CurrencyId, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingJournalizingProcessor.UpdateAsync(new BillingJournalizingOption {
                    CompanyId       = CompanyId,
                    BilledAtFrom    = BilledAtFrom,
                    BilledAtTo      = BilledAtTo,
                    CurrencyId      = CurrencyId,
                    LoginUserId     = LoginUserId,
                }, token)).ToList();
                return new BillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = result,
                };
            }, logger);
        }

        public async Task<BillingsResult> CancelBillingJournalizingAsync(string SessionKey, int CompanyId, int CurrencyId, DateTime[] OutputAt, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await billingJournalizingProcessor.CancelAsync(new BillingJournalizingOption {
                    CompanyId       = CompanyId,
                    CurrencyId      = CurrencyId,
                    OutputAt        = OutputAt ?? new DateTime[] { },
                    LoginUserId     = LoginUserId,
                }, token)).ToList();
                return new BillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = result,
                };
            }, logger);
        }

        #endregion

    }
}
