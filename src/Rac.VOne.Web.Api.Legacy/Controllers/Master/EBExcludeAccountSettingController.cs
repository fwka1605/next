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
    ///  EBデータ取込対象外口座設定
    /// </summary>
    public class EBExcludeAccountSettingController : ApiControllerAuthorized
    {
        private readonly IEBExcludeAccountSettingProcessor ebExcludeAccountSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public EBExcludeAccountSettingController(
            IEBExcludeAccountSettingProcessor ebExcludeAccountSettingProcessor
            )
        {
            this.ebExcludeAccountSettingProcessor = ebExcludeAccountSettingProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<EBExcludeAccountSetting>> GetItems([FromBody] int companyId, CancellationToken token)
            => (await ebExcludeAccountSettingProcessor.GetAsync(companyId)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="setting">EBデータ取込対象外口座設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<EBExcludeAccountSetting> Save(EBExcludeAccountSetting setting, CancellationToken token)
            => await ebExcludeAccountSettingProcessor.SaveAsync(setting, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="setting">EBデータ取込対象外口座設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns>削除されたレコード件数</returns>
        [HttpPost]
        public async Task<int> Delete(EBExcludeAccountSetting setting, CancellationToken token)
            => await ebExcludeAccountSettingProcessor.DeleteAsync(setting, token);
    }
}
