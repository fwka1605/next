using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Api.Filters;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Api.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// ログイン画面から呼び出されるAPI
    /// IP 制限付き
    /// </summary>
    /// <remarks>
    /// <see cref="ApiControllerAttribute"/> を 付けると、<see cref="RouteAttribute"/> が必須となる
    /// <see cref="ApiControllerAttribute"/> があると、Swagger で API が表示される 表示する必要が無いものは、無くてよい？
    /// <see cref="RouteAttribute"/> が自動生成されるものを惰性で利用するのは回避したい
    /// 
    /// 残タスク
    /// ・ログイン処理
    /// ・セッションキー取得処理
    /// ・セッションキー払出処理
    /// </remarks>
    [TypeFilter(typeof(AuthenticationFilter))]
    [SkipAuthorizationFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly ILoginUserPasswordProcessor loginUserPasswordProcessor;

        private readonly IAuthenticationWebApiProcessor authenticationWebApiProcessor;


        /// <summary>
        /// constructor
        /// </summary>
        public LoginController(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            ILoginUserPasswordProcessor loginUserPasswordProcessor,
            IAuthenticationWebApiProcessor authenticationWebApiProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.loginUserPasswordProcessor = loginUserPasswordProcessor;
            this.authenticationWebApiProcessor = authenticationWebApiProcessor;
        }

        /// <summary>
        /// 会社一覧取得
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
            => (await companyProcessor.GetAsync(new CompanySearch())).ToArray();


        /// <summary>
        /// TenantCode の検証処理 <see cref="AuthenticationFilter"/>で
        /// 誤った テナントコードを連携した場合 403 となるので、単に true を返す API を用意すればよい
        /// HttpPost だが、必要になる request.body に設定するパラメータも存在しない
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<bool> ValidateTenantCode()
        {
            return true;
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <param name="parameters">CompanyCode, UserCode, Password の指定が必須 </param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<WebApiLoginResult>> Login(LoginParameters parameters, CancellationToken token)
        {
            var result = new WebApiLoginResult {
                LoginResult             = (int)LoginResult.Failed,
                PasswordChangeResult  = (int)PasswordValidateResult.Valid,
            };

            var company = (await companyProcessor.GetAsync(new CompanySearch { Code = parameters.CompanyCode }, token))?.FirstOrDefault();
            if (company == null) return result;

            var loginUser = (await loginUserProcessor.GetAsync(new LoginUserSearch {
                CompanyId   = company.Id,
                Codes       = new[] { parameters.UserCode },
            }, token))?.FirstOrDefault();
            if (loginUser == null) return result;

            var loginResult = await loginUserPasswordProcessor.LoginAsync(company.Id, loginUser.Id, parameters.Password, token);
            result.LoginResult = (int)loginResult;
            if (loginResult != LoginResult.Success && loginResult != LoginResult.Expired) return result;

            var authKey     = string.Empty;
            var tenantCode  = string.Empty;
            if (!ParseHeader(out authKey, out tenantCode))
            {
                result.LoginResult = (int)LoginResult.Failed;
                return result;
            }
            var createResult = await authenticationWebApiProcessor.CreateSessionAsync(tenantCode, token);
            if (!createResult.Result)
            {
                result.LoginResult = (int)LoginResult.DBError;
                return result;
            }
            result.SessionKey = createResult.ErrorMessage;

            return result;
        }


        /// <summary>
        /// パスワード変更処理
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<WebApiLoginResult>> ChangePassword(LoginParameters parameters, CancellationToken token)
        {
            var result = new WebApiLoginResult {
                LoginResult             = (int)LoginResult.Failed,
                PasswordChangeResult    = (int)PasswordChangeResult.Failed,
            };

            var company = (await companyProcessor.GetAsync(new CompanySearch { Code = parameters.CompanyCode }, token))?.FirstOrDefault();
            if (company == null) return result;

            var loginUser = (await loginUserProcessor.GetAsync(new LoginUserSearch
            {
                CompanyId = company.Id,
                Codes = new[] { parameters.UserCode },
            }, token))?.FirstOrDefault();
            if (loginUser == null) return result;

            var changeResult = await loginUserPasswordProcessor.ChangeAsync(company.Id, loginUser.Id, parameters.OldPassword, parameters.Password, token);
            result.PasswordChangeResult = (int)changeResult;
            return result;
        }


        private bool ParseHeader(out string authKey, out string tenantCode)
        {
            authKey     = string.Empty;
            tenantCode  = string.Empty;

            var dic = HttpContext.GetRequestHeaders(new[] { Constants.VOneAuthenticationKey, Constants.VOneTenantCode });

            dic.TryGetValue(Constants.VOneAuthenticationKey , out authKey);
            dic.TryGetValue(Constants.VOneTenantCode        , out tenantCode);

            return !string.IsNullOrEmpty(authKey) &&
                   !string.IsNullOrEmpty(tenantCode);
        }

    }
}