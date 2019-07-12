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
    ///  照合設定
    /// </summary>
    public class CollationSettingController : ApiControllerAuthorized
    {
        private readonly ICollationSettingProcessor collationSettingProcessor;

        /// <summary>constructor</summary>
        public CollationSettingController(
            ICollationSettingProcessor collationSettingProcessor
            )
        {
            this.collationSettingProcessor = collationSettingProcessor;
        }

        /// <summary>照合設定 取得</summary>
        /// <param name="setting">会社IDを指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<CollationSetting> Get(CollationSetting setting, CancellationToken token)
            => await collationSettingProcessor.GetAsync(setting.CompanyId, token);

        /// <summary>請求側 消込順序設定取得</summary>
        /// <param name="setting">会社IDを指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<IEnumerable<MatchingOrder>> GetMatchingBillingOrder(CollationSetting setting, CancellationToken token)
            => (await collationSettingProcessor.GetMatchingBillingOrderAsync(setting.CompanyId)).ToArray();

        /// <summary>入金側 消込順序設定取得</summary>
        /// <param name="setting">会社IDを指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<IEnumerable<MatchingOrder>> GetMatchingReceiptOrder(CollationSetting setting, CancellationToken token)
            => (await collationSettingProcessor.GetMatchingReceiptOrderAsync(setting.CompanyId)).ToArray();

        /// <summary>照合順序設定取得</summary>
        /// <param name="setting">会社IDを指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<IEnumerable<CollationOrder>> GetCollationOrder(CollationSetting setting, CancellationToken token)
            => (await collationSettingProcessor.GetCollationOrderAsync(setting.CompanyId)).ToArray();

        /// <summary>照合設定 登録</summary>
        /// <param name="setting">照合設定
        /// collationOrders : 照合順序
        /// billingMatchingOrders : 請求側消込順序
        /// receiptMatchingOrders : 入金側消込順序
        /// 上記へ値を設定したものを連携する
        /// </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CollationSetting> Save(CollationSetting setting, CancellationToken token)
            => await collationSettingProcessor.SaveAsync(setting, token);

    }
}
