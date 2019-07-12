using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IClosingHistoryService" を変更できます。
    [ServiceContract]
    public interface IClosingService
    {
        [OperationContract] Task<ClosingInformationResult> GetClosingInformationAsync(string sessionKey, int companyId);
        [OperationContract] Task<ClosingHistorysResult> GetClosingHistoryAsync(string sessionKey, int companyId);
        [OperationContract] Task<ClosingResult> SaveAsync(string sessionKey, Closing closing);
        [OperationContract] Task<CountResult> DeleteAsync(string sessionKey, int companyId);
    }
}
