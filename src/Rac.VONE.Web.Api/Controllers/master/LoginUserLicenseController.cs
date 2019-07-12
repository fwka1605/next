using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  ログインユーザー ライセンス
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginUserLicenseController : ControllerBase
    {
        private readonly ILoginUserLicenseProcessor loginUserLicenseProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public LoginUserLicenseController(
            ILoginUserLicenseProcessor loginUserLicenseProcessor
            )
        {
            this.loginUserLicenseProcessor = loginUserLicenseProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="license">会社ID CompanyId を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<LoginUserLicense>>> GetItems(LoginUserLicense license, CancellationToken token)
            => (await loginUserLicenseProcessor.GetAsync(license.CompanyId, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="licenses">ログインユーザーライセンスの配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<LoginUserLicense>>> Save(IEnumerable< LoginUserLicense > licenses, CancellationToken token)
            => (await loginUserLicenseProcessor.SaveAsync(licenses, token)).ToArray();
    }
}
