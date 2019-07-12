using System;
using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IIgnoreKanaMaster
    {
        [OperationContract]
        Task<IgnoreKanasResult> GetItemsAsync(string sessionKey, int companyId);

        [OperationContract]
        Task<IgnoreKanaResult> SaveAsync(string sessionKey, IgnoreKana kana);

        [OperationContract]
        Task<CountResult> DeleteAsync(string sessionKey, int companyId, string kana);

        [OperationContract]
        Task<ImportResult> ImportAsync(string sessionKey,
                IgnoreKana[] insertList, IgnoreKana[] updateList, IgnoreKana[] deleteList);

        [OperationContract]
        Task<ExistResult> ExistCategoryAsync(string sessionKey, int id);

        [OperationContract]
        Task<IgnoreKanaResult> GetAsync(string sessionKey, int companyId, string kana);

    }
}
