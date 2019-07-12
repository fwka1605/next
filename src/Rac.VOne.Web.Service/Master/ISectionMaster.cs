using System.ServiceModel;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ISectionMaster
    {
        [OperationContract]
        Task<SectionsResult> GetByLoginUserIdAsync(string SessionKey, int LoginUserId);
        [OperationContract]
        Task<SectionsResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);
        [OperationContract]
        Task<SectionResults> SaveAsync(string SessionKey, Section Section);
        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);
        [OperationContract]
        Task<SectionsResult> GetItemsAsync(string SessionKey, int CompanyId, SectionSearch SectionSearch);
        [OperationContract]
        Task<SectionResult> GetByCustomerIdAsync(string SessionKey, int CustomerId);
        [OperationContract]
        Task<ImportResultSection> ImportAsync(string SessionKey,
                Section[] InsertList, Section[] UpdateList, Section[] DeleteList);
        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForBankAccountAsync(string SessionKey, int CompanyId, string[] Code);
        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForSectionWithDepartmentAsync(string SessionKey, int CompanyId, string[] Code);
        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForSectionWithLoginUserAsync(string SessionKey, int CompanyId, string[] Code);
        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForReceiptAsync(string SessionKey, int CompanyId, string[] Code);
        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForNettingAsync(string SessionKey, int CompanyId, string[] Code);
        [OperationContract]
        Task<SectionsResult> GetImportItemsForSectionAsync(string SessionKey, int CompanyId, string[] PayerCode);

        [OperationContract]
        Task<SectionsResult> GetAsync(string SessionKey, int[] ids);
    }
}
