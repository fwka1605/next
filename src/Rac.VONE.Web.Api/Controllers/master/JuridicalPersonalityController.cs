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
    ///  �@�l�i�����}�X�^�[
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
        /// �폜
        /// </summary>
        /// <param name="personality">���ID�A�J�i���w��</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(JuridicalPersonality personality, CancellationToken token)
            => await juridicalPersonalityProcessor.DeleteAsync(personality.CompanyId, personality.Kana, token);

        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="personality"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< JuridicalPersonality>> Save(JuridicalPersonality personality, CancellationToken token)
            => await juridicalPersonalityProcessor.SaveAsync(personality, token);


        /// <summary>
        /// �擾 �z��
        /// </summary>
        /// <param name="personality">���ID �K�{�A�J�i �C��</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<JuridicalPersonality>>> GetItems(JuridicalPersonality personality, CancellationToken token)
            => (await juridicalPersonalityProcessor.GetAsync(personality, token)).ToArray();

        /// <summary>
        /// �C���|�[�g
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await juridicalPersonalityImportProcessor.ImportAsync(source, token);

    }
}
