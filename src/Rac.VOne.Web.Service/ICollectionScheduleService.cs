using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ICollectionScheduleService" を変更できます。
    [ServiceContract]
    public interface ICollectionScheduleService
    {

        [OperationContract]
        Task<CollectionSchedulesResult> GetAsync(string SessionKey, CollectionScheduleSearch SearchOption, string connectionId);
    }
}
