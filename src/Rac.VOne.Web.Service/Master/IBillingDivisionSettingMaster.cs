using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IBillingDivisionSettingMaster" を変更できます。
    [ServiceContract]
    public interface IBillingDivisionSettingMaster
    {
        [OperationContract]
        Task<BillingDivisionSettingResult> GetAsync(string SessionKey, int CompanyId);
    }
}
