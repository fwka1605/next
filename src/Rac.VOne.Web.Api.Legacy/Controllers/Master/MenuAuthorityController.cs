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
    /// メニュー権限マスター
    /// </summary>
    public class MenuAuthorityController : ApiControllerAuthorized
    {
        private readonly IMenuAuthorityProcessor menuAuthorityProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public MenuAuthorityController(
            IMenuAuthorityProcessor menuAuthorityProcessor
            )
        {
            this.menuAuthorityProcessor = menuAuthorityProcessor;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="option">会社ID を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(MenuAuthoritySearch option, CancellationToken token)
            => await menuAuthorityProcessor.DeleteAsync(option.CompanyId.Value, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">会社ID, ログインユーザーID を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MenuAuthority>> GetItems(MenuAuthoritySearch option, CancellationToken token)
            => (await menuAuthorityProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MenuAuthority>> Save(IEnumerable<MenuAuthority> menus, CancellationToken token)
            => (await menuAuthorityProcessor.SaveAsync(menus, token)).ToArray();

    }
}
