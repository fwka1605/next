using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IBillingDivisionContractMaster" を変更できます。
    [ServiceContract]
    public interface IBillingDivisionContractMaster
    {
        [OperationContract]
        Task<BillingDivisionContractsResult> GetByContractNumberAsync(string SessionKey, int CompanyId, int CustomerId, string ContractNumber);
        [OperationContract]
        Task<BillingDivisionContractsResult> GetItemsAsync(string SessionKey, int CompanyId, int CustomerId);
        [OperationContract]
        Task<BillingDivisionContractsResult> GetByCustomerIdsAsync(string SessionKey, int CompanyId, int[] CustomerIds);

        [OperationContract]
        Task<BillingDivisionContractsResult> GetByBillingIdsAsync(string sessionKey, long[] billingIds);
    }
}
