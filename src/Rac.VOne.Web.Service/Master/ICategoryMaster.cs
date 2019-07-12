using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ICategoryMaster
    {
        [OperationContract]
        Task<CategoriesResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<CategoriesResult> GetByCodeAsync(string SessionKey, int CompanyId, int CategoryType, string[] Code);

        [OperationContract]
        Task<CategoriesResult> GetItemsAsync(string SessionKey, CategorySearch CategorySearch);


        [OperationContract]
        Task<CategoriesResult> GetInvoiceCollectCategoriesAsync(string SessionKey, int CompanyId, int CategoryType);

        [OperationContract]
        Task<CategoryResult> SaveAsync(string SessionKey, Category Category);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);


        [OperationContract]
        Task<ExistResult> ExistAccountTitleAsync(string SessionKey, int AccountTitleId);

        [OperationContract]
        Task<ExistResult> ExistPaymentAgencyAsync(string SessionKey,int PaymentAgencyId);
    }
}
