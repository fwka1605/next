using Microsoft.AspNetCore.Mvc.Filters;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Common;
using System.Threading.Tasks;
using static Rac.VOne.Web.Api.Constants;

namespace Rac.VOne.Web.Api.Filters
{
    /// <summary>
    /// 認証フィルタ
    /// </summary>
    public class AuthenticationFilter : IAsyncActionFilter
    {
        // authentication processor は wcf のものと別で用意する
        private readonly IAuthenticationWebApiProcessor authenticationProcessor;

        /// <summary>
        /// </summary>
        public AuthenticationFilter(IAuthenticationWebApiProcessor authenticationProcessor)
        {
            this.authenticationProcessor = authenticationProcessor;
        }

        /// <summary>
        ///  controller の action の前後に実行する 認証処理
        /// </summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var key = "";
            var code = "";

            var dic = context.HttpContext.GetRequestHeaders(new[] { VOneAuthenticationKey, VOneTenantCode });

            dic.TryGetValue(VOneAuthenticationKey, out key);
            dic.TryGetValue(VOneTenantCode       , out code);

            if (string.IsNullOrEmpty(key) ||
                string.IsNullOrEmpty(code))
            {
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return;
            }

            var result = await authenticationProcessor.AuthenticateAsync(key, code);

            if (! result.Result)
            {
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return;
            }

            await next.Invoke();
        }
    }
}
