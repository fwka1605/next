using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// Company Master操作用コントロール
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionProcessor sectionProcessor;
        private readonly ISectionFileImportProcessor sectionImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public SectionController(
            ISectionProcessor sectionProcessor,
            ISectionFileImportProcessor sectionImportProcessor
            )
        {
            this.sectionProcessor = sectionProcessor;
            this.sectionImportProcessor = sectionImportProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="section"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< Section >> Save(Section section, CancellationToken token)
            => await sectionProcessor.SaveAsync(section, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="section">setion.Id を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(Section section, CancellationToken token)
            => await sectionProcessor.DeleteAsync(section.Id, token);

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Section>>> GetItems(SectionSearch option, CancellationToken token)
            => (await sectionProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await sectionImportProcessor.ImportAsync(source, token);

    }
}
