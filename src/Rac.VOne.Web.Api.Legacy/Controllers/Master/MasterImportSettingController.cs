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
    ///  取込設定マスター
    /// </summary>
    public class MasterImportSettingController : ApiControllerAuthorized
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
        public async Task<IEnumerable<ImportSetting>> GetItems(ImportSettingSearch option, CancellationToken token)
            => (await importsettingProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ImportSetting>> Save(IEnumerable<ImportSetting> settings, CancellationToken token)
            => (await importsettingProcessor.SaveAsync(settings, token)).ToArray();

    }
}
