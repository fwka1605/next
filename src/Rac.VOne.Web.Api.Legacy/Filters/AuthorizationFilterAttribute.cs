using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Rac.VOne.Web.Common;
using static Rac.VOne.Web.Api.Legacy.Constants;

using Newtonsoft.Json;

namespace Rac.VOne.Web.Api.Legacy.Filters
{
    /// <summary>
    /// 認可処理 filter attribute
    /// </summary>
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        /// <summary></summary>
        public AuthorizationFilterAttribute()
        {
        }

        /// <summary>
        /// 認可処理の実態
        /// </summary>
        /// <returns></returns>
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var processor = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IAuthorizationProcessor)) as IAuthorizationProcessor;

            var access_token = "";
            if (actionContext.Request.Headers.TryGetValues(VOneAccessTokenKey, out var values)) access_token = values.FirstOrDefault();

            var result = await processor.AuthorizeAsync(access_token);

            if (!result.Item1.Result)
            {
                actionContext.Response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                var json = JsonConvert.SerializeObject(result.Item1);
                actionContext.Response.Content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
                return;
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

    }
}