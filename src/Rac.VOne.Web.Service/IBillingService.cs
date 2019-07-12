using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IBillingService" を変更できます。
    [ServiceContract]
    public interface IBillingService
    {
        [OperationContract]
        Task<BillingsResult> SaveAsync(string SessionKey, Billing[] billings);

        [OperationContract]
        Task<BillingsResult> SaveForInputAsync(string SessionKey, Billing[] billings);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, long[] Id, int UseLongTermAdvanceReceived,
            int RegisterContractInAdvance, int UseDiscount, int LoginUserId);

        [OperationContract]
        Task<CountResult> DeleteByInputIdAsync(string SessionKey, long InputId, int UseLongTermAdvanceReceived,
            int RegisterContractInAdvance, int UseDiscount, int LoginUserId);

        [OperationContract]
        Task<BillingResult> GetAsync(string SessionKey, long[] Id);

        [OperationContract]
        Task<BillingsResult> GetItemsAsync(string SessionKey, BillingSearch BillingSearch);

        [OperationContract]
        Task<BillingsResult> InputScheduledPaymentAsync(string sessionKey, Billing[] billings);

        [OperationContract]
        Task<CountResult> SaveMemoAsync(string SessionKey, long Id, string Memo);

        [OperationContract]
        Task<CountResult> DeleteMemoAsync(string SessionKey, long Id);

        [OperationContract]
        Task<BillMemoResult> GetMemoAsync(string SessionKey, long Id);

        [OperationContract]
        Task<CountResult> SaveDiscountAsync(string SessionKey, BillingDiscount BillingDiscount);

        [OperationContract]
        Task<CountResult> DeleteDiscountAsync(string SessionKey, long BillingId);

        [OperationContract]
        Task<BillingDiscountsResult> GetDiscountAsync(string SessionKey, long BillingId);

        [OperationContract]
        Task<ExistResult> ExistAccountTitleAsync(string SessionKey, int AccountTitleId);

        [OperationContract]
        Task<ExistResult> ExistCollectCategoryAsync(string SessionKey, int CategoryId);

        [OperationContract]
        Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId);

        [OperationContract]
        Task<ExistResult> ExistBillingCategoryAsync(string SessionKey, int CategoryId);

        [OperationContract]
        Task<ExistResult> ExistStaffAsync(string SessionKey, int StaffId);

        [OperationContract]
        Task<ExistResult> ExistCurrencyAsync(string SessionKey, int CurrencyId);

        [OperationContract]
        Task<ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId);

        [OperationContract]
        Task<ExistResult> ExistDestinationAsync(string SessionKey, int DestinationId);

        [OperationContract]
        Task<ScheduledPaymentImportResult> ImportScheduledPaymentAsync(string SessionKey, int CompanyId, int LoginUserId, int ImporterSettingId, ScheduledPaymentImport[] SchedulePayment);


        [OperationContract]
        Task<CountResult> OmitAsync(string SessionKey, int doDelete, int loginUserId, Transaction[] transactions);

        [OperationContract]
        Task<BillingsResult> GetItemsForScheduledPaymentImportAsync(string SessionKey, int CompanyId, ScheduledPaymentImport[] SchedulePayment, ImporterSettingDetail[] details);

        [OperationContract]
        Task<BillingImportResult> ImportAsync(string SessionKey, int CompanyId, int LoginUserId, int ImporterSettingId, BillingImport[] billingImport);
        [OperationContract]
        Task<BillingImportDuplicationResult> BillingImportDuplicationCheckAsync(string SessionKey, int CompanyId, BillingImportDuplicationWithCode[] BillingImportDuplication, ImporterSettingDetail[] ImporterSettingDetail);

        //請求仕訳出力
        [OperationContract]
        Task<JournalizingSummariesResult> GetBillingJournalizingSummaryAsync(string SessionKey, int OutputFlg, int CompanyId, int CurrencyId, DateTime? BilledAtFrom, DateTime? BilledAtTo);
        [OperationContract]
        Task<BillingJournalizingsResult> ExtractBillingJournalizingAsync(string SessionKey, int CompanyId, DateTime? BilledAtFrom, DateTime? BilledAtTo, int CurrencyId, DateTime[] OutputAt);
        [OperationContract]
        Task<BillingsResult> UpdateOutputAtAsync(string SessionKey, int CompanyId, DateTime? BilledAtFrom, DateTime? BilledAtTo, int CurrencyId,int LoginUserId);
        [OperationContract]
        Task<BillingsResult> CancelBillingJournalizingAsync(string SessionKey, int CompanyId, int CurrencyId, DateTime[] OutputAt,int LoginUserId);

        // 口座振替結果データ取込
        [OperationContract]
        Task<BillingsResult> GetAccountTransferMatchingTargetListAsync(string sessionKey, int companyId, int paymentAgencyId, DateTime transferDate);
        [OperationContract]
        Task<AccountTransferImportResult> ImportAccountTransferResultAsync(string sessionKey, AccountTransferImportData[] importDataList, int? dueDateOffset, int? collectCategoryId);

        #region dueat modify

        [OperationContract]
        Task<BillingDueAtModifyResults> GetDueAtModifyItemsAsync(string sessionKey, BillingSearch option);

        [OperationContract]
        Task<BillingsResult> UpdateDueAtAsync(string sessionKey, BillingDueAtModify[] billings);

        #endregion
    }
}
