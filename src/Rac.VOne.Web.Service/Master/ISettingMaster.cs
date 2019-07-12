using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ISettingMaster
    {
        [OperationContract]
        Task<SettingsResult> GetItemsAsync(string SessionKey, string[] ItemId);
    }
}
