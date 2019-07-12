using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using Rac.VOne.Data;
using Rac.VOne.Data.SqlServer;
using Rac.VOne.Common;
using Rac.VOne.Common.Logging;
using Rac.VOne.Common.Security;
using Rac.VOne.Common.TypeMapping;

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// DI 登録
    /// </summary>
    public class SimpleInjectorConfigurator
    {
        private readonly Container container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public SimpleInjectorConfigurator(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// 登録
        /// </summary>
        public void Configure()
        {
            ConfigureNLog();
            ConfigureAutoMapper();
            ConfigureWebApplication();
            ConfigureDbHelper();
        }

        private void ConfigureNLog()
        {
            var manager = new LogManagerAdapter();
            container.RegisterInstance<ILogManager>(manager);
        }
        private void ConfigureAutoMapper()
        {
            var mapper = new AutoMapperAdapter();
            AutoMapperConfig.Initialize(mapper);
            container.RegisterInstance<IAutoMapper>(mapper);
        }

        private void ConfigureWebApplication()
        {
            container.RegisterSingleton<ISetting, ApplicationSettings>();
            container.RegisterSingleton<IConnectionString, ApplicationSettings>();
            container.RegisterSingleton<ITimeOutSetter, ApplicationSettings>();
            container.RegisterSingleton<IHashAlgorithm, HashAlgorithm>();
            container.RegisterSingleton<IStringConnectionFactory, StringConnectionFactory>();
            container.RegisterSingleton<ITransactionScopeBuilder, TransactionScopeBuilder>();
            //Extensions.AuthorizationExtensions.Initilize(container);
            container.Register<IConnectionFactory, SqlConnectionFactory>(Lifestyle.Scoped);
        }

        private void ConfigureDbHelper()
        {
            container.Register<IDbHelper, DbHelper>(Lifestyle.Scoped);
            var timeoutSetting = System.Configuration.ConfigurationManager.AppSettings["QueryTimeoutSecond"];
            var commandTimeoutSecond = 0;
            if (int.TryParse(timeoutSetting, out commandTimeoutSecond))
                DbHelper.CommandTimeoutSecond = commandTimeoutSecond;
        }
    }
}
