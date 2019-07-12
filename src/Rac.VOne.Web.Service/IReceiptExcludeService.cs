using System.ServiceModel;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IReceiptExcludeService
    {
        [OperationContract]
        Task<ExistResult> ExistExcludeCategoryAsync(string SessionKey, int CategoryId);

        [OperationContract]
        Task<ReceiptExcludesResult> GetByReceiptIdAsync(string SessionKey, long ReceiptId);

        [OperationContract]
        Task<ReceiptExcludesResult> GetByIdsAsync(string SessionKey, long[] ids);
    }
}
