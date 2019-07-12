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
    ///  ログインユーザー ライセンス
    /// </summary>
    public class LoginUserLicenseController : ApiControllerAuthorized
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
        public async Task<IEnumerable<LoginUserLicense>> GetItems(LoginUserLicense license, CancellationToken token)
            => (await loginUserLicenseProcessor.GetAsync(license.CompanyId, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="licenses">ログインユーザーライセンスの配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<LoginUserLicense>> Save(IEnumerable< LoginUserLicense > licenses, CancellationToken token)
            => (await loginUserLicenseProcessor.SaveAsync(licenses, token)).ToArray();
    }
}
