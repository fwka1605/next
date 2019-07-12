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
    /// 定期請求マスター
    /// </summary>
    public class PeriodicBillingSettingController : ApiControllerAuthorized
    {
        private readonly IPeriodicBillingSettingProcessor periodicBillingSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public PeriodicBillingSettingController(
            IPeriodicBillingSettingProcessor periodicBillingSettingProcessor
            )
        {
            this.periodicBillingSettingProcessor = periodicBillingSettingProcessor;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(long id, CancellationToken token)
            => await periodicBillingSettingProcessor.DeleteAsync(id, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<PeriodicBillingSetting>> GetItems(PeriodicBillingSettingSearch option, CancellationToken token)
            => (await periodicBillingSettingProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task< PeriodicBillingSetting > Save(PeriodicBillingSetting setting, CancellationToken token)
            => await periodicBillingSettingProcessor.SaveAsync(setting, token);
    }
}
