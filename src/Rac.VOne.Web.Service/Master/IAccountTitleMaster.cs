using Rac.VOne.Web.Models;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IAccountTitleMaster" を変更できます。
    [ServiceContract]
    public interface IAccountTitleMaster
    {
        [OperationContract]
        Task<AccountTitlesResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<AccountTitlesResult> GetItemsAsync(string SessionKey, AccountTitleSearch searchOption);

        [OperationContract]
        Task<AccountTitlesResult> GetByCodeAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<AccountTitleResult> SaveAsync(string SessionKey, AccountTitle AccountTitle);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int AccountTitleId);

        [OperationContract]
        Task<ImportResultAccountTitle> ImportAsync(string SessionKey,
                AccountTitle[] InsertList, AccountTitle[] UpdateList, AccountTitle[] DeleteList);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForCategoryAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForCustomerDiscountAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForDebitBillingAsync(string SessionKey, int CompanyId, string[] Code);

        [OperationContract]
        Task<MasterDatasResult> GetImportItemsForCreditBillingAsync(string SessionKey, int CompanyId, string[] Code);

    }
}
