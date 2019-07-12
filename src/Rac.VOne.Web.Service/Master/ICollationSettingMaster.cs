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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ICollationSettingMaster" を変更できます。
    [ServiceContract]
    public interface ICollationSettingMaster
    {
        [OperationContract]
        Task<CollationSettingResult> GetAsync(string SessionKey, int CompanyId);
        [OperationContract]
        Task<CollationOrdersResult> GetCollationOrderAsync(string SessionKey, int CompanyId);
        [OperationContract]
        Task<MatchingOrdersResult> GetMatchingBillingOrderAsync(string SessionKey, int CompanyId);
        [OperationContract]
        Task<MatchingOrdersResult> GetMatchingReceiptOrderAsync(string SessionKey, int CompanyId);
        [OperationContract]
        Task<CollationSettingResult> SaveAsync(string SessionKey, CollationSetting CollationSetting, CollationOrder[] CollationOrder,
           MatchingOrder[] MatchingBillingOrder, MatchingOrder[] MatchingReceiptOrder);
    }
}
