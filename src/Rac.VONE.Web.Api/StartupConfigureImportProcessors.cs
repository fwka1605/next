using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Import;

namespace Rac.VOne.Web.Api
{
    /// <summary>
    /// ImportProcessor の DI
    /// </summary>
    public static class StartupConfigureImportProcessors
    {
        /// <summary>
        /// インポート処理の DI 設定を行う拡張メソッド
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureImportProcessors(this IServiceCollection services)
        {
            services.AddTransient<IAccountTitleFileImportProcessor             , AccountTitleFileImportProcessor>();
            services.AddTransient<IBankAccountFileImportProcessor              , BankAccountFileImportProcessor>();
            services.AddTransient<IBankBranchFileImportProcessor               , BankBranchFileImportProcessor>();
            services.AddTransient<ICustomerFileImportProcessor                 , CustomerFileImportProcessor>();
            services.AddTransient<ICustomerDiscountFileImportProcessor         , CustomerDiscountFileImportProcessor>();
            services.AddTransient<ICustomerFeeFileImportProcessor              , CustomerFeeFileImportProcessor>();
            services.AddTransient<ICustomerGroupFileImportProcessor            , CustomerGroupFileImportProcessor>();
            services.AddTransient<IDepartmentFileImportProcessor               , DepartmentFileImportProcessor>();
            services.AddTransient<IHolidayCalendarFileImportProcessor          , HolidayCalendarFileImportProcessor>();
            services.AddTransient<IIgnoreKanaFileImportProcessor               , IgnoreKanaFileImportProcessor>();
            services.AddTransient<IJuridicalPersonalityFileImportProcessor     , JuridicalPersonalityFileImportProcessor>();
            services.AddTransient<IKanaHistoryCustomerFileImportProcessor      , KanaHistoryCustomerFileImportProcessor>();
            services.AddTransient<IKanaHistoryPaymentAgencyFileImportProcessor , KanaHistoryPaymentAgencyFileImportProcessor>();
            services.AddTransient<ILoginUserFileImportProcessor                , LoginUserFileImportProcessor>();
            services.AddTransient<ISectionFileImportProcessor                  , SectionFileImportProcessor>();
            services.AddTransient<ISectionWithDepartmentFileImportProcessor    , SectionWithDepartmentFileImportProcessor>();
            services.AddTransient<ISectionWithLoginUserFileImportProcessor     , SectionWithLoginUserFileImportProcessor>();
            services.AddTransient<IStaffFileImportProcessor                    , StaffFileImportProcessor>();

            services.AddTransient<IEbFileImportProcessor                       , EbFileImportProcessor>();
            services.AddTransient<IBillingAccountTransferFileImportProcessor   , BillingAccountTransferFileImportProcessor>();
            services.AddTransient<IBillingFileImportProcessor                  , BillingFileImportProcessor>();
            services.AddTransient<IPaymentScheduleFileImportProcessor          , PaymentScheduleFileImportProcessor>();
            services.AddTransient<IReceiptFileImportProcessor                  , ReceiptFileImportProcessor>();

            return services;
        }
    }
}
