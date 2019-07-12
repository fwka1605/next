using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IGridSettingMaster" を変更できます。
    [ServiceContract]
    public interface IGridSettingMaster
    {
        [OperationContract]
        Task<GridSettingResult> SaveAsync(string SessionKey, GridSetting[] GridSettings);

        [OperationContract]
        Task<GridSettingsResult> GetItemsAsync(string SessionKey, int CompanyId, int LoginUserId, int? GridId);

        [OperationContract]
        Task<GridSettingsResult> GetDefaultItemsAsync(string SessionKey, int CompanyId, int LoginUserId, int? GridId);
    }
}
