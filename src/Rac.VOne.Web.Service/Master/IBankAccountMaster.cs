using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IBankAccountMaster
    {
        [OperationContract]
        Task<BankAccountsResult> GetAsync(string SessionKey, int[] Id);

        [OperationContract]
        Task<BankAccountResult> GetByCodeAsync(string SessionKey, int CompanyId, string BankCode,
            string BranchCode, int AccountTypeId, string AccountNumber);

        [OperationContract]
        Task<BankAccountResult> GetByBranchNameAsync(string SessionKey, int CompanyId, string BankCode,
            string BranchName, int AccountTypeId, string AccountNumber);

        [OperationContract]
        Task<BankAccountsResult> GetItemsAsync(string SessionKey, int CompanyId,
            BankAccountSearch BankAccountSearch);

        [OperationContract]
        Task<BankAccountResult> SaveAsync(string SessionKey, BankAccount BankAccount);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<ExistResult> ExistCategoryAsync(string SessionKey, int CategoryId);

        [OperationContract]
        Task<ExistResult> ExistSectionAsync(string SessionKey, int SectionId);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
             BankAccount[] InsertList, BankAccount[] UpdateList, BankAccount[] DeleteList);
    }
}
