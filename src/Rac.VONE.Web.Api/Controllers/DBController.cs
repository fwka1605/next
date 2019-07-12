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
    ///  クライアントキー取得
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private readonly IDbFunctionProcessor dbfunctionProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public DbController(
            IDbFunctionProcessor dbfunctionProcessor
            )
        {
            this.dbfunctionProcessor = dbfunctionProcessor;
        }

        /// <summary>
        /// クライアントキー取得
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<byte[]>> GetClientKey(ClientKeySearch option, CancellationToken token)
            => await dbfunctionProcessor.CreateClientKeyAsync(option, token);

    }
}
