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
    ///  締め処理
    /// </summary>
    public class ClosingController : ApiControllerAuthorized
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
        public async Task<ClosingInformation> GetClosingInformation([FromBody] int companyId, CancellationToken token)
            => await closingProcessor.GetClosingInformationAsync(companyId, token);

        /// <summary>
        /// 履歴取得
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ClosingHistory>> GetClosingHistory([FromBody] int companyId, CancellationToken token)
            => (await closingProcessor.GetClosingHistoryAsync(companyId, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="closing"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Closing> Save([FromBody] Closing closing, CancellationToken token)
            => await closingProcessor.SaveAsync(closing, token);

        /// <summary>
        ///  削除
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete([FromBody] int companyId, CancellationToken token)
            => await closingProcessor.DeleteAsync(companyId, token);

    }
}
