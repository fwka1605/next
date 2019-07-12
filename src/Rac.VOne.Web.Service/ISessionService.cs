using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ISessionService" を変更できます。
    [ServiceContract]
    public interface ISessionService
    {
        [OperationContract]
        Task<ConnectionInfoResult> GetConnectionInfoAsync(string SessionKey);
    }

    [DataContract]
    public class ConnectionInfoResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }

        [DataMember]
        public string ConnectionInfo { get; set; }
    }
}
