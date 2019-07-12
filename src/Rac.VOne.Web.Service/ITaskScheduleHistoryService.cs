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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "ITaskScheduleHistoryService" を変更できます。
    [ServiceContract]
    public interface ITaskScheduleHistoryService
    {
        [OperationContract]
        Task<TaskScheduleHistoryResult> GetItemsAsync(string SessionKey, TaskScheduleHistorySearch searchConditions);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<TaskScheduleHistoryResult> SaveAsync(string SessionKey, TaskScheduleHistory history);
    }
}
