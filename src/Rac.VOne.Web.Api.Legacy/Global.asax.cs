using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.WebApi;


namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            new SimpleInjectorConfigurator(container).Configure();
            new SimpleInjectorWebCommonProcessorConfigurator(container).Configure();
            new SimpleInjectorQueryProcessorConfigurator(container).Configure();
            new SimpleInjectorFileImportConfigurator(container).Configure();
            new SimpleInjectorSpreadsheetProcessorConfigurator(container).Configure();

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);


            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver
                = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
