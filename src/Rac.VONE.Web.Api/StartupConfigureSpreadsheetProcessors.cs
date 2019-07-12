using Microsoft.Extensions.DependencyInjection;
using Rac.VOne.Web.Common.Spreadsheets;
using Rac.VOne.Web.Spreadsheets;

namespace Rac.VOne.Web.Api
{
    /// <summary>Spreadsheet 作成の DI 定義</summary>
    public static class StartupConfigureSpreadsheetProcessors
    {
        /// <summary>
        /// Spreadsheet 作成の DI設定を行うメソッド
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSpreadsheetProcessors(this IServiceCollection services)
        {
            services.AddTransient<IArrearagesListSpreadsheetProcessor, ArrearagesListSpreadsheetProcessor>();
            services.AddTransient<IBillingAgingListSpreadsheetProcessor, BillingAgingListShpreadsheetProcessor>();
            return services;
        }
    }
}
