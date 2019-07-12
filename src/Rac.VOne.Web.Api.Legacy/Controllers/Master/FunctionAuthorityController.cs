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
    ///  �@�\�����}�X�^�[
    /// </summary>
    public class FunctionAuthorityController : ApiControllerAuthorized
    {
        private readonly IFunctionAuthorityProcessor functionAuthorityProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public FunctionAuthorityController(
            IFunctionAuthorityProcessor functionAuthorityProcessor
            )
        {
            this.functionAuthorityProcessor = functionAuthorityProcessor;
        }

        /// <summary>
        /// �擾 �z��
        /// </summary>
        /// <param name="option">���ID</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<FunctionAuthority>> GetItems(FunctionAuthoritySearch option, CancellationToken token)
            => (await functionAuthorityProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// �o�^
        /// </summary>
        /// <param name="authorities">�@�\���� �z��</param>
        /// <param name="token">�����o�C���h</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<FunctionAuthority>> Save(IEnumerable<FunctionAuthority> authorities, CancellationToken token)
            => (await functionAuthorityProcessor.SaveAsync(authorities, token)).ToArray();

    }
}
