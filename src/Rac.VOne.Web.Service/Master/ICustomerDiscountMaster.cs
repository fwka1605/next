using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ICustomerDiscountMaster
    {
        [OperationContract]
        Task<ExistResult> ExistAccountTitleAsync(string sessionKey, int accountTitleid);

        [OperationContract]
        Task<ImportResult> ImportAsync(string sessionKey,
                CustomerDiscount[] insertList, CustomerDiscount[] updateList, CustomerDiscount[] deleteList);
    }
}