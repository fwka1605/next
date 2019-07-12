using System;
using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IDestinationMaster
    {
        [OperationContract]
        Task<DestinationsResult> GetItemsAsync(string SessionKey, DestinationSearch option);

        [OperationContract]
        Task<DestinationResult> SaveAsync(string SessionKey, Destination Destination);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);
    }
}
