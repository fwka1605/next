using System.ServiceModel;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ICurrencyMaster
    {
        [OperationContract]
        Task<CurrenciesResult> GetAsync(string SessionKey, int[] CurrencyId);

        [OperationContract]
        Task<CurrenciesResult> GetItemsAsync(string SessionKey, int CompanyId, CurrencySearch CurrencySearch);

        [OperationContract]
        Task<CurrenciesResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<CurrencyResult> SaveAsync(string SessionKey, Currency Currency);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
             Currency[] InsertList, Currency[] UpdateList, Currency[] DeleteList);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsBillingAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsReceiptAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsNettingAsync(string SessionKey, int CompanyId, string[] Code);

    }
}

