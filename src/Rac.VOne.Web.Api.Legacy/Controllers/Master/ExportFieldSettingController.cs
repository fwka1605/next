using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  出力設定マスター（消込済入金データ出力）
    /// </summary>
    public class ExportFieldSettingController : ApiControllerAuthorized
    {
        private readonly IExportFieldSettingProcessor exportFieldSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ExportFieldSettingController(
            IExportFieldSettingProcessor exportFieldSettingProcessor
            )
        {
            this.exportFieldSettingProcessor = exportFieldSettingProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="setting">検索条件 会社ID、出力ファイル種別 を指定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ExportFieldSetting>> GetItems(ExportFieldSetting setting, CancellationToken token)
            => (await exportFieldSettingProcessor.GetAsync(setting, token)).ToArray();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="settings">出力フィールド設定 配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ExportFieldSetting>> Save(IEnumerable<ExportFieldSetting> settings, CancellationToken token)
            => (await exportFieldSettingProcessor.SaveAsync(settings, token)).ToArray();

    }
}
