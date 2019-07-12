using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Api.Hubs;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  回収予定表
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollectionScheduleController : ControllerBase
    {
        private readonly ICollectionScheduleProcessor collectionScheduleProcessor;
        private readonly IHubContext<ProgressHub> hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        public CollectionScheduleController(
            ICollectionScheduleProcessor collectionScheduleProcessor,
            IHubContext<ProgressHub> hubContext
            )
        {
            this.collectionScheduleProcessor = collectionScheduleProcessor;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< CollectionSchedule>>> GetR(CollectionScheduleSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token)
                =>(await collectionScheduleProcessor.GetAsync(option, token, notifier)).ToArray());

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CollectionSchedule>>> Get(CollectionScheduleSearch option, CancellationToken token)
            => (await collectionScheduleProcessor.GetAsync(option, token, null)).ToArray();

    }
}
