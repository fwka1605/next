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
    ///  データ削除
    /// </summary>
    public class DataMaintenanceController : ApiControllerAuthorized
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
        public async Task<int> DeleteData([FromBody] DateTime deleteDate, CancellationToken token)
            => await dataMaintenanceProcessor.DeleteDataAsync(deleteDate, token);

    }
}
