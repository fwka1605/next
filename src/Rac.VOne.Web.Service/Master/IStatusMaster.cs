using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IStatusMaster" を変更できます。
    [ServiceContract]
    public interface IStatusMaster
    {
        [OperationContract]
        Task<StatusResult> GetStatusByCodeAsync(string SessionKey, int CompanyId, int StatusType, string Code);

        [OperationContract]
        Task<StatusesResult> GetStatusesByStatusTypeAsync(string SessionKey, int CompanyId, int StatusType);

        [OperationContract]
        Task<StatusResult> SaveAsync(string SessionKey, Status Status);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<ExistResult> ExistReminderAsync(string SessionKey, int StatusId);

        [OperationContract]
        Task<ExistResult> ExistReminderHistoryAsync(string SessionKey, int StatusId);
    }
}
