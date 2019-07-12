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
    ///  EBファイル設定
    /// </summary>
    public class EBFileSettingController : ApiControllerAuthorized
    {
        private readonly IEBFileSettingProcessor ebFileSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public EBFileSettingController(
            IEBFileSettingProcessor ebFileSettingProcessor
            )
        {
            this.ebFileSettingProcessor = ebFileSettingProcessor;
        }


        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="setting">EBファイル設定ID を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(EBFileSetting setting, CancellationToken token)
            => await ebFileSettingProcessor.DeleteAsync(setting.Id, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">会社ID または EBファイル設定ID の配列を指定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<EBFileSetting>> GetItems(EBFileSettingSearch option, CancellationToken token)
            => (await ebFileSettingProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="setting">EBファイル設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<EBFileSetting> Save(EBFileSetting setting, CancellationToken token)
            => await ebFileSettingProcessor.SaveAsync(setting, token);

        /// <summary>
        /// 利用可否変更
        /// </summary>
        /// <param name="option">利用可とする EBファイル設定ID 配列
        /// 会社ID、ログインユーザーIDを設定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateIsUseable(EBFileSettingSearch option, CancellationToken token)
            => await ebFileSettingProcessor.UpdateIsUseableAsync(option.CompanyId.Value, option.LoginUserId.Value, option.UpdateIds);
    }

}
