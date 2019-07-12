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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IMatchingService" を変更できます。
    [ServiceContract]
    public interface IMatchingService
    {
        [OperationContract]
        Task<CollationsResult> CollateAsync(string SessionKey, CollationSearch CollationSearch, string connectionId);

        [OperationContract]
        Task<MatchingResult> SequentialMatchingAsync(string SessionKey, Collation[] Collations, CollationSearch CollationSearch, string connectionId);

        [OperationContract]
        Task<MatchingHeadersResult> SearchMatchedDataAsync(string SessionKey, CollationSearch CollationSearch, string connectionId);

        [OperationContract]
        Task<MatchingResult> CancelMatchingAsync(string SessionKey, MatchingHeader[] MatchingHeader, int LoginUserId, string connectionId);

        [OperationContract]
        Task<BillingsResult> SearchBillingDataAsync(string SessionKey, MatchingBillingSearch MatchingBillingSearch);

        [OperationContract]
        Task<ReceiptsResult> SearchReceiptDataAsync(string SessionKey, MatchingReceiptSearch MatchingReceiptSearch);

        [OperationContract]
        Task<MatchingSourceResult> SolveAsync(string SessionKey, MatchingSource source, CollationSearch option);

        [OperationContract]
        Task<MatchingResult> MatchingIndividuallyAsync(string SessionKey, MatchingSource source);

        [OperationContract]
        Task<BillingIndicesResult> SimulateAsync(string SessionKey, Billing[] MatchingBilling, decimal SearchValue);

        [OperationContract]
        Task<MatchingHeadersResult> ApproveAsync(string SessionKey, MatchingHeader[] headers);

        [OperationContract]
        Task<MatchingHeadersResult> CancelApprovalAsync(string SessionKey, MatchingHeader[] headers);

        [OperationContract]
        Task<ReceiptsResult> searchReceiptByIdAsync(string SessionKey, long[] ReceiptId);

        [OperationContract]
        Task<MatchingsResult> GetAsync(string SessionKey, long[] Ids);

        [OperationContract]
        Task<MatchingHeadersResult> GetHeaderItemsAsync(string SessionKey, long[] Ids);

        [OperationContract]
        Task<MatchingJournalizingDetailsResult> GetMatchingJournalizingDetailAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<MatchingJournalizingProcessResult> CancelMatchingJournalizingDetailAsync(string SessionKey, MatchingJournalizingDetail[] MatchingJournalizingDetail);


        [OperationContract]
        Task<JournalizingSummariesResult> GetMatchingJournalizingSummaryAsync(string SessionKey, JournalizingOption option);
        [OperationContract]
        Task<MatchingJournalizingsResult> ExtractMatchingJournalizingAsync(string SessionKey, JournalizingOption option);
        [OperationContract]
        Task<GeneralJournalizingsResult> ExtractGeneralJournalizingAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<MatchedReceiptsResult> GetMatchedReceiptAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<MatchingJournalizingProcessResult> UpdateOutputAtAsync(string SessionKey, JournalizingOption option);
        [OperationContract]
        Task<MatchingJournalizingProcessResult> CancelMatchingJournalizingAsync(string SessionKey, JournalizingOption option);

        [OperationContract]
        Task<CountResult> SaveWorkDepartmentTargetAsync(string SessionKey, int CompanyId, byte[] ClientKey, int[] DepartmentIds);

        [OperationContract]
        Task<CountResult> SaveWorkSectionTargetAsync(string SessionKey, int CompanyId, byte[] ClientKey, int[] SectionIds);

        [OperationContract]
        Task<MatchingJournalizingsResult> MFExtractMatchingJournalizingAsync(string SessionKey, JournalizingOption option);
    }
}
