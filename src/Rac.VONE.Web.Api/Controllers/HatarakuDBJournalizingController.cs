using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  働くDB 仕訳
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HatarakuDBJournalizingController : ControllerBase
    {
        private readonly IHatarakuDBJournalizingProcessor hatarakuDBJournalizingProcessor;
        /// <summary>
        /// constructor
        /// </summary>
        public HatarakuDBJournalizingController(
            IHatarakuDBJournalizingProcessor hatarakuDBJournalizingProcessor
            )
        {
            this.hatarakuDBJournalizingProcessor = hatarakuDBJournalizingProcessor;
        }

        /// <summary>
        /// 履歴取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<JournalizingSummary>>> GetSummaryAsync(JournalizingOption option)
            => (await hatarakuDBJournalizingProcessor.GetSummaryAsync(option)).ToArray();

        /// <summary>
        /// 抽出
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<HatarakuDBData>>> ExtractAsync(JournalizingOption option)
            => (await hatarakuDBJournalizingProcessor.ExtractAsync(option)).ToArray();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdateAsync(JournalizingOption option)
            => await hatarakuDBJournalizingProcessor.UpdateAsync(option);

        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CancelAsync(JournalizingOption option)
            => await hatarakuDBJournalizingProcessor.CancelAsync(option);

    }
}
