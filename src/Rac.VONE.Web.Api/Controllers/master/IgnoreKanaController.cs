using System;
using Rac.VOne.Web.Common.Importers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  除外カナマスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IgnoreKanaController : ControllerBase
    {
        private readonly IIgnoreKanaProcessor ignoreKanaProcessor;
        private readonly IIgnoreKanaFileImportProcessor ignoreKanaImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public IgnoreKanaController(
            IIgnoreKanaProcessor ignoreKanaProcessor,
            IIgnoreKanaFileImportProcessor ignoreKanaImportProcessor
            )
        {
            this.ignoreKanaProcessor = ignoreKanaProcessor;
            this.ignoreKanaImportProcessor = ignoreKanaImportProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="kana">会社ID 必須 カナ 任意</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<IgnoreKana>>> GetItems(IgnoreKana kana, CancellationToken token)
            => (await ignoreKanaProcessor.GetAsync(kana, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="kana">除外カナ</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IgnoreKana>> Save(IgnoreKana kana, CancellationToken token)
            => await ignoreKanaProcessor.SaveAsync(kana, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="kana">除外カナ</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(IgnoreKana kana, CancellationToken token)
            => await ignoreKanaProcessor.DeleteAsync(kana, token);

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await ignoreKanaImportProcessor.ImportAsync(source, token);

        /// <summary>
        /// 区分マスターの登録確認
        /// </summary>
        /// <param name="id">区分ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistCategory([FromBody] int id, CancellationToken token)
            => await ignoreKanaProcessor.ExistCategoryAsync(id, token);


    }
}
