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
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IImportFileLogService" を変更できます。
    [ServiceContract]
    public interface IImportFileLogService
    {
        [OperationContract]
        Task<ImportFileLogsResult> GetHistoryAsync(string Sessionkey, int ComapnyId);

        [OperationContract]
        Task<ImportFileLogsResult> DeleteItemsAsync(string SessionKey, int[] Ids);

        [OperationContract]
        Task<ImportFileLogsResult> SaveImportFileLogAsync(string Sessionkey, ImportFileLog[] ImportFileLog);


    }
}
