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
    ///  締め処理
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClosingController : ControllerBase
    {
        private readonly IClosingProcessor closingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ClosingController(
            IClosingProcessor closingProcessor
            )
        {
            this.closingProcessor = closingProcessor;
        }

        /// <summary>
        /// 締め情報の取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ClosingInformation>> GetClosingInformation([FromBody] int companyId, CancellationToken token)
            => await closingProcessor.GetClosingInformationAsync(companyId, token);

        /// <summary>
        /// 履歴取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ClosingHistory>>> GetClosingHistory([FromBody] int companyId, CancellationToken token)
            => (await closingProcessor.GetClosingHistoryAsync(companyId, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="closing"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Closing>> Save([FromBody] Closing closing, CancellationToken token)
            => await closingProcessor.SaveAsync(closing, token);

        /// <summary>
        ///  削除
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete([FromBody] int companyId, CancellationToken token)
            => await closingProcessor.DeleteAsync(companyId, token);

    }
}
