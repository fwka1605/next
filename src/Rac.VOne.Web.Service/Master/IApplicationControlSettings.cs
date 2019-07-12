using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IApplicationControlMaster
    {
        [OperationContract]
        Task<ApplicationControlResult> GetAsync(string sessionKey, int companyId);

        [OperationContract]
        Task<CountResult> SaveAsync(string sessionKey, ApplicationControl applicationControl);

        [OperationContract]
        Task<ApplicationControlResult> AddAsync(string sessionKey, ApplicationControl applicationControl);
    }
}
