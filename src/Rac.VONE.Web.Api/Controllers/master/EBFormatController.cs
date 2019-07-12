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
    ///  EBフォーマットマスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EBFormatController : ControllerBase
    {
        private readonly IEBFormatProcessor ebFormatProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public EBFormatController(
            IEBFormatProcessor ebFormatProcessor
            )
        {
            this.ebFormatProcessor = ebFormatProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<EBFormat>>> GetItems(CancellationToken token)
            => (await ebFormatProcessor.GetAsync(token)).ToArray();
    }
}
