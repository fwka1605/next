using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api
{
    using Microsoft.Extensions.DependencyInjection;
    using Rac.VOne.Web.Common;
    using Rac.VOne.Web.Common.ThirdPartyApis;

    /// <summary>
    /// <see cref="Rac.VOne.Web.Common"/>の DI設定
    /// </summary>
    public static class StartupConfigureProcessors
    {
        /// <summary>
        /// <see cref="Rac.VOne.Web.Common"/>の DI設定 拡張メソッド
        /// </summary>
        public static IServiceCollection ConfigureProcessors(this IServiceCollection services)
        {
            Configure(services);
            return services;
        }

        private static IServiceCollection Configure(IServiceCollection services)
        {
            services.AddTransient<IAuthenticationProcessor          , AuthenticationProcessor>();
            services.AddTransient<IAuthenticationWebApiProcessor    , AuthenticationWebApiProcessor>();
            services.AddTransient<IAuthorizationProcessor           , AuthorizationProcessor>();

            services.AddTransient<IAccountTitleProcessor            , AccountTitleProcessor>();
            services.AddTransient<IAccountTransferProcessor         , AccountTransferProcessor>();
            services.AddTransient<IAdvanceReceivedBackupProcessor   , AdvanceReceivedBackupProcessor>();
            services.AddTransient<IApplicationControlProcessor      , ApplicationControlProcessor>();

            services.AddTransient<IBankAccountProcessor             , BankAccountProcessor>();
            services.AddTransient<IBankAccountTypeProcessor         , BankAccountTypeProcessor>();
            services.AddTransient<IBankBranchProcessor              , BankBranchProcessor>();

            services.AddTransient<IBillingAgingListProcessor        , BillingAgingListProcessor>();

            services.AddTransient<IBillingBalanceBackProcessor      , BillingBalanceBackProcessor>();
            services.AddTransient<IBillingBalanceProcessor          , BillingBalanceProcessor>();
            services.AddTransient<IBillingDiscountProcessor         , BillingDiscountProcessor>();
            services.AddTransient<IBillingDivisionContractProcessor , BillingDivisionContractProcessor>();
            services.AddTransient<IBillingDivisionSettingProcessor  , BillingDivisionSettingProcessor>();
            services.AddTransient<IBillingDueAtModifyProcessor      , BillingDueAtModifyProcessor>();
            services.AddTransient<IBillingInputProcessor            , BillingInputProcessor>();
            services.AddTransient<IBillingInvoiceProcessor          , BillingInvoiceProcessor>();
            services.AddTransient<IBillingSaveProcessor             , BillingSaveProcessor>();
            services.AddTransient<IBillingSaveForInputProcessor     , BillingSaveProcessor>();
            services.AddTransient<IBillingProcessor                 , BillingProcessor>();
            services.AddTransient<IBillingJournalizingProcessor     , BillingJournalizingProcessor>();
            services.AddTransient<IBillingMemoProcessor             , BillingMemoProcessor>();
            services.AddTransient<IBillingScheduledIncomeProcessor  , BillingScheduledIncomeProcessor>();
            services.AddTransient<IBillingSearchProcessor           , BillingSearchProcessor>();
            services.AddTransient<IBillingImporterProcessor         , BillingImporterProcessor>();
            services.AddTransient<IBillingImporterCodeToIdSolveProcessor, BillingImporterCodeToIdSolveProcessor>();
            services.AddTransient<IBillingScheduledPaymentProcessor , BillingScheduledPaymentProcessor>();
            services.AddTransient<IBillingAccountTransferProcessor  , BillingAccountTransferProcessor>();

            services.AddTransient<ICategoryProcessor                , CategoryProcessor>();

            services.AddTransient<IClosingProcessor                 , ClosingProcessor>();
            services.AddTransient<IClosingSettingProcessor          , ClosingSettingProcessor>();

            services.AddTransient<ICollationSettingProcessor        , CollationSettingProcessor>();
            services.AddTransient<ICollectionScheduleProcessor      , CollectionScheduleProcessor>();
            services.AddTransient<IColumnNameSettingProcessor       , ColumnNameSettingProcessor>();

            services.AddTransient<IControlColorProcessor            , ControlColorProcessor>();

            services.AddTransient<ICompanyProcessor                 , CompanyProcessor>();
            services.AddTransient<ICompanyLogoProcessor             , CompanyLogoProcessor>();
            services.AddTransient<ICompanyInitializeProcessor       , CompanyInitializeProcessor>();

            services.AddTransient<ICreditAgingListProcessor         , CreditAgingListProcessor>();

            services.AddTransient<ICurrencyProcessor                , CurrencyProcessor>();
            services.AddTransient<ICustomerGroupProcessor           , CustomerGroupProcessor>();
            services.AddTransient<ICustomerDiscountProcessor        , CustomerDiscountProcessor>();
            services.AddTransient<ICustomerFeeProcessor             , CustomerFeeProcessor>();
            services.AddTransient<ICustomerLedgerProcessor          , CustomerLedgerProcessor>();
            services.AddTransient<ICustomerPaymentContractProcessor , CustomerPaymentContractProcessor>();
            services.AddTransient<ICustomerProcessor                , CustomerProcessor>();

            services.AddTransient<IDataMaintenanceProcessor         , DataMaintenanceProcessor>();

            services.AddTransient<IDbFunctionProcessor              , DbFunctionProcessor>();
            services.AddTransient<IDepartmentProcessor              , DepartmentProcessor>();
            services.AddTransient<IDestinationProcessor             , DestinationProcessor>();

            services.AddTransient<IEBExcludeAccountSettingProcessor , EBExcludeAccountSettingProcessor>();

            services.AddTransient<IEBFormatProcessor                , EBFormatProcessor>();
            services.AddTransient<IEBFileSettingProcessor           , EBFileSettingProcessor>();

            services.AddTransient<IExportFieldSettingProcessor      , ExportFieldSettingProcessor>();

            services.AddTransient<IFunctionAuthorityProcessor       , FunctionAuthorityProcessor>();

            services.AddTransient<IGeneralSettingProcessor          , GeneralSettingProcessor>();
            services.AddTransient<IGridSettingProcessor             , GridSettingProcessor>();

            services.AddTransient<IHolidayCalendarProcessor         , HolidayCalendarProcessor>();

            services.AddTransient<IIgnoreKanaProcessor              , IgnoreKanaProcessor>();

            services.AddTransient<IImporterSettingDetailProcessor   , ImporterSettingDetailProcessor>();
            services.AddTransient<IImporterSettingProcessor         , ImporterSettingProcessor>();

            services.AddTransient<IImportFileLogProcessor           , ImportFileLogProcessor>();

            services.AddTransient<IImportSettingProcessor           , ImportSettingProcessor>();

            services.AddTransient<IInputControlProcessor            , InputControlProcessor>();

            services.AddTransient<IInvoiceCommonSettingProcessor    , InvoiceCommonSettingProcessor>();
            services.AddTransient<IInvoiceNumberHistoryProcessor    , InvoiceNumberHistoryProcessor>();
            services.AddTransient<IInvoiceNumberSettingProcessor    , InvoiceNumberSettingProcessor>();
            services.AddTransient<IInvoiceTemplateSettingProcessor  , InvoiceTemplateSettingProcessor>();

            services.AddTransient<IJuridicalPersonalityProcessor    , JuridicalPersonalityProcessor>();

            services.AddTransient<IKanaHistoryCustomerProcessor     , KanaHistoryCustomerProcessor>();
            services.AddTransient<IKanaHistoryPaymentAgencyProcessor, KanaHistoryPaymentAgencyProcessor>();

            services.AddTransient<ILogDataProcessor                 , LogDataProcessor>();
            services.AddTransient<ILoginUserLicenseProcessor        , LoginUserLicenseProcessor>();
            services.AddTransient<ILoginUserPasswordProcessor       , LoginUserPasswordProcessor>();
            services.AddTransient<ILoginUserProcessor               , LoginUserProcessor>();
            services.AddTransient<ILogsProcessor                    , LogsProcessor>();

            services.AddTransient<ICollationProcessor               , CollationProcessor>();
            services.AddTransient<IMatchingCombinationSolveProcessor, MatchingCombinationSolveProcessor>();
            services.AddTransient<IMatchingHeaderProcessor          , MatchingHeaderProcessor>();
            services.AddTransient<IMatchingHistorySearchProcessor   , MatchingHistorySearchProcessor>();
            services.AddTransient<IMatchingOutputedProcessor        , MatchingOutputedProcessor>();
            services.AddTransient<IMatchingProcessor                , MatchingProcessor>();
            services.AddTransient<IMatchingSaveProcessor            , MatchingSaveProcessor>();
            services.AddTransient<IMatchingSequentialProcessor      , MatchingSequentialProcessor>();
            services.AddTransient<IMatchingIndividualProcessor      , MatchingIndividualProcessor>();
            services.AddTransient<IMatchingCancellationProcessor    , MatchingCancellationProcessor>();
            services.AddTransient<IMatchingSolveProcessor           , MatchingSolveProcessor>();
            services.AddTransient<IMatchingJournalizingProcessor    , MatchingJournalizingProcessor>();

            services.AddTransient<IMenuAuthorityProcessor           , MenuAuthorityProcessor>();

            services.AddTransient<IMFBillingProcessor               , MFBillingProcessor>();

            services.AddTransient<IMfAggrTagProcessor               , MfAggrTagProcessor>();
            services.AddTransient<IMfAggrAccountProcessor           , MfAggrAccountProcessor>();
            services.AddTransient<IMfAggrTransactionProcessor       , MfAggrTransactionProcessor>();

            services.AddTransient<INettingProcessor                 , NettingProcessor>();
            services.AddTransient<INettingSearchProcessor           , NettingSearchProcessor>();

            services.AddTransient<IPasswordPolicyProcessor          , PasswordPolicyProcessor>();

            services.AddTransient<IPaymentAgencyProcessor           , PaymentAgencyProcessor>();
            services.AddTransient<IPaymentAgencyFeeProcessor        , PaymentAgencyFeeProcessor>();
            services.AddTransient<IPaymentFileFormatProcessor       , PaymentFileFormatProcessor>();

            services.AddTransient<IPdfOutputSettingProcessor        , PdfOutputSettingProcessor>();

            services.AddTransient<IPeriodicBillingSettingProcessor  , PeriodicBillingSettingProcessor>();
            services.AddTransient<IPeriodicBillingProcesssor        , PeriodicBillingProcessor>();

            services.AddTransient<IReceiptExcludeProcessor          , ReceiptExcludeProcessor>();
            services.AddTransient<IReceiptHeaderProcessor           , ReceiptHeaderProcessor>();
            services.AddTransient<IReceiptMemoProcessor             , ReceiptMemoProcessor>();
            services.AddTransient<IReceiptProcessor                 , ReceiptProcessor>();
            services.AddTransient<IReceiptApportionProcessor        , ReceiptApportionProcessor>();
            services.AddTransient<IReceiptSearchProcessor           , ReceiptSearchProcessor>();
            services.AddTransient<IReceiptSectionTransferProcessor  , ReceiptSectionTransferProcessor>();
            services.AddTransient<IReceiptJournalizingProcessor     , ReceiptJournalizingProcessor>();
            services.AddTransient<IAdvanceReceivedProcessor         , AdvanceReceivedProcessor>();
            services.AddTransient<IAdvanceReceivedSplitProcessor    , AdvanceReceivedSplitProcessor>();


            services.AddTransient<IReminderProcessor                , ReminderProcessor>();
            services.AddTransient<IReminderHistoryProcessor         , ReminderHistoryProcessor>();
            services.AddTransient<IReminderCommonSettingProcessor   , ReminderCommonSettingProcessor>();
            services.AddTransient<IReminderTemplateSettingProcessor , ReminderTemplateSettingProcessor>();
            services.AddTransient<IReminderLevelSettingProcessor    , ReminderLevelSettingProcessor>();
            services.AddTransient<IReminderSummarySettingProcessor  , ReminderSummarySettingProcessor>();

            services.AddTransient<IReportSettingProcessor           , ReportSettingProcessor>();
            services.AddTransient<IArrearagesListProcessor          , ArrearagesListProcessor>();
            services.AddTransient<IScheduledPaymentListProcessor    , ScheduledPaymentListProcessor>();


            services.AddTransient<ISectionProcessor                 , SectionProcessor>();
            services.AddTransient<ISectionWithDepartmentProcessor   , SectionWithDepartmentProcessor>();
            services.AddTransient<ISectionWithLoginUserProcessor    , SectionWithLoginUserProcessor>();
            services.AddTransient<ISettingProcessor                 , SettingProcessor>();
            services.AddTransient<IStaffProcessor                   , StaffProcessor>();
            services.AddTransient<IStatusProcessor                  , StatusProcessor>();

            services.AddTransient<ISessionStorageProcessor          , SessionStorageProcessor>();

            services.AddTransient<ISynchronizationProcessor         , SynchronizationProcessor>();

            services.AddTransient<ITaskScheduleProcessor            , TaskScheduleProcessor>();
            services.AddTransient<ITaskScheduleHistoryProcessor     , TaskScheduleHistoryProcessor>();
            services.AddTransient<ITaxClassProcessor                , TaxClassProcessor>();

            services.AddTransient<IWebApiSettingProcessor           , WebApiSettingProcessor>();
            services.AddTransient<IHatarakuDBJournalizingProcessor  , HatarakuDBJournalizingProcessor>();

            services.AddTransient<IImportDataProcessor              , ImportDataProcessor>();
            services.AddTransient<IMFWebApiProcessor                , MFWebApiProcessor>();


            return services;
        }
    }
}
