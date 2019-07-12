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
    ///  機能権限マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FunctionAuthorityController : ControllerBase
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
        /// 取得 配列
        /// </summary>
        /// <param name="option">会社ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<FunctionAuthority>>> GetItems(FunctionAuthoritySearch option, CancellationToken token)
            => (await functionAuthorityProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="authorities">機能権限 配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<FunctionAuthority>>> Save(IEnumerable<FunctionAuthority> authorities, CancellationToken token)
            => (await functionAuthorityProcessor.SaveAsync(authorities, token)).ToArray();

    }
}
