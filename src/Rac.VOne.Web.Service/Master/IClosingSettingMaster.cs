using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Text;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IClosingSettingMaster" を変更できます。
    [ServiceContract]
    public interface IClosingSettingMaster
    {
        [OperationContract]
        Task<ClosingSettingResult> GetAsync(string sessionKey, int companyId);
        [OperationContract]
        Task<ClosingSettingResult> SaveAsync(string sessionKey, ClosingSetting setting);
    }
}
