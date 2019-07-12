using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ICustomerMaster
    {
        [OperationContract]
        Task<CustomerResult> SaveAsync(string SessionKey, Customer Customer);

        [OperationContract]
        Task<CustomersResult> SaveItemsAsync(string SessionKey, Customer[] Customer);

        [OperationContract]
        Task<CustomersResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<CustomersResult> GetItemsWithAsync(int CompanyId, string SessionKey);

        [OperationContract]
        Task<CustomerPaymentContractResult> SavePaymentContractAsync(string SessionKey, CustomerPaymentContract CustomerPaymentContract);

        [OperationContract]
        Task<CustomerDiscountResult> SaveDiscountAsync(string SessionKey, CustomerDiscount CustomerDiscount);

        [OperationContract]
        Task<CountResult> DeletePaymentContractAsync(string SessionKey, int CustomerId);

        [OperationContract]
        Task<CustomerPaymentContractsResult> GetPaymentContractAsync(string SessionKey, int[] CustomerId);

        [OperationContract]
        Task<CustomerDiscountsResult> GetDiscountAsync(string SessionKey, int customerId);

        [OperationContract]
        Task<CustomersResult> GetParentItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CustomersResult> GetCustomerDetailItemsAsync(int CompanyId, string Code, string Name, int ShareTransferFee,
            int ClosingDay, string SessionKey);

        [OperationContract]
        Task<CustomersResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<CustomersResult> GetItemsAsync(string Sessionkey, int CompanyId, CustomerSearch CustomerSearch);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CustomerId);

        [OperationContract]
        Task<CustomersResult> GetByChildDetailsAsync(string SessionKey, int CompanyId, int ParentId);

        [OperationContract]
        Task<CountResult> DeleteDiscountAsync(string SessionKey, int CustomerId, int Sequence);

        [OperationContract]
        Task<CustomerDiscountsResult> GetDiscountItemsAsync(string SessionKey, CustomerSearch CustomerSearch);

        [OperationContract]
        Task<ExistResult> ExistCategoryAsync(string SessionKey, int CategoryId);

        [OperationContract]
        Task<ExistResult> ExistStaffAsync(string SessionKey, int StaffId);

        [OperationContract]
        Task<ExistResult> ExistCompanyAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CustomersResult> GetCustomerGroupAsync(string SessionKey, int CompanyId, int ParentId);

        [OperationContract]
        Task<CustomersResult> GetCustomerWithListAsync(string SessionKey, int CompanyId, int[] CusId);

        [OperationContract]
        Task<CustomerResult> GetTopCustomerAsync(string SessionKey, Customer Customer);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForCustomerGroupParentAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForCustomerGroupChildAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForKanaHistoryAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForBillingAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForReceiptAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForNettingAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
                Customer[] InsertList, Customer[] UpdateList, Customer[] DeleteList);

        [OperationContract] Task<CustomerMinsResult> GetMinItemsAsync(string SessionKey, int CompanyId);
    }
}
