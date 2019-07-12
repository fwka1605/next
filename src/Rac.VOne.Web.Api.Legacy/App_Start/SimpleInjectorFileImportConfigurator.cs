using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Import;
using SimpleInjector;

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// インポート処理のDI設定
    /// </summary>
    public class SimpleInjectorFileImportConfigurator
    {
        private readonly Container container;

        /// <summary>constructor</summary>
        public SimpleInjectorFileImportConfigurator(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// 構成
        /// </summary>
        public void Configure()
        {
            container.Register<IAccountTitleFileImportProcessor             , AccountTitleFileImportProcessor>();
            container.Register<IBankAccountFileImportProcessor              , BankAccountFileImportProcessor>();
            container.Register<IBankBranchFileImportProcessor               , BankBranchFileImportProcessor>();
            container.Register<ICustomerFileImportProcessor                 , CustomerFileImportProcessor>();
            container.Register<ICustomerDiscountFileImportProcessor         , CustomerDiscountFileImportProcessor>();
            container.Register<ICustomerFeeFileImportProcessor              , CustomerFeeFileImportProcessor>();
            container.Register<ICustomerGroupFileImportProcessor            , CustomerGroupFileImportProcessor>();
            container.Register<IDepartmentFileImportProcessor               , DepartmentFileImportProcessor>();
            container.Register<IHolidayCalendarFileImportProcessor          , HolidayCalendarFileImportProcessor>();
            container.Register<IIgnoreKanaFileImportProcessor               , IgnoreKanaFileImportProcessor>();
            container.Register<IJuridicalPersonalityFileImportProcessor     , JuridicalPersonalityFileImportProcessor>();
            container.Register<IKanaHistoryCustomerFileImportProcessor      , KanaHistoryCustomerFileImportProcessor>();
            container.Register<IKanaHistoryPaymentAgencyFileImportProcessor , KanaHistoryPaymentAgencyFileImportProcessor>();
            container.Register<ILoginUserFileImportProcessor                , LoginUserFileImportProcessor>();
            container.Register<ISectionFileImportProcessor                  , SectionFileImportProcessor>();
            container.Register<ISectionWithDepartmentFileImportProcessor    , SectionWithDepartmentFileImportProcessor>();
            container.Register<ISectionWithLoginUserFileImportProcessor     , SectionWithLoginUserFileImportProcessor>();
            container.Register<IStaffFileImportProcessor                    , StaffFileImportProcessor>();

            container.Register<IEbFileImportProcessor                       , EbFileImportProcessor>();
            container.Register<IBillingAccountTransferFileImportProcessor   , BillingAccountTransferFileImportProcessor>();
            container.Register<IBillingFileImportProcessor                  , BillingFileImportProcessor>();
            container.Register<IPaymentScheduleFileImportProcessor          , PaymentScheduleFileImportProcessor>();
            container.Register<IReceiptFileImportProcessor                  , ReceiptFileImportProcessor>();
        }
    }
}