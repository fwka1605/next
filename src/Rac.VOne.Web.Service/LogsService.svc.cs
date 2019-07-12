using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
        // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "LogsService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで LogsService.svc または LogsService.svc.cs を選択し、デバッグを開始してください。
    public class LogsService : ILogsService
    {
        private readonly ILogsProcessor logsProcessor;
        //private readonly ILogger logger;

        public LogsService(ILogsProcessor logsProcessor/*, ILogManager logManager*/)
        {
            this.logsProcessor = logsProcessor;
            //logger = logManager.GetLogger(typeof(LogsService));
        }

        public async Task<bool> SaveErrorLogAsync(Logs log)
        {
            try
            {
                await logsProcessor.SaveAsync(log);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
