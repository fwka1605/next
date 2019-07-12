using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using System.Web.Http.ExceptionHandling;
using Rac.VOne.Web.Api.Legacy.Library;

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// Web API の設定
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 設定の登録 routingなど
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API の設定およびサービス
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            //log
            config.Services.Replace(typeof(IExceptionHandler), new CustomizeExceptionHandler(config.Services.GetExceptionHandler()));
            config.Services.Replace(typeof(IExceptionLogger), new CustomizeExceptionLogger());

            // Web API ルート
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
