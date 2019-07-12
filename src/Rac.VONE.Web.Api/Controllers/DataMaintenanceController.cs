using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  データ削除
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataMaintenanceController : ControllerBase
    {
        private IDataMaintenanceProcessor dataMaintenanceProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dataMaintenanceProcessor"></param>
        public DataMaintenanceController(
            IDataMaintenanceProcessor dataMaintenanceProcessor
            )
        {
            this.dataMaintenanceProcessor = dataMaintenanceProcessor;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="deleteDate"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> DeleteData([FromBody] DateTime deleteDate, CancellationToken token)
            => await dataMaintenanceProcessor.DeleteDataAsync(deleteDate, token);

    }
}
