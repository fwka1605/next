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
    ///  グリッド表示設定マスター
    /// </summary>
    public class GridSettingController : ApiControllerAuthorized
    {
        private readonly IGridSettingProcessor gridSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public GridSettingController(
            IGridSettingProcessor gridSettingProcessor
            )
        {
            this.gridSettingProcessor = gridSettingProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="settings">グリッド設定 配列
        /// すべてのユーザーへ反映したい場合、配列の最初の <see cref="GridSetting.AllLoginUser"/>を true にする
        /// todo: test 未設定のユーザーには何も反映されないのだが...
        /// </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<GridSetting>> Save(IEnumerable<GridSetting> settings, CancellationToken token)
            => (await gridSettingProcessor.SaveAsync(settings, token)).ToArray();

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">会社ID,ログインユーザーID,グリッドID(任意),初期値が欲しい場合は<see cref="GridSettingSearch.IsDefault"/>をture にして問合せ</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<GridSetting>> GetItems(GridSettingSearch option, CancellationToken token)
            => (await gridSettingProcessor.GetAsync(option, token)).ToArray();


    }
}
