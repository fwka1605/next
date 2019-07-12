using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Common.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ILogsService" を変更できます。
    [ServiceContract]
    public interface ILogsService
    {
        [OperationContract]
        Task<bool> SaveErrorLogAsync(Logs log);
    }
}
