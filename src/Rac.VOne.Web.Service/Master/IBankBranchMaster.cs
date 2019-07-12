using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IBankBranchMaster
    {
        [OperationContract]
        Task<BankBranchResult> GetAsync(string Sessionkey, int CompanyId, string BankCode, string BranchCode);

        [OperationContract]
        Task<BankBranchResult> SaveAsync(string Sessionkey, BankBranch BankBranch);

        [OperationContract]
        Task<CountResult> DeleteAsync(string Sessionkey, int CompanyId, string BankCode, string BranchCode);

        [OperationContract]
        Task<BankBranchsResult> GetItemsAsync(string Sessionkey, int CompanyId, BankBranchSearch BankBranchSearch);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
                BankBranch[] insertList, BankBranch[] updateList, BankBranch[] deleteList);
    }
}
