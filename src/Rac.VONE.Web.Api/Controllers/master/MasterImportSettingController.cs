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
    ///  取込設定マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterImportSettingController : ControllerBase
    {
        private readonly IImportSettingProcessor importsettingProcessor;


        /// <summary>
        /// constructor
        /// </summary>
        public MasterImportSettingController(
            IImportSettingProcessor importsettingProcessor
            )
        {
            this.importsettingProcessor = importsettingProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ImportSetting>>> GetItems(ImportSettingSearch option, CancellationToken token)
            => (await importsettingProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ImportSetting>>> Save(IEnumerable<ImportSetting> settings, CancellationToken token)
            => (await importsettingProcessor.SaveAsync(settings, token)).ToArray();

    }
}
