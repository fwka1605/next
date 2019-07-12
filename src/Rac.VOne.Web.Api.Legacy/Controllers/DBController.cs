using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  クライアントキー取得
    /// </summary>
    public class DbController : ApiControllerAuthorized
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
        public async Task<byte[]> GetClientKey(ClientKeySearch option, CancellationToken token)
            => await dbfunctionProcessor.CreateClientKeyAsync(option, token);

    }
}
