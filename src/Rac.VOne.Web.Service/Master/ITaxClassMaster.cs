using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ITaxClassMaster
    {
        [OperationContract]
        Task<TaxClassResult> GetItemsAsync(string sessionKey);
    }
}
