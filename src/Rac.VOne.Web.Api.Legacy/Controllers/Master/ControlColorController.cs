using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  色マスター
    /// </summary>
    public class ControlColorController : ApiControllerAuthorized
    {
        private readonly IControlColorProcessor controlcolorProcessor;

        /// <summary>constructor</summary>
        public ControlColorController(
            IControlColorProcessor controlcolorProcessor
            )
        {
            this.controlcolorProcessor = controlcolorProcessor;
        }

        /// <summary>色マスター取得</summary>
        /// <param name="color">検索条件  会社ID、ログインユーザーID を設定する</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ControlColor> Get(ControlColor color, CancellationToken token)
            => await controlcolorProcessor.GetAsync(color.CompanyId, color.LoginUserId, token);

        /// <summary>色マスター登録</summary>
        /// <param name="color">色マスター</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ControlColor> Save(ControlColor color, CancellationToken token)
            => await controlcolorProcessor.SaveAsync(color, token);

    }
}
