using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 税区分
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaxClassController : ControllerBase
    {
        private readonly ITaxClassProcessor taxclassProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public TaxClassController(
            ITaxClassProcessor taxClassProcessor
            )
        {
            taxclassProcessor = taxClassProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< TaxClass >>> GetItems(CancellationToken token)
            => (await taxclassProcessor.GetAsync(token)).ToArray();
    }
}
