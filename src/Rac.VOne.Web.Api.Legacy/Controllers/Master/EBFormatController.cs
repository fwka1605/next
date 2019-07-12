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
    ///  EBフォーマットマスター
    /// </summary>
    public class EBFormatController : ApiControllerAuthorized
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
        public async Task<IEnumerable<EBFormat>> GetItems(CancellationToken token)
            => (await ebFormatProcessor.GetAsync(token)).ToArray();
    }
}
