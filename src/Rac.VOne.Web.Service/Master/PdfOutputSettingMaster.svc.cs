using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "PdfOutputSettingMaster" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで PdfOutputSettingMaster.svc または PdfOutputSettingMaster.svc.cs を選択し、デバッグを開始してください。
    public class PdfOutputSettingMaster : IPdfOutputSettingMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IPdfOutputSettingProcessor pdfOutputSettingProcessor;
        private readonly ILogger logger;
        public PdfOutputSettingMaster(
            IAuthorizationProcessor authorizationProcessor,
            IPdfOutputSettingProcessor pdfOutputSettingProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.pdfOutputSettingProcessor = pdfOutputSettingProcessor;
            logger = logManager.GetLogger(typeof(InvoiceSettingService));
        }
        public async Task<PdfOutputSettingResult> GetAsync(string SessionKey, int CompanyId, int ReportType, int UserId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await pdfOutputSettingProcessor.GetAsync(CompanyId, ReportType, UserId, token);
                return new PdfOutputSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PdfOutputSetting = result,
                };
            }, logger);
        }

        public async Task<PdfOutputSettingResult> SaveAsync(string SessionKey, PdfOutputSetting Setting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await pdfOutputSettingProcessor.SaveAsync(Setting, token);

                return new PdfOutputSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    PdfOutputSetting = result,
                };
            }, logger);
        }
    }
}
