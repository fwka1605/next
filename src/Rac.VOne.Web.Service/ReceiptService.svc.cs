using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IReceiptHeaderProcessor receiptHeaderProcessor;
        private readonly IReceiptProcessor receiptProcessor;
        private readonly IReceiptApportionProcessor receiptApportionProcessor;
        private readonly IReceiptSearchProcessor receiptSearchProcessor;
        private readonly IReceiptExcludeProcessor receiptExcludeProcessor;
        private readonly IReceiptMemoProcessor receiptMemoProcessor;
        private readonly IReceiptJournalizingProcessor receiptJournalizingProcessor;
        private readonly IReceiptSectionTransferProcessor receiptSectionTransferProcessor;
        private readonly IAdvanceReceivedProcessor advanceReceivedProcessor;
        private readonly IAdvanceReceivedSplitProcessor advanceReceivedSplitProcessor;

        private readonly ILogger logger;

        public ReceiptService(IAuthorizationProcessor authorizationProcessor,
            IReceiptProcessor receiptProcessor,
            IReceiptApportionProcessor receiptApportionProcessor,
            IReceiptSearchProcessor receiptSearchProcessor,
            IReceiptHeaderProcessor receiptHeaderProcessor,
            IReceiptExcludeProcessor receiptExcludeProcessor,
            IReceiptMemoProcessor receiptMemoProcessor,
            IReceiptJournalizingProcessor receiptJournalizingProcessor,
            IReceiptSectionTransferProcessor receiptSectionTransferProcessor,
            IAdvanceReceivedProcessor advanceReceivedProcessor,
            IAdvanceReceivedSplitProcessor advanceReceivedSplitProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.receiptHeaderProcessor = receiptHeaderProcessor;
            this.receiptProcessor = receiptProcessor;
            this.receiptApportionProcessor = receiptApportionProcessor;
            this.receiptSearchProcessor = receiptSearchProcessor;
            this.receiptExcludeProcessor = receiptExcludeProcessor;
            this.receiptMemoProcessor = receiptMemoProcessor;
            this.receiptJournalizingProcessor = receiptJournalizingProcessor;
            this.receiptSectionTransferProcessor = receiptSectionTransferProcessor;
            this.advanceReceivedProcessor = advanceReceivedProcessor;
            this.advanceReceivedSplitProcessor = advanceReceivedSplitProcessor;
            logger = logManager.GetLogger(typeof(ReceiptService));
        }

        public async Task<ReceiptInputsResult> SaveAsync(string SessionKey, ReceiptInput[] ReceiptInput, byte[] ClientKey, int ParentCustomerId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token
                => await receiptProcessor.SaveAsync(new ReceiptSaveItem {
                    Receipts            = ReceiptInput,
                    ClientKey           = ClientKey,
                    ParentCustomerId    = ParentCustomerId == 0 ? (int?)null : ParentCustomerId,
                }, token), logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, long Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => {
                var result = await receiptProcessor.DeleteAsync(Id, token);
                return new CountResult {
                    ProcessResult       = new ProcessResult { Result = true },
                    Count               = result,
                };
            }, logger);
        }

        public async Task<ReceiptsResult> GetAsync(string SessionKey, long[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptProcessor.GetByIdsAsync(Id, token)).ToList();
                return new ReceiptsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Receipts = result,
                };
            }, logger);
        }

        public async Task<ReceiptsResult> GetItemsAsync(string SessionKey, ReceiptSearch ReceiptSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptSearchProcessor.GetAsync(ReceiptSearch, token)).ToList();
                return new ReceiptsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Receipts = result,
                };
            }, logger);
        }

        public async Task<AdvanceReceiptsResult> GetAdvanceReceiptsAsync(string SessionKey, long originalReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var originalReceipt = await receiptProcessor.GetReceiptAsync(originalReceiptId, token);
                var advanceReceipts = (await advanceReceivedProcessor.GetAdvanceReceiptsAsync(originalReceiptId, token)).ToList();

                return new AdvanceReceiptsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    OriginalReceipt = originalReceipt,
                    AdvanceReceipts = advanceReceipts,
                };
            }, logger);
        }

        public async Task<ReceiptExcludeResult> SaveExcludeAmountAsync(string SessionKey, ReceiptExclude[] RecExc)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return await receiptExcludeProcessor.SaveAsync(RecExc, token);
            }, logger);
        }

        public async Task<ReceiptResult> SaveMemoAsync(string SessionKey, long ReceiptId, string Memo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await receiptMemoProcessor.SaveAsync(ReceiptId, Memo, token);
                return new ReceiptResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);
        }

        public async Task<ReceiptResult> DeleteMemoAsync(string SessionKey, long ReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await receiptMemoProcessor.DeleteAsync(ReceiptId, token);
                return new ReceiptResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);
        }

        public async Task<ReceiptMemoResult> GetMemoAsync(string SessionKey, long ReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptMemoProcessor.GetAsync(ReceiptId, token))?.Memo;
                return new ReceiptMemoResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptMemo = result,
                };
            }, logger);
        }

        public async Task<AdvanceReceivedResult> SaveAdvanceReceivedAsync(string SessionKey, AdvanceReceived[] ReceiptInfo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token
                => await advanceReceivedProcessor.SaveAsync(ReceiptInfo, token), logger);
        }

        public async Task<AdvanceReceivedResult> CancelAdvanceReceivedAsync(string SessionKey, AdvanceReceived[] ReceiptInfo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token
                => await advanceReceivedProcessor.CancelAsync(ReceiptInfo, token), logger);
        }

        public async Task<ReceiptHeadersResult> GetHeaderItemsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptHeaderProcessor.GetItemsAsync(CompanyId, token)).ToList();

                return new ReceiptHeadersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptHeaders = result,
                };
            }, logger);
        }

        public async Task<ReceiptApportionsResult> GetApportionItemsAsync(string SessionKey, long[] receiptHeaderId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptApportionProcessor.GetAsync(receiptHeaderId, token)).ToList();
                return new ReceiptApportionsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptApportion = result,
                };
            }, logger);
        }

        public async Task<ReceiptApportionsResult> ApportionAsync(string SessionKey, ReceiptApportion[] receiptList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token
                => await receiptApportionProcessor.ApportionAsync(receiptList, token), logger);
        }

        public async Task<ExistResult> ExistCurrencyAsync(string SessionKey, int CurrencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistCurrencyAsync(CurrencyId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistReceiptCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistReceiptCategoryAsync(CategoryId, token);

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
                var result = await receiptProcessor.ExistCustomerAsync(CustomerId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistSectionAsync(SectionId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCompanyAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistCompanyAsync(CompanyId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistExcludeCategoryAsync(string SessionKey, int CategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptExcludeProcessor.ExistExcludeCategoryAsync(CategoryId, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistOriginalReceiptAsync(string SessionKey, long ReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistOriginalReceiptAsync(ReceiptId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistNonApportionedReceiptAsync(string SessionKey,
            int CompanyId,
            DateTime ClosingFrom,
            DateTime ClosingTo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistNonApportionedAsync(CompanyId, ClosingFrom, ClosingTo, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistNonOutputedReceiptAsync(string SessionKey,
          int CompanyId,
          DateTime ClosingFrom,
          DateTime ClosingTo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistNonOutputedAsync(CompanyId, ClosingFrom, ClosingTo, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistNonAssignmentReceiptAsync(string SessionKey,
           int CompanyId,
           DateTime ClosingFrom,
           DateTime ClosingTo)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptProcessor.ExistNonAssignmentAsync(CompanyId, ClosingFrom, ClosingTo, token);

                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<CountResult> OmitAsync(string sessionKey, int doDelete, int loginUserId, Transaction[] transactions)
            => await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token
                => await receiptProcessor.OmitAsync(new OmitSource {
                    DoDelete        = doDelete == 1,
                    LoginUserId     = loginUserId,
                    Transactions    = transactions,
                }, token), logger);



        public async Task<ReceiptSectionTransfersResult> SaveReceiptSectionTransferAsync(string SessionKey, ReceiptSectionTransfer[] ReceiptSectionTransfer, int LoginUserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token
                => await receiptSectionTransferProcessor.SaveAsync(ReceiptSectionTransfer, token), logger);
        }

        public async Task<ReceiptSectionTransfersResult> GetReceiptSectionTransferForPrintAsync(string SessionKey,int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptSectionTransferProcessor.GetReceiptSectionTransferForPrintAsync(CompanyId, token)).ToList();
                return new ReceiptSectionTransfersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptSectionTransfers = result,
                };
            }, logger);
        }

        public async Task<ReceiptSectionTransfersResult> UpdateReceiptSectionTransferPrintFlagAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptSectionTransferProcessor.UpdateReceiptSectionTransferPrintFlagAsync(CompanyId, token)).ToList();
                return new ReceiptSectionTransfersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptSectionTransfers = result,
                };
            }, logger);
        }

        public async Task<ReceiptImportDuplicationResult> ReceiptImportDuplicationCheckAsync(string SessionKey, int CompanyId, ReceiptImportDuplication[] ReceiptImportDuplication, ImporterSettingDetail[] ImporterSettingDetail)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await receiptProcessor.ReceiptImportDuplicationCheckAsync(CompanyId, ReceiptImportDuplication, ImporterSettingDetail, token)).ToArray();
                return new ReceiptImportDuplicationResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    RowNumbers = result,
                };
            }, logger);
        }

        public async Task<VoidResult> AdvanceReceivedDataSplitAsync(string SessionKey, int CompanyId, int LoginUserId, int CurrencyId, long OriginalReceiptId, AdvanceReceivedSplit[] AdvanceReceivedSplitList)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token => 
            {
                await advanceReceivedSplitProcessor.SplitAsync(new AdvanceReceivedSplitSource {
                    CompanyId           = CompanyId,
                    CurrencyId          = CurrencyId,
                    LoginUserId         = LoginUserId,
                    OriginalReceiptId   = OriginalReceiptId,
                    Items               = AdvanceReceivedSplitList,
                }, token);
                return new VoidResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);
        }

        public async Task<VoidResult> CancelAdvanceReceivedDataSplitAsync(string SessionKey, int CompanyId, int LoginUserId, int CurrencyId, long OriginalReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await advanceReceivedSplitProcessor.CancelAsync(new AdvanceReceivedSplitSource {
                    CompanyId = CompanyId,
                    CurrencyId = CurrencyId,
                    LoginUserId = LoginUserId,
                    OriginalReceiptId = OriginalReceiptId,
                }, token);
                return new VoidResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);
        }

        #region journalizing

        public async Task<JournalizingSummariesResult> GetReceiptJournalizingSummaryAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptJournalizingProcessor.GetSummaryAsync(option, token);
                return new JournalizingSummariesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    JournalizingsSummaries = result.ToList(),
                };
            }, logger);

        public async Task<ReceiptJournalizingsResult> ExtractReceiptJournalizingAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptJournalizingProcessor.ExtractAsync(option, token);
                return new ReceiptJournalizingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ReceiptJournalizings = result.ToList(),
                };
            }, logger);

        public async Task<GeneralJournalizingsResult> ExtractReceiptGeneralJournalizingAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await receiptJournalizingProcessor.ExtractGeneralAsync(option, token);
                return new GeneralJournalizingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GeneralJournalizings = result.ToList(),
                };
            }, logger);

        public async Task<CountResult> UpdateOutputAtAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var count = await receiptJournalizingProcessor.UpdateOutputAtAsync(option, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = count,
                };
            }, logger);

        public async Task<CountResult> CancelReceiptJournalizingAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var count = await receiptJournalizingProcessor.CancelJournalizingAsync(option, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = count,
                };
            }, logger);

        #endregion
    }
}