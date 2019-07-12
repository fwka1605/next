using Microsoft.Extensions.DependencyInjection;
using Rac.VOne.Common;
using Rac.VOne.Common.Logging;
using Rac.VOne.Common.Security;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Rac.VOne.Data.SqlServer;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Api.Library;

namespace Rac.VOne.Web.Api
{

    /// <summary>
    /// todo: rename 適切な名称
    /// </summary>
    public static class StartupConfigureDIContainer
    {
        /// <summary>
        /// DI 設定を行う 拡張メソッド
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection ConfigureDIContainer(this IServiceCollection services)
        {
            ConfigureNLog(services);
            ConfigureAutoMapper(services);
            ConfigureWebApplication(services);
            ConfigureDbHelper(services);
            services.AddSingleton<IIPWhiteList, IPWhiteList>();
            return services;
        }

        private static IServiceCollection ConfigureNLog(IServiceCollection services)
        {
            services.AddSingleton<ILogManager, LogManagerAdapter>();
            return services;
        }
        private static IServiceCollection ConfigureAutoMapper(IServiceCollection services)
        {
            var mapper = new AutoMapperAdapter();
            mapper.Initialize();
            services.AddSingleton<IAutoMapper>(mapper);
            return services;
        }

        private static IServiceCollection ConfigureWebApplication(IServiceCollection services)
        {
            services.AddSingleton<ISetting                  , ApplicationSettings>();
            services.AddSingleton<IConnectionString         , ApplicationSettings>();
            services.AddSingleton<ITimeOutSetter            , ApplicationSettings>();
            services.AddSingleton<IHashAlgorithm            , HashAlgorithm>();
            services.AddSingleton<IStringConnectionFactory  , StringConnectionFactory>();
            services.AddSingleton<ITransactionScopeBuilder  , TransactionScopeBuilder>();
            services.AddScoped   <IConnectionFactory        , SqlConnectionFactory>();
            return services;
        }

        private static IServiceCollection ConfigureDbHelper(IServiceCollection services)
        {
            services.AddScoped<IDbHelper, DbHelper>();
            var timeoutSetting = System.Configuration.ConfigurationManager.AppSettings["QueryTimeoutSecond"]; // todo : remove ConfigurationManager
            var commandTimeoutSecond = 0;
            if (int.TryParse(timeoutSetting, out commandTimeoutSecond))
                DbHelper.CommandTimeoutSecond = commandTimeoutSecond;
            return services;
        }

    }
}
