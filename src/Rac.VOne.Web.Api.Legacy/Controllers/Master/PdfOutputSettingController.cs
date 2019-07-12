using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// PDF出力設定
    /// </summary>
    public class PdfOutputSettingController : ApiControllerAuthorized
    {
        private readonly IPdfOutputSettingProcessor pdfOutputSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public PdfOutputSettingController(
            IPdfOutputSettingProcessor pdfOutputSettingProcessor
            )
        {
            this.pdfOutputSettingProcessor = pdfOutputSettingProcessor;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="setting">会社ID、レポートタイプ、UpdateBy にログインユーザーID を設定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns>
        /// 取得時に マスター未登録の場合、データの登録を行うため、 UpdateBy へ ログインユーザID の設定が必要
        /// </returns>
        [HttpPost]
        public async Task<PdfOutputSetting> Get(PdfOutputSetting setting, CancellationToken token)
            => await pdfOutputSettingProcessor.GetAsync(setting.CompanyId, setting.ReportType, setting.UpdateBy, token);

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task< PdfOutputSetting> Save(PdfOutputSetting setting, CancellationToken token)
            => await pdfOutputSettingProcessor.SaveAsync(setting, token);

    }
}
