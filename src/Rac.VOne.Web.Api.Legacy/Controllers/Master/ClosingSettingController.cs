using System.Web.Http;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  締め処理設定
    /// </summary>
    public class ClosingSettingController : ApiControllerAuthorized
    {
        private readonly IClosingSettingProcessor closingSettingProcessor;

        /// <summary>constructor</summary>
        public ClosingSettingController(
            IClosingSettingProcessor closingSettingProcessor
            )
        {
            this.closingSettingProcessor = closingSettingProcessor;
        }

        /// <summary>締め処理設定の取得</summary>
        /// <param name="setting">会社IDを指定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ClosingSetting> Get(ClosingSetting setting, CancellationToken token)
            => await closingSettingProcessor.GetAsync(setting.CompanyId, token);

        /// <summary>締め処理設定の登録</summary>
        /// <param name="setting">締め処理設定</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ClosingSetting> Save(ClosingSetting setting, CancellationToken token)
            => await closingSettingProcessor.SaveAsync(setting, token);


    }
}
