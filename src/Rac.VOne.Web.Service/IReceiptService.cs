using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IReceiptService
    {
        [OperationContract]
        Task<ReceiptInputsResult> SaveAsync(string SessionKey, ReceiptInput[] ReceiptInput, byte[] ClientKey, int ParentCustomerId);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, long Id);

        [OperationContract]
        Task<ReceiptsResult> GetAsync(string SessionKey, long[] Id);

        [OperationContract]
        Task<ReceiptsResult> GetItemsAsync(string SessionKey, ReceiptSearch ReceiptSearch);

        [OperationContract]
        Task<AdvanceReceiptsResult> GetAdvanceReceiptsAsync(string SessionKey, long originalReceiptId);

        [OperationContract]
        Task<ReceiptExcludeResult> SaveExcludeAmountAsync(string SessionKey, Models.ReceiptExclude[] RecExc);

        [OperationContract]
        Task<ReceiptResult> SaveMemoAsync(string SessionKey, long ReceiptId, string Memo);

        [OperationContract]
        Task<ReceiptResult> DeleteMemoAsync(string SessionKey, long ReceiptId);

        [OperationContract]
        Task<ReceiptMemoResult> GetMemoAsync(string SessionKey, long ReceiptId);

        [OperationContract]
        Task<AdvanceReceivedResult> SaveAdvanceReceivedAsync(string SessionKey, AdvanceReceived[] ReceiptInfo);

        [OperationContract]
        Task<AdvanceReceivedResult> CancelAdvanceReceivedAsync(string SessionKey, AdvanceReceived[] ReceiptInfo);

        [OperationContract]
        Task<ReceiptHeadersResult> GetHeaderItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReceiptApportionsResult> GetApportionItemsAsync(string SessionKey, long[] receiptHeaderId);

        [OperationContract]
        Task<ReceiptApportionsResult> ApportionAsync(string SessionKey, ReceiptApportion[] receiptList);

        [OperationContract]
        Task<ExistResult> ExistCurrencyAsync(string SessionKey, int CurrencyId);

        [OperationContract]
        Task<ExistResult> ExistReceiptCategoryAsync(string SessionKey, int CategoryId);

        [OperationContract]
        Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId);

        [OperationContract]
        Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId);

        [OperationContract]
        Task<ExistResult> ExistCompanyAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ExistResult> ExistExcludeCategoryAsync(string SessionKey, int CategoryId);

        [OperationContract]
        Task<ExistResult> ExistOriginalReceiptAsync(string SessionKey, long ReceiptId);

        [OperationContract]
        Task<ExistResult> ExistNonApportionedReceiptAsync(string SessionKey,
            int CompanyId,
            DateTime ClosingFrom,
            DateTime ClosingTo);

        [OperationContract]
        Task<ExistResult> ExistNonOutputedReceiptAsync(string SessionKey,
           int CompanyId,
           DateTime ClosingFrom,
           DateTime ClosingTo);

        [OperationContract]
        Task<ExistResult> ExistNonAssignmentReceiptAsync(string SessionKey,
            int CompanyId,
            DateTime ClosingFrom,
            DateTime ClosingTo);

        [OperationContract]
        Task<CountResult> OmitAsync(string sessionKey, int doDelete, int loginUserId, Transaction[] transactions);

        [OperationContract]
        Task<ReceiptSectionTransfersResult> SaveReceiptSectionTransferAsync(string SessionKey, ReceiptSectionTransfer[] ReceiptSectionTransfer, int LoginUserId);

        [OperationContract]
        Task<ReceiptSectionTransfersResult> GetReceiptSectionTransferForPrintAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReceiptSectionTransfersResult> UpdateReceiptSectionTransferPrintFlagAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReceiptImportDuplicationResult> ReceiptImportDuplicationCheckAsync(string SessionKey, int CompanyId, ReceiptImportDuplication[] ReceiptImportDuplication, ImporterSettingDetail[] ImporterSettingDetail);

        [OperationContract]
        Task<VoidResult> AdvanceReceivedDataSplitAsync(string SessionKey, int CompanyId, int LoginUserId, int CurrencyId, long OriginalReceiptId, AdvanceReceivedSplit[] AdvanceReceivedSplitList);

        [OperationContract]
        Task<VoidResult> CancelAdvanceReceivedDataSplitAsync(string SessionKey, int CompanyId, int LoginUserId, int CurrencyId, long OriginalReceiptId);

        [OperationContract]
        Task<JournalizingSummariesResult> GetReceiptJournalizingSummaryAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<ReceiptJournalizingsResult> ExtractReceiptJournalizingAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<GeneralJournalizingsResult> ExtractReceiptGeneralJournalizingAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<CountResult> UpdateOutputAtAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<CountResult> CancelReceiptJournalizingAsync(string SessionKey, JournalizingOption option);

    }
}
