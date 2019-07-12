using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IInputControlMaster
    {
        [OperationContract]
        Task<InputControlsResult> GetAsync(string SessionKey, int CompanyId, int LoginUserId, int InputGridTypeId);

        [OperationContract]
        Task<InputControlsResult> SaveAsync(string SessionKey, int CompanyId, int LoginUserId, int InputGridTypeId,
            InputControl[] InputControl);
    }
}
