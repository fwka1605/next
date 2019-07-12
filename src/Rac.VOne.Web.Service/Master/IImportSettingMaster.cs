using System.Collections.Generic;
using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
   [ServiceContract]
   public interface IImportSettingMaster
      {
        [OperationContract]
        Task<ImportSettingResult> GetAsync(string SessionKey, int CompanyId, int ImportFileType);

        [OperationContract]
        Task<ImportSettingResults> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ImportSettingResult> SaveAsync(string SessionKey, ImportSetting[] ImportSetting);

    }
}
