using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
   [ServiceContract]
    public interface IControlColorMaster
    {
        [OperationContract]
        Task<ControlColorResult> GetAsync(string SessionKey, int CompanyId, int LoginuserId);

        [OperationContract]
        Task<ControlColorsResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ControlColorResult> SaveAsync(string SessionKey, ControlColor ControlColor);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int LoginUserId);
    }
}
