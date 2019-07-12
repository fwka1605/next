using System;
using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IStaffMaster
    {
        [OperationContract]
        Task<StaffResult> SaveAsync(string SessionKey, Staff Staff);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<StaffsResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<StaffsResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<StaffsResult> GetItemsAsync(string SessionKey, StaffSearch option);

        [OperationContract]
        Task<ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId);

        [OperationContract]
        Task<ImportResultStaff> ImportAsync(string SessionKey, Staff[] InsertList, Staff[] UpdateList, Staff[] DeleteList);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsLoginUserAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsCustomerAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsBillingAsync(string SessionKey, int CompanyId, string[] Code);
    }
}



