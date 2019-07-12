using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using Rac.VOne.Web.Models;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IClientKey" を変更できます。
    [ServiceContract]
    public interface IDBService
    {
        [OperationContract]
        Task<ClientKeyResult> GetClientKeyAsync(string SessionKey, string ProgramId, string ClientName, string CompanyCode, string LoginUserCode);

    }
}
