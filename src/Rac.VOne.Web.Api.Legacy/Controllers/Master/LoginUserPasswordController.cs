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
    ///  ログインユーザーパスワード
    /// </summary>
    public class LoginUserPasswordController : ApiControllerAuthorized
    {
        private readonly ILoginUserPasswordProcessor loginUserPasswordProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public LoginUserPasswordController(
            ILoginUserPasswordProcessor loginUserPasswordProcessor
            )
        {
            this.loginUserPasswordProcessor = loginUserPasswordProcessor;
        }

        /// <summary>
        /// パスワード変更
        /// </summary>
        /// <param name="parameters">CompanyId, LoginUserId, OldPassword, Password の指定が必須</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PasswordChangeResult> Change(LoginParameters parameters, CancellationToken token)
            => await loginUserPasswordProcessor.ChangeAsync(parameters.CompanyId.Value, parameters.LoginUserId.Value, parameters.OldPassword, parameters.Password);

        /// <summary>
        /// ログイン処理が別にあるので、不要
        /// </summary>
        /// <param name="parameters">CompanyId, LoginUserId, Password の指定が必須</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginResult> Login(LoginParameters parameters, CancellationToken token)
            => await loginUserPasswordProcessor.LoginAsync(parameters.CompanyId.Value, parameters.LoginUserId.Value, parameters.Password, token);


        /// <summary>
        /// パスワード登録
        /// </summary>
        /// <param name="parameters">CompanyId, LoginUserId, Password の指定が必須</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PasswordChangeResult> Save(LoginParameters parameters, CancellationToken token)
            => await loginUserPasswordProcessor.SaveAsync(parameters.CompanyId.Value, parameters.LoginUserId.Value, parameters.Password, token);
    }
}
