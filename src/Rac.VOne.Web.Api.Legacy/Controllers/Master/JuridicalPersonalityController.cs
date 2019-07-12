using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  �@�l�i�����}�X�^�[
    /// </summary>
    public class JuridicalPersonalityController : ApiControllerAuthorized
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
        public async Task<int> Delete(JuridicalPersonality personality, CancellationToken token)
            => await juridicalPersonalityProcessor.DeleteAsync(personality.CompanyId, personality.Kana, token);

        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="personality"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task< JuridicalPersonality> Save(JuridicalPersonality personality, CancellationToken token)
            => await juridicalPersonalityProcessor.SaveAsync(personality, token);


        /// <summary>
        /// �擾 �z��
        /// </summary>
        /// <param name="personality">���ID �K�{�A�J�i �C��</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<JuridicalPersonality>> GetItems(JuridicalPersonality personality, CancellationToken token)
            => (await juridicalPersonalityProcessor.GetAsync(personality, token)).ToArray();

        /// <summary>
        /// �C���|�[�g
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await juridicalPersonalityImportProcessor.ImportAsync(source, token);

    }
}
