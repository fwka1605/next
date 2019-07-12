using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  除外カナマスター
    /// </summary>
    public class IgnoreKanaController : ApiControllerAuthorized
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
        public async Task<IEnumerable<IgnoreKana>> GetItems(IgnoreKana kana, CancellationToken token)
            => (await ignoreKanaProcessor.GetAsync(kana, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="kana">除外カナ</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IgnoreKana> Save(IgnoreKana kana, CancellationToken token)
            => await ignoreKanaProcessor.SaveAsync(kana, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="kana">除外カナ</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(IgnoreKana kana, CancellationToken token)
            => await ignoreKanaProcessor.DeleteAsync(kana, token);

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await ignoreKanaImportProcessor.ImportAsync(source, token);

        /// <summary>
        /// 区分マスターの登録確認
        /// </summary>
        /// <param name="id">区分ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCategory([FromBody] int id, CancellationToken token)
            => await ignoreKanaProcessor.ExistCategoryAsync(id, token);


    }
}
