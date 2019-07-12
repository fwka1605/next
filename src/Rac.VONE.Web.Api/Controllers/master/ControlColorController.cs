using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  色マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ControlColorController : ControllerBase
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
        public async Task<ActionResult<ControlColor>> Get(ControlColor color, CancellationToken token)
            => await controlcolorProcessor.GetAsync(color.CompanyId, color.LoginUserId, token);

        /// <summary>色マスター登録</summary>
        /// <param name="color">色マスター</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<ControlColor>> Save(ControlColor color, CancellationToken token)
            => await controlcolorProcessor.SaveAsync(color, token);

    }
}
