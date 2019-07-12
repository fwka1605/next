using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IMFBillingService" を変更できます。
    [ServiceContract]
    public interface IMFBillingService
    {
        [OperationContract]
        Task<BillingsResult> SavingSetAsync(string SessionKey, IEnumerable<Billing> billings, IEnumerable<Customer> customers);
        [OperationContract]
        Task<MFBillingsResult> GetItemsAsync(string SessionKey, int companyId);

        [OperationContract] Task<MFBillingsResult> GetByBillingIdsAsync(string sessionKey, IEnumerable<long> ids, bool isMatched);
    }
}
