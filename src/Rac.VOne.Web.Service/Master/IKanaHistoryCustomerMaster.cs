using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IKanaHistoryCustomerMaster" を変更できます。
    [ServiceContract]
    public interface IKanaHistoryCustomerMaster
    {

        [OperationContract]
        Task<ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId);

        [OperationContract]
        Task<KanaHistoryCustomersResult> GetListAsync(string SessionKey, int CompanyId, string PayerName, string CustomerCodeFrom, string CustomerCodeTo);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey,
               int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int CustomerId);

        [OperationContract]
        Task<ImportResult> ImportAsync(string SessionKey,
             KanaHistoryCustomer[] InsertList, KanaHistoryCustomer[] UpdateList, KanaHistoryCustomer[] DeleteList);

    }
}
