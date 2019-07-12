using System;
using System.Linq;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "ClosingHistoryService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで ClosingHistoryService.svc または ClosingHistoryService.svc.cs を選択し、デバッグを開始してください。
    public class ClosingService : IClosingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IClosingProcessor closingProcessor;
        private readonly ILogger logger;

        public ClosingService(
            IAuthorizationProcessor authorizationProcessor,
            IClosingProcessor closingProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.closingProcessor = closingProcessor;
            logger = logManager.GetLogger(typeof(CollectionScheduleService));
        }
        public async Task<ClosingInformationResult> GetClosingInformationAsync(string sessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await closingProcessor.GetClosingInformationAsync(companyId, token);
                return new ClosingInformationResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ClosingInformation = result
                };
            }, logger);
        }

        public async Task<ClosingHistorysResult> GetClosingHistoryAsync(string sessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await closingProcessor.GetClosingHistoryAsync(companyId, token);
                return new ClosingHistorysResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ClosingHistorys = result.ToList(),
                };
            }, logger);
        }

        public async Task<ClosingResult> SaveAsync(string sessionKey, Closing closing)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await closingProcessor.SaveAsync(closing, token);
                return new ClosingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Closing = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string sessionKey, int companyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
            {
                var result = await closingProcessor.DeleteAsync(companyId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

    }
}
