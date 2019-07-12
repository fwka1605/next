using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Rac.VOne.Web.Api
{
    /// <summary></summary>
    public class Program
    {
        /// <summary>web api のエントリポイント</summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "stopped program because of exception.");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit.
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                /* configuration logging  */
                .ConfigureLogging(logging => logging
                    .ClearProviders()
                    .SetMinimumLevel(LogLevel.Trace))
                .UseNLog()
            ;
    }
}
