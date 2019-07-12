using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IGeneralSettingMaster
    {
        [OperationContract]
        Task<GeneralSettingsResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<GeneralSettingResult> SaveAsync(string SessionKey, GeneralSetting Generalsetting);

        [OperationContract]
        Task<GeneralSettingResult> GetByCodeAsync(string SessionKey, int CompanyId, string Code);
    }
}
