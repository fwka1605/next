using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "Report" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで Report.svc または Report.svc.cs を選択し、デバッグを開始してください。
    public class ReportService : IReportService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IReportSettingProcessor reportSettingProcessor;
        private readonly IArrearagesListProcessor arrearagesListProcessor;
        private readonly IScheduledPaymentListProcessor scheduledPaymentListProcessor;
        private readonly ILogger logger;

        public ReportService(
            IAuthorizationProcessor authorizationProcessor,
            IReportSettingProcessor reportSettingProcessor,
            IArrearagesListProcessor arrearagesListProcessor,
            IScheduledPaymentListProcessor scheduledPaymentListProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.reportSettingProcessor = reportSettingProcessor;
            this.arrearagesListProcessor = arrearagesListProcessor;
            this.scheduledPaymentListProcessor = scheduledPaymentListProcessor;
            logger = logManager.GetLogger(typeof(ReportService));
        }

        public async Task<ArrearagesListsResult> ArrearagesListAsync(string SessionKey, int CompanyId, ArrearagesListSearch ArrearagesListSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var reportSettingList = (await reportSettingProcessor.GetAsync(CompanyId, "PF0401", token)).ToList();
                ArrearagesListSearch.CompanyId = CompanyId;
                ArrearagesListSearch.ReportSettings = reportSettingList;
                var searchResult = (await arrearagesListProcessor.GetAsync(ArrearagesListSearch, token)).ToList();

                return new ArrearagesListsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ArrearagesLists  = searchResult,
                };
            }, logger);
        }


        public async Task<ScheduledPaymentListsResult> ScheduledPaymentListAsync(string SessionKey, int CompanyId, ScheduledPaymentListSearch ScheduledPaymentListSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var reportSettingList = (await reportSettingProcessor.GetAsync(CompanyId, "PF0301", token)).ToList();
                ScheduledPaymentListSearch.CompanyId = CompanyId;
                ScheduledPaymentListSearch.ReportSettings = reportSettingList;
                var searchResult = (await scheduledPaymentListProcessor.GetAsync(ScheduledPaymentListSearch, token)).ToList();

                return new ScheduledPaymentListsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    ScheduledPaymentLists = searchResult,
                };
            }, logger);
        }

    }
}
