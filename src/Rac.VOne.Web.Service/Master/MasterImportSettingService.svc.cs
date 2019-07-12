using Rac.VOne.Web.Models;
using System;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    public class MasterImportSettingService : IMasterImportSettingService
    {
        public Task<CountResult> DeleteAsync(string SessionKey, int CompanyId)
        {
            throw new NotImplementedException();
        }

        public Task<MasterImportSettingResult> GetAsync(string SessionKey, int CompanyId)
        {
            throw new NotImplementedException();
        }

        public Task<MasterImportSettingsResult> GetItemsAsync(string SessionKey, int CompanyId, int ImportFileType)
        {
            throw new NotImplementedException();
        }

        public Task<MasterImportSettingResult> SaveAsync(string SessionKey, MasterImportSetting masterImportSetting)
        {
            throw new NotImplementedException();
        }
    }
}
