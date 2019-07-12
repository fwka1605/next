using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Rac.VOne.Web.Common;
using static Rac.VOne.Web.Api.Legacy.Constants;

namespace Rac.VOne.Web.Api.Legacy.Filters
{
    /// <summary>
    /// 認証処理
    /// </summary>
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// constructor
        /// </summary>
        public AuthenticationFilterAttribute()
        {
        }


        /// <summary>
        /// 認証処理 Web API 処理実行前の
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var processor = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IAuthenticationWebApiProcessor)) as IAuthenticationWebApiProcessor;
            var key = "";
            var code = "";
            var headers = actionContext.Request.Headers;
            if (headers.TryGetValues(VOneAuthenticationKey, out var val1)) key  = val1.FirstOrDefault();
            if (headers.TryGetValues(VOneTenantCode       , out var val2)) code = val2.FirstOrDefault();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(code))
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Forbidden);
                return;
            }

            var result = await processor.AuthenticateAsync(key, code, cancellationToken);
            if (!result.Result)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }


    }
}