using System;
using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ILoginUserMaster
    {
        [OperationContract]
        Task<UsersResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<UserResult> SaveAsync(string SessionKey, LoginUser UserData);

        [OperationContract]
        Task<UsersResult> GetItemsAsync(string SessionKey, int CompanyId, LoginUserSearch LoginUserSearch);

        [OperationContract]
        Task<CountResult> ResetPasswordAsync(string SessionKey, int Id);

        [OperationContract]
        Task<CountResult> ExitStaffAsync(string SessionKey, int StaffId);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<UsersResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<ImportResultLoginUser> ImportAsync(string SessionKey,
            int CompanyId,
            int LoginUserId,
            LoginUser[] InsertList,
            LoginUser[] UpdateList,
            LoginUser[] DeleteList);

        [OperationContract]
        Task<UsersResult> GetImportItemsForSectionAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<UsersResult> GetItemsForGridLoaderAsync(string Sessionkey, int CompanyId);
    }
}
