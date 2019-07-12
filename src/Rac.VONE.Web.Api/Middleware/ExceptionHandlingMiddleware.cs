using Microsoft.AspNetCore.Http;
using NLog;
using Rac.VOne.Common.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Utf8Json;

namespace Rac.VOne.Web.Api.Middleware
{
    /// <summary>
    /// 例外をハンドリングする middleware
    /// </summary>
    /// <remarks>
    /// 統一的な例外処理
    /// 例外発生時は、HttpStatusCode 500 を返し、例外を json 形式で返す
    /// </remarks>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        /// <summary>constructor</summary>
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogManager logManager
            )
        {
            this.next = next;
            this.logger = logManager.GetLogger(typeof(ExceptionHandlingMiddleware));
        }

        /// <summary>error handling の実態 </summary>
        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Global Handler Catched.");
                await HandleExceptionAsync(context, ex);
                //throw;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            // TODO: to subdivide
            var result = "";
#if DEBUG
                // if DEBUG contains stacktrace
            result = JsonSerializer.ToJsonString(exception);
#else
            result = JsonSerializer.ToJsonString(new { ClassName = exception.GetType().Name, exception.Source, exception.Message,  });
#endif

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
}
