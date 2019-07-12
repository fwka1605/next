using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IBillingAgingListService" を変更できます。
    [ServiceContract]
    public interface IBillingAgingListService
    {
        [OperationContract]
        Task<BillingAgingListsResult> GetAsync(string SessionKey, BillingAgingListSearch searchOption, string connectionId);

        [OperationContract]
        Task<BillingAgingListDetailsResult> GetDetailsAsync(string SessionKey, BillingAgingListSearch searchOption);
    }
}
