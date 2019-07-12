using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface ICustomerGroupMaster
    {
        [OperationContract] Task<CustomerGroupsResult> GetByParentAsync(string SessionKey, int ParentCustomerId);
        [OperationContract] Task<CustomerGroupsResult> GetItemsAsync(string SessionKey, int CompanyId);

        /// <summary>
        ///  債権代表者グループを 子のグループで取得するのと、
        ///  得意先マスターに親として登録されているものを取得する
        /// </summary>
        /// <param name="SessionKey"></param>
        /// <param name="ChildCustomerId"></param>
        /// <returns></returns>
        [OperationContract] Task<CustomerGroupsResult> GetByChildIdAsync(string SessionKey, int[] ChildCustomerId);
        [OperationContract] Task<CustomerGroupsResult> GetPrintCustomerDataAsync(string SessionKey, int CompanyId);

        /// <summary>
        ///  会社ID、得意先コードから債権代表者マスター用の得意先を取得
        /// </summary>
        /// <param name="SessionKey">セッションキー</param>
        /// <param name="CompanyId">会社ID</param>
        /// <param name="Code">得意先コード</param>
        /// <returns></returns>
        [OperationContract] Task<CustomerGroupResult> GetCustomerForCustomerGroupAsync(string SessionKey, int CompanyId, string Code);

        [OperationContract] Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId);

        [OperationContract] Task<ExistResult> HasChildAsync(string SessionKey, int ParentCustomerId);

        /// <summary>
        /// 消込対象の請求データと入金データの債権代表者入子の確認処理
        /// </summary>
        /// <param name="SessionKey"></param>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [OperationContract] Task<CountResult> GetUniqueGroupCountAsync(string SessionKey, int[] Ids);

        [OperationContract] Task<CustomerGroupResult> SaveAsync(string SessionKey, CustomerGroup[] AddList, CustomerGroup[] DeleteList);
        [OperationContract] Task<ImportResultCustomerGroup> ImportAsync(string SessionKey,
               CustomerGroup[] InsertList, CustomerGroup[] UpdateList, CustomerGroup[] DeleteList);


    }
}
