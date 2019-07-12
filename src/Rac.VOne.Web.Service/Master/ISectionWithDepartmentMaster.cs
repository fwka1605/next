using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ISectionWithDepartmentMaster" を変更できます。
    [ServiceContract]
    public interface ISectionWithDepartmentMaster
    {
        [OperationContract]
        Task<SectionWithDepartmentResult> SaveAsync(string sessionKey, SectionWithDepartment[] AddList, SectionWithDepartment[] DeleteList);

        [OperationContract]
        Task<SectionWithDepartmentsResult> GetBySectionAsync(string SessionKey, int CompanyId, int SectionId);

        [OperationContract]
        Task<SectionWithDepartmentResult> GetByDepartmentAsync(string sessionKey, int CompanyId, int DepartmentId);

        [OperationContract]
        Task<SectionWithDepartmentsResult> GetItemsAsync(string SessionKey, int CompanyId, SectionWithDepartmentSearch SectionWithDepartmentSearch);

        [OperationContract]
        Task<ImportResult> ImportAsync(string sessionKey,
                SectionWithDepartment[] insertList, SectionWithDepartment[] updateList, SectionWithDepartment[] deleteList);
        [OperationContract]
        Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId);

        [OperationContract]
        Task<ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId);
    }
}
