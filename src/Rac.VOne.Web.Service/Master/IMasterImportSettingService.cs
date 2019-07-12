using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IBillingService" を変更できます。
    [ServiceContract]
    public interface IMasterImportSettingService
    {
        [OperationContract]
        Task<MasterImportSettingResult> SaveAsync(string SessionKey, MasterImportSetting masterImportSetting);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey,int CompanyId);

        [OperationContract]
        Task<MasterImportSettingResult> GetAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<MasterImportSettingsResult> GetItemsAsync(string SessionKey, int CompanyId, int ImportFileType);
    }
}