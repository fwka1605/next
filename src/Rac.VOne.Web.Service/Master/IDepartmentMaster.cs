using Rac.VOne.Web.Models;
using System.ServiceModel;
using System;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IDepartmentMaster
    {
        [OperationContract]
        Task<DepartmentResult> SaveAsync(string SessionKey, Department Department);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<DepartmentsResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<DepartmentsResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<DepartmentResult> GetByCodeAndStaffAsync(string SessionKey, int CompanyId, string Code);


        [OperationContract]
        Task<DepartmentsResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<DepartmentsResult> GetDepartmentAndStaffAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey, Department[] InsertList, Department[] UpdateList, Department[] DeleteList);

        //for Importing
        [OperationContract]
        Task<MasterDatasResult> GetImportItemsStaffAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsSectionWithDepartmentAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsBillingAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<DepartmentsResult> DepartmentWithSectionAsync(string SessionKey, int CompanyId, int SectionId, int[] GridDepartmentId);

        [OperationContract]
        Task<DepartmentsResult> GetWithoutSectionAsync(string SessionKey, int CompanyId, int SectionId);

        [OperationContract]
        Task<DepartmentsResult> GetByLoginUserIdAsync(string SessionKey, int CompanyId, int LoginUserId);
    }
}
