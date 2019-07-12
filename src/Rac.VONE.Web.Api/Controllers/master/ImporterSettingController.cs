using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImporterSettingController : ControllerBase
    {
        private readonly IImporterSettingProcessor importSettingProcessor;
        private readonly IImporterSettingDetailProcessor importSettingDetailProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ImporterSettingController(
            IImporterSettingProcessor importSettingProcessor,
            IImporterSettingDetailProcessor importSettingDetailProcessor
            )
        {
            this.importSettingProcessor = importSettingProcessor;
            this.importSettingDetailProcessor = importSettingDetailProcessor;
        }

        /// <summary>
        /// 取得 ヘッダーの配列
        /// </summary>
        /// <param name="setting">検索条件 会社ID、フォーマットIDを指定、コード指定は任意</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ImporterSetting>>> GetHeader(ImporterSetting setting, CancellationToken token)
            => (await importSettingProcessor.GetAsync(setting, token)).ToArray();

        /// <summary>
        /// 取得 明細配列
        /// </summary>
        /// <param name="setting">検索条件
        /// 会社ID、フォーマットID、コード を指定 するか、<see cref="ImporterSetting.Id"/>を指定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ImporterSettingDetail>>> GetDetail(ImporterSetting setting, CancellationToken token)
            => (await importSettingDetailProcessor.GetAsync(setting, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="setting">設定ヘッダー 設定明細 配列も設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImporterSetting>> Save(ImporterSetting setting, CancellationToken token)
            => await importSettingProcessor.SaveAsync(setting, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="setting">ヘッダーID を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(ImporterSetting setting, CancellationToken token)
            => await importSettingProcessor.DeleteAsync(setting.Id, token);

    }
}
