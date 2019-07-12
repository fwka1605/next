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
    ///  法人格除去マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JuridicalPersonalityController : ControllerBase
    {
        private readonly IJuridicalPersonalityProcessor juridicalPersonalityProcessor;
        private readonly IJuridicalPersonalityFileImportProcessor juridicalPersonalityImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public JuridicalPersonalityController(
            IJuridicalPersonalityProcessor juridicalPersonalityProcessor,
            IJuridicalPersonalityFileImportProcessor juridicalPersonalityImportProcessor
            )
        {
            this.juridicalPersonalityProcessor = juridicalPersonalityProcessor;
            this.juridicalPersonalityImportProcessor = juridicalPersonalityImportProcessor;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="personality">会社ID、カナを指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(JuridicalPersonality personality, CancellationToken token)
            => await juridicalPersonalityProcessor.DeleteAsync(personality.CompanyId, personality.Kana, token);

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="personality"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< JuridicalPersonality>> Save(JuridicalPersonality personality, CancellationToken token)
            => await juridicalPersonalityProcessor.SaveAsync(personality, token);


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="personality">会社ID 必須、カナ 任意</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<JuridicalPersonality>>> GetItems(JuridicalPersonality personality, CancellationToken token)
            => (await juridicalPersonalityProcessor.GetAsync(personality, token)).ToArray();

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await juridicalPersonalityImportProcessor.ImportAsync(source, token);

    }
}
