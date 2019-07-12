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
    /// 税区分
    /// </summary>
    public class TaxClassController : ApiControllerAuthorized
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
        public async Task<IEnumerable< TaxClass >> GetItems(CancellationToken token)
            => (await taxclassProcessor.GetAsync(token)).ToArray();
    }
}
