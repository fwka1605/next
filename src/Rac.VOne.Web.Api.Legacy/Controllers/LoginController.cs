using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Api.Legacy.Filters;
using Rac.VOne.Web.Api.Legacy.Extensions;
using System.Threading;
using System.Threading.Tasks;


namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// ログイン画面から呼び出されるAPI
    /// IP 制限付き
    /// </summary>
    /// <remarks>
    /// <see cref="RouteAttribute"/> が自動生成されるものを惰性で利用するのは回避したい
    /// 
    /// 残タスク
    /// ・ログイン処理
    /// ・セッションキー取得処理
    /// ・セッションキー払出処理
    /// </remarks>
    [AuthenticationFilter]
    public class LoginController : ApiController
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
        public async Task<IEnumerable<Company>> GetCompanies()
            => (await companyProcessor.GetAsync(new CompanySearch())).ToArray();


        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <returns>access_token の払出</returns>
        [HttpPost]
        public async Task<UnaryValue<string>> Login(LoginParameters parameters, CancellationToken token)
        {
            var company = (await companyProcessor.GetAsync(new CompanySearch {
                Code = parameters.CompanyCode,
            }, token)).FirstOrDefault() ?? throw new ArgumentException($"companyCode:{parameters.CompanyCode} is not exist.");

            // login user code to user id
            var loginUser = (await loginUserProcessor.GetAsync(new LoginUserSearch {
                CompanyId   = company.Id,
                Codes       = new[] { parameters.UserCode },
            })).FirstOrDefault() ?? throw new ArgumentException($"usercode:{parameters.UserCode} is not exist.");

            // confirm password
            var loginResult = await loginUserPasswordProcessor.LoginAsync(company.Id, loginUser.Id, parameters.Password, token);

            if (loginResult == LoginResult.Failed)
                throw new ArgumentException("login password incorrect.");

            if (loginResult == LoginResult.DBError)
                throw new ArgumentException("db exception");

            if (loginResult == LoginResult.Expired)
                throw new ArgumentException("login password expired.");

            var authKey         = string.Empty;
            var tenentCode      = string.Empty;
            if (!ParseHeader(out authKey, out tenentCode))
                throw new ArgumentException("request header is invalid.");

            // create session key
            var createResult = await authenticationWebApiProcessor.CreateSessionAsync(tenentCode, token);
            if (!createResult.Result)
                throw new InvalidOperationException($"create session failure:{createResult.ErrorMessage}");

            // publish session key /*TODO: ErrorMessage -> Message へ 変更 */
            return createResult.ErrorMessage.GetUnaryValue();
        }


        private bool ParseHeader(out string authKey, out string tenantCode)
        {
            authKey     = string.Empty;
            tenantCode  = string.Empty;

            if (Request.Headers.TryGetValues(Constants.VOneAuthenticationKey , out var val1 )) authKey      = val1.FirstOrDefault();
            if (Request.Headers.TryGetValues(Constants.VOneTenantCode        , out var val2 )) tenantCode   = val2.FirstOrDefault();

            return !string.IsNullOrEmpty(authKey) &&
                   !string.IsNullOrEmpty(tenantCode);
        }

    }
}