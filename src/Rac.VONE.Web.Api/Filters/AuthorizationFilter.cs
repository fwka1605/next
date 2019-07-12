using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Common;
using System.Linq;
using System.Threading.Tasks;
using static Rac.VOne.Web.Api.Constants;


namespace Rac.VOne.Web.Api.Filters
{
    /// <summary>
    /// 認可処理
    /// 
    /// access_token の 登録を確認し、web api へのアクセスを認可する
    /// </summary>
    public class AuthorizationFilter : IAsyncActionFilter
    {
        // wcf の 認可処理とことなる名前にする
        private readonly IAuthorizationProcessor authorizationProcessor;

        /// <summary>コンストラクタ</summary>
        public AuthorizationFilter(
            IAuthorizationProcessor authorizationProcessor
            )
        {
            this.authorizationProcessor = authorizationProcessor;
        }

        /// <summary>認可処理の実態</summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var skip = context.ActionDescriptor
                .FilterDescriptors.Any(x => x.Filter.GetType() == typeof(SkipAuthorizationFilterAttribute));

            if (!skip)
            {
                var dic = context.HttpContext.GetRequestHeaders(new[] { VOneAccessTokenKey });
                var token = "";
                dic.TryGetValue(VOneAccessTokenKey, out token);

                var result = await authorizationProcessor.AuthorizeAsync(token);

                if (!result.Item1.Result)
                {
                    context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    var json = ConvertToJson(result.Item1);
                    await context.HttpContext.Response.WriteAsync(json);
                    return;
                }
            }
            await next.Invoke();
        }

        private string ConvertToJson<T>(T result)
        {
            var json = "";
            using (var stream = new System.IO.MemoryStream())
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, result);
                json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
            return json;
        }


    }
}
