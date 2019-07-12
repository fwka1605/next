using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api
{
    using Microsoft.Extensions.DependencyInjection;
    using Rac.VOne.Data.QueryProcessors;
    using Rac.VOne.Data.SqlServer.QueryProcessors;

    /// <summary>
    /// <see cref="Data.QueryProcessors"/>の DI 定義 を行う拡張メソッド
    /// </summary>
    public static class StartupConfigureQueryProcessors
    {
        /// <summary>
        /// <see cref="Data.QueryProcessors"/>の DI 定義 を行う拡張メソッド
        /// </summary>
        public static IServiceCollection ConfigureQueryProcessors(this IServiceCollection services)
        {
            ConfigureGenerics(services);
            ConfigureTrunk(services);
            ConfigureMaster(services);
            ConfigureTransaction(services);
            return services;
        }

        private static IServiceCollection ConfigureGenerics(IServiceCollection services)
        {
            services.AddTransient(typeof(IIdenticalEntityExistByIdQueryprocessor   <>), typeof(IdenticalEntityQueryProcessor<>));
            services.AddTransient(typeof(IIdenticalEntityGetByIdsQueryProcessor    <>), typeof(IdenticalEntityQueryProcessor<>));
            services.AddTransient(typeof(IDeleteIdenticalEntityQueryProcessor      <>), typeof(IdenticalEntityQueryProcessor<>));
            services.AddTransient(typeof(IMasterGetByCodeQueryProcessor            <>), typeof(MasterQueryProcessor<>));
            services.AddTransient(typeof(IMasterGetByCodesQueryProcessor           <>), typeof(MasterQueryProcessor<>));
            services.AddTransient(typeof(IMasterGetIdByCodeQueryProcessor          <>), typeof(MasterQueryProcessor<>));
            services.AddTransient(typeof(IMasterGetItemsQueryProcessor             <>), typeof(MasterQueryProcessor<>));
            services.AddTransient(typeof(ITransactionalGetByIdQueryProcessor       <>), typeof(TransactionQueryProcessor<>));
            services.AddTransient(typeof(IDeleteTransactionQueryProcessor          <>), typeof(TransactionQueryProcessor<>));
            services.AddTransient(typeof(ITransactionalGetByIdsQueryProcessor      <>), typeof(TransactionQueryProcessor<>));
            services.AddTransient(typeof(IByCompanyGetEntityQueryProcessor         <>), typeof(ByCompanyQueryProcessor<>));
            services.AddTransient(typeof(IByCompanyGetEntitiesQueryProcessor       <>), typeof(ByCompanyQueryProcessor<>));
            services.AddTransient(typeof(IByCompanyExistQueryProcessor             <>), typeof(ByCompanyQueryProcessor<>));
            services.AddTransient(typeof(IDeleteByCompanyQueryProcessor            <>), typeof(ByCompanyQueryProcessor<>));
            services.AddTransient(typeof(IDeleteTransactionDataQueryProcessor      <>), typeof(TransactionDataQueryProcessor<>));
            return services;
        }

        private static IServiceCollection ConfigureTrunk(IServiceCollection services)
        {
            // AdvanceReceivedBackup
            services.AddTransient<IAdvanceReceivedBackupQueryProcessor  , AdvanceReceivedBackupQueryProcessor>();

            services.AddTransient<IAuthenticationQueryProcessor         , AuthencationQueryProcessor>();
            services.AddTransient<IAuthorizationQueryProcessor          , AuthorizationQueryProcessor>();
            // DbFunction
            services.AddTransient<ICreateClientKeyQueryProcessor        , DbFunctionQueryProcessor>();
            services.AddTransient<IDbSystemDateTimeQueryProcessor       , DbFunctionQueryProcessor>();

            // trunk Logs
            services.AddTransient<ILogsQueryProcessor                   , LogsQueryProcessor>();

            return services;
        }

        private static IServiceCollection ConfigureMaster(IServiceCollection services)
        {

            // AccountTitle
            services.AddTransient<IAccountTitleQueryProcessor               , AccountTitleQueryProcessor>();
            services.AddTransient<IAccountTitleForImportQueryProcessor      , AccountTitleQueryProcessor>();
            services.AddTransient<IAddAccountTitleQueryProcessor            , AccountTitleQueryProcessor>();

            // ApplicationControl
            services.AddTransient<IAddApplicationControlQueryProcessor      , ApplicationControlQueryProcessor>();
            services.AddTransient<IUpdateApplicationControlQueryProcessor   , ApplicationControlQueryProcessor>();

            // BankAccount
            services.AddTransient<IBankAccountQueryProcessor                , BankAccountQueryProcessor>();
            services.AddTransient<IAddBankAccountQueryProcessor             , BankAccountQueryProcessor>();

            // BankAccountType
            services.AddTransient<IBankAccountTypeQueryProcessor            , BankAccountTypeQueyProcessor>();

            // BankBranch
            services.AddTransient<IBankBranchQueryProcessor                 , BankBranchQueryProcessor>();
            services.AddTransient<IAddBankBranchQueryProcessor              , BankBranchQueryProcessor>();
            services.AddTransient<IDeleteBankBranchQueryProcessor           , BankBranchQueryProcessor>();

            // Category
            services.AddTransient<ICategoryByCodeQueryProcessor             , CategoryQueryProcessor>();
            services.AddTransient<IAddCategoryQueryProcessor                , CategoryQueryProcessor>();
            services.AddTransient<ICategoriesQueryProcessor                 , CategoryQueryProcessor>();

            // Closing
            services.AddTransient<IClosingQueryProcessor                    , ClosingQueryProcessor>();
            services.AddTransient<IClosingSettingQueryProcessor             , ClosingSettingQueryProcessor>();

            // Company
            services.AddTransient<ICompanyQueryProcessor                    , CompanyQueryProcessor>();
            services.AddTransient<IAddCompanyQueryProcessor                 , CompanyQueryProcessor>();
            services.AddTransient<IDeleteCompanyQueryProcessor              , CompanyQueryProcessor>();
            services.AddTransient<IAddCompanyLogoQueryProcessor             , CompanyLogoQueryProcessor>();
            services.AddTransient<IDeleteCompanyLogoQueryProcessor          , CompanyLogoQueryProcessor>();

            // ColumnName
            services.AddTransient<IColumnNameSettingQueryProcessor          , ColumnNameSettingQueryProcessor>();
            services.AddTransient<IAddColumnNameSettingQueryProcessor       , ColumnNameSettingQueryProcessor>();

            // CollationOrder
            services.AddTransient<ICollationOrderQueryProcessor             , CollationOrderQueryProcessor>();

            // CollationSetting
            services.AddTransient<IAddCollationSettingQueryProcessor        , CollationSettingQueryProcessor>();
            services.AddTransient<ICollationSettingByCompanyIdQueryProcessor, CollationSettingQueryProcessor>();

            // ControlColor
            services.AddTransient<IControlColorQueryProcessor               , ControlColorQueryProcessor>();
            services.AddTransient<IAddControlColorQueryProcessor            , ControlColorQueryProcessor>();

            // Currency
            services.AddTransient<ICurrencyQueryProcessor                   , CurrencyQueryProcessor>();
            services.AddTransient<IAddCurrencyQueryProcessor                , CurrencyQueryProcessor>();

            // Customer
            services.AddTransient<IAddCustomerQueryProcessor                , CustomerQueryProcessor>();
            services.AddTransient<IUpdateCustomerQueryProcessor             , CustomerQueryProcessor>();
            services.AddTransient<ICustomerQueryProcessor                   , CustomerQueryProcessor>();

            services.AddTransient<ICustomerMinQueryProcessor                , CustomerQueryProcessor>();
            services.AddTransient<ICustomerExistsQueryProcessor             , CustomerQueryProcessor>();
            services.AddTransient<ICustomerImportQueryProcessor             , CustomerQueryProcessor>();

            // Customer Discount
            services.AddTransient<ICustomerDiscountQueryProcessor           , CustomerDiscountQueryProcessor>();
            services.AddTransient<IAddCustomerDiscountQueryProcessor        , CustomerDiscountQueryProcessor>();
            services.AddTransient<IDeleteCustomerDiscountQueryProcessor     , CustomerDiscountQueryProcessor>();

            // CustomerGroup
            services.AddTransient<ICustomerGroupByIdQueryProcessor          , CustomerGroupQueryProcessor>();
            services.AddTransient<IAddCustomerGroupQueryProcessor           , CustomerGroupQueryProcessor>();
            services.AddTransient<IDeleteCustomerGroupQueryProcessor        , CustomerGroupQueryProcessor>();

            // CustomerPaymentContract
            services.AddTransient<ICustomerPaymentContractQueryProcessor        , CustomerPaymentContractQueryProcessor>();
            services.AddTransient<IAddCustomerPaymentContractQueryProcessor     , CustomerPaymentContractQueryProcessor>();
            services.AddTransient<IDeleteCustomerPaymentContractQueryProcessor  , CustomerPaymentContractQueryProcessor>();

            // CustomerFee
            services.AddTransient<ICustomerFeeQueryProcessor                , CustomerFeeQueryProcessor>();
            services.AddTransient<IAddCustomerFeeQueryProcessor             , CustomerFeeQueryProcessor>();
            services.AddTransient<IDeleteCustomerFeeQueryProcessor          , CustomerFeeQueryProcessor>();

            // Department
            services.AddTransient<IAddDepartmentQueryProcessor              , DepartmentQueryProcessor>();
            services.AddTransient<IDepartmentByCodeQueryProcessor           , DepartmentQueryProcessor>();

            // Destination
            services.AddTransient<IAddDestinationQueryProcessor             , DestinationQueryProcessor>();
            services.AddTransient<IDestinationQueryProcessor                , DestinationQueryProcessor>();

            // EBExcludeAccountSetting
            services.AddTransient<IEBExcludeAccountSettingQueryProcessor    , EBExcludeAccountSettingQueryProcessor>();

            // EBFomat
            services.AddTransient<IEBFormatQueryProcessor                   , EBFormatQueryProcessor>();

            // EBFileStting
            services.AddTransient<IEBFileSettingQueryProcessor, EBFileSettingQueryProcessor>();
            services.AddTransient<IAddEBFileSettingQueryProcessor, EBFileSettingQueryProcessor>();
            services.AddTransient<IUpdateEBFileSettingQueryProcessor, EBFileSettingQueryProcessor>();

            // ExportField 出力項目設定
            services.AddTransient<IExportFieldSettingQueryProcessor, ExportFieldSettingQueryProcessor>();
            services.AddTransient<IAddExportFieldSettingQueryProcessor, ExportFieldSettingQueryProcessor>();

            // FunctionAuthority
            services.AddTransient<IAddFunctionAuthorityQueryProcessor, FunctionAuthorityQueryProcessor>();
            services.AddTransient<IFunctionAuthorityByLoginUserIdQueryProcessor, FunctionAuthorityQueryProcessor>();

            // GeneralSetting
            services.AddTransient<IUpdateGeneralSettingQueryProcessor, GeneralSettingQueryProcessor>();
            services.AddTransient<IGeneralSettingQueryProcessor, GeneralSettingQueryProcessor>();

            // GridSetting
            services.AddTransient<IGridSettingQueryProcessor, GridSettingQueryProcessor>();
            services.AddTransient<IAddGridSettingQueryProcessor, GridSettingQueryProcessor>();

            // HolidayCalendar
            services.AddTransient<IHolidayCalendarQueryProcessor, HolidayCalendarQueryProcessor>();
            services.AddTransient<IAddHolidayCalendarQueryProcessor, HolidayCalendarQueryProcessor>();
            services.AddTransient<IDeleteHolidayCalendarQueryProcessor, HolidayCalendarQueryProcessor>();

            // IgnoreKana
            services.AddTransient<IIgnoreKanasByCompanyIdQueryProcessor, IgnoreKanaQueryProcessor>();
            services.AddTransient<IIgnoreKanaByCodeQueryProcessor, IgnoreKanaQueryProcessor>();
            services.AddTransient<IAddIgnoreKanaQueryProcessor, IgnoreKanaQueryProcessor>();
            services.AddTransient<IDeleteIgnoreKanaQueryProcessor, IgnoreKanaQueryProcessor>();

            // ImporterSetting
            services.AddTransient<IImporterSettingQueryProcessor, ImporterSettingQueryProcessor>();
            services.AddTransient<IAddImporterSettingQueryProcessor, ImporterSettingQueryProcessor>();

            // ImporterSettingDetail
            services.AddTransient<IImporterSettingDetailQueryProcessor, ImporterSettingDetailQueryProcessor>();
            services.AddTransient<IAddImporterSettingDetailQueryProcessor, ImporterSettingDetailQueryProcessor>();

            // InputControl
            services.AddTransient<IInputControlQueryProcessor, InputControlQueryProcessor>();
            services.AddTransient<IAddInputControlQueryProcessor, InputControlQueryProcessor>();
            services.AddTransient<IDeleteInputControlQueryProcessor, InputControlQueryProcessor>();

            //InvoiceCommonSetting
            services.AddTransient<IAddInvoiceCommonSettingQueryProcessor, InvoiceCommonSettingQueryProcessor>();

            //InvoiceNumberHistory
            services.AddTransient<IAddInvoiceNumberHistoryQueryProcessor, InvoiceNumberHistoryQueryProcessor>();

            //InvoiceNumberSetting
            services.AddTransient<IAddInvoiceNumberSettingQueryProcessor, InvoiceNumberSettingQueryProcessor>();

            //InvoiceTemplateSetting
            services.AddTransient<IInvoiceTemplateSettingQueryProcessor, InvoiceTemplateSettingQueryProcessor>();
            services.AddTransient<IAddInvoiceTemplateSettingQueryProcessor, InvoiceTemplateSettingQueryProcessor>();

            // JuridicalPersonality
            services.AddTransient<IAddJuridicalPersonalityQueryProcessor, JuridicalPersonalityQueryProcessor>();
            services.AddTransient<IDeleteJuridicalPersonalityQueryProcessor, JuridicalPersonalityQueryProcessor>();
            services.AddTransient<IJuridicalPersonalityQueryProcessor, JuridicalPersonalityQueryProcessor>();

            // MasterImportSetting
            services.AddTransient<IUpdateImportSettingQueryProcessor, ImportSettingQueryProcessor>();
            services.AddTransient<IImportSettingQueryProcessor, ImportSettingQueryProcessor>();
            services.AddTransient<IInitializeImportSettingQueryProcessor, ImportSettingQueryProcessor>();

            // MenuAuthority
            services.AddTransient<IAddMenuAuthorityQueryProcessor, MenuAuthorityQueryProcessor>();
            services.AddTransient<IMenuAuthorityQueryProcessor, MenuAuthorityQueryProcessor>();

            // MfAggrTag
            services.AddTransient<IAddMfAggrTagQueryProcessor  , MfAggrTagQueryProcessor>();
            services.AddTransient<IMfAggrTagQueryProcessor     , MfAggrTagQueryProcessor>();

            // MfAggrTagRel
            services.AddTransient<IAddMfAggrTagRelQueryProcessor   , MfAggrTagRelQueryProcessor>();
            services.AddTransient<IDeleteMfAggrTagRelQueryProcessor, MfAggrTagRelQueryProcessor>();
            services.AddTransient<IMfAggrTagRelQueryProcessor      , MfAggrTagRelQueryProcessor>();

            // MfAggrAccount
            services.AddTransient<IAddMfAggrAccountQueryProcessor  , MfAggrAccountQueryProcessor>();
            services.AddTransient<IMfAggrAccountQueryProcessor     , MfAggrAccountQueryProcessor>();

            // MfAggrSubAccount
            services.AddTransient<IAddMfAggrSubAccountQueryProcessor, MfAggrSubAccountQueryProcessor>();
            services.AddTransient<IMfAggrSubAccountQueryProcessor   , MfAggrSubAccountQueryProcessor>();


            // KanaHistoryCustomer
            services.AddTransient<IKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();
            services.AddTransient<IAddKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();
            services.AddTransient<IUpdateKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();
            services.AddTransient<IDeleteKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();

            // LoginUser
            services.AddTransient<ILoginUserLicenseQueryProcessor, LoginUserLicenseQueryProcessor>();
            services.AddTransient<ILoginUserQueryProcessor, LoginUserQueryProcessor>();
            services.AddTransient<IAddLoginUserQueryProcessor, LoginUserQueryProcessor>();
            services.AddTransient<IDeleteLoginUserPasswordQueryProcessor, LoginUserPasswordQueryProcessor>();

            // LoginUserPassword
            services.AddTransient<ILoginUserPasswordQueryProcessor, LoginUserPasswordQueryProcessor>();
            services.AddTransient<IAddLoginUserPasswordQueryProcessor, LoginUserPasswordQueryProcessor>();

            // KanaHistoryPaymentAgency
            services.AddTransient<IKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();
            services.AddTransient<IAddKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();
            services.AddTransient<IUpdateKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();
            services.AddTransient<IDeleteKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();

            // PasswordPolicy
            services.AddTransient<IAddPasswordPolicyQueryProcessor, PasswordPolicyQueryProcessor>();

            // PaymentAgency
            services.AddTransient<IAddPaymentAgencyQueryProcessor, PaymentAgencyQueryProcessor>();

            // PaymentAgencyFee
            services.AddTransient<IAddPaymentAgencyFeeQueryProcessor, PaymentAgencyFeeQueryProcessor>();
            services.AddTransient<IPaymentAgencyFeeQueryProcessor, PaymentAgencyFeeQueryProcessor>();
            services.AddTransient<IDeletePaymentAgencyFeeQueryProcessor, PaymentAgencyFeeQueryProcessor>();

            // PdfOutputSetting
            services.AddTransient<IPdfOutputSettingQueryProcessor, PdfOutputSettingQueryProcessor>();
            services.AddTransient<IAddPdfOutputSettingQueryProcessor, PdfOutputSettingQueryProcessor>();

            // PeriodicBillingSetting
            services.AddTransient<IPeriodicBillingSettingQueryProcessor, PeriodicBillingSettingQueryProcessor>();
            services.AddTransient<IAddPeriodicBillingSettingQueryProcessor, PeriodicBillingSettingQueryProcessor>();

            // PeriodicBillingSettingDetail
            services.AddTransient<IPeriodicBillingSettingDetailQueryProcessor, PeriodicBillingSettingDetailQueryProcessor>();
            services.AddTransient<IAddPeriodicBillingSettingDetailQueryProcessor, PeriodicBillingSettingDetailQueryProcessor>();
            services.AddTransient<IDeletePeriodicBillingSettingDetailQueryProcessor, PeriodicBillingSettingDetailQueryProcessor>();


            //Reminder
            services.AddTransient<IReminderQueryProcessor, ReminderQueryProcessor>();
            services.AddTransient<IAddReminderQueryProcessor, ReminderQueryProcessor>();
            services.AddTransient<IUpdateReminderQueryProcessor, ReminderQueryProcessor>();
            services.AddTransient<IUpdateBillingReminderQueryProcessor, ReminderQueryProcessor>();
            services.AddTransient<IDeleteReminderSummaryQueryProcessor, ReminderQueryProcessor>();

            //ReminderHistory
            services.AddTransient<IReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();
            services.AddTransient<IAddReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();
            services.AddTransient<IUpdateReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();
            services.AddTransient<IDeleteReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();

            //ReminderOutputed
            services.AddTransient<IReminderOutputedQueryProcessor, ReminderOutputedQueryProcessor>();
            services.AddTransient<IReminderOutputedExistsQueryProcessor, ReminderOutputedQueryProcessor>();

            //ReminderCommonSetting
            services.AddTransient<IAddReminderCommonSettingQueryProcessor, ReminderCommonSettingQueryProcessor>();

            //ReminderTemplateSetting
            services.AddTransient<IAddReminderTemplateSettingQueryProcessor, ReminderTemplateSettingQueryProcessor>();

            //ReminderLevelSetting
            services.AddTransient<IReminderLevelSettingQueryProcessor, ReminderLevelSettingQueryProcessor>();
            services.AddTransient<IAddReminderLevelSettingQueryProcessor, ReminderLevelSettingQueryProcessor>();
            services.AddTransient<IDeleteReminderLevelSettingQueryProcessor, ReminderLevelSettingQueryProcessor>();

            //ReminderSummarySetting
            services.AddTransient<IAddReminderSummarySettingQueryProcessor, ReminderSummarySettingQueryProcessor>();
            services.AddTransient<IReminderSummarySettingQueryProcessor, ReminderSummarySettingQueryProcessor>();

            // ReportSetting
            services.AddTransient<IReportSettingQueryProcessor, ReportSettingQueryProcessor>();
            services.AddTransient<IAddReportSettingQueryProcessor, ReportSettingQueryProcessor>();
            services.AddTransient<IDeleteReportSettingQueryProcessor, ReportSettingQueryProcessor>();


            // Staff
            services.AddTransient<IStaffQueryProcessor, StaffQueryProcessor>();
            services.AddTransient<IAddStaffQueryProcessor, StaffQueryProcessor>();

            // Section
            services.AddTransient<ISectionQueryProcessor, SectionQueryProcessor>();
            services.AddTransient<IAddSectionQueryProcessor, SectionQueryProcessor>();

            // SectionWithDepartment
            services.AddTransient<ISectionWithDepartmentQueryProcessor, SectionWithDepartmentQueryProcessor>();
            services.AddTransient<IAddSectionWithDepartmentQueryProcessor, SectionWithDepartmentQueryProcessor>();
            services.AddTransient<IDeleteSectionWithDepartmentQueryProcessor, SectionWithDepartmentQueryProcessor>();

            // SectionWithLoginUser
            services.AddTransient<ISectionWithLoginUserQueryProcessor, SectionWithLoginUserQueryProcessor>();
            services.AddTransient<IAddSectionWithLoginUserQueryProcessor, SectionWithLoginUserQueryProcessor>();
            services.AddTransient<IDeleteSectionWithLoginUserQueryProcessor, SectionWithLoginUserQueryProcessor>();

            // Setting
            services.AddTransient<ISettingQueryProcessor, SettingQueryProcessor>();

            //Status
            services.AddTransient<IAddStatusQueryProcessor, StatusQueryProcessor>();
            services.AddTransient<IDeleteStatusQueryProcessor, StatusQueryProcessor>();
            services.AddTransient<IStatusQueryProcessor, StatusQueryProcessor>();
            services.AddTransient<IStatusExistsQueryProcessor, StatusQueryProcessor>();

            // TaskSchedule
            services.AddTransient<ITaskScheduleQueryProcessor, TaskScheduleQueryProcessor>();

            // TaxClass
            services.AddTransient<ITaxClassQueryProcessor, TaxClassQueryProcessor>();

            // PaymentFileFormat
            services.AddTransient<IPaymentFileFormatQueryProcessor, PaymentFileFormatQueryProcessor>();

            // WebApiSetting
            services.AddTransient<IWebApiSettingQueryProcessor, WebApiSettingQueryProcessor>();
            services.AddTransient<IAddWebApiSettingQueryProcessor, WebApiSettingQueryProcessor>();
            services.AddTransient<IDeleteWebApiSettingQueryProcessor, WebApiSettingQueryProcessor>();

            return services;
        }

        /// <summary>トランザクション系 query processor の登録</summary>
        private static IServiceCollection ConfigureTransaction(IServiceCollection services)
        {
            // AccountTransfer Log / Detail
            services.AddTransient<IAccountTransferLogQueryProcessor, AccountTransferLogQueryProcessor>();
            services.AddTransient<IAddAccountTransferLogQueryProcessor, AccountTransferLogQueryProcessor>();
            services.AddTransient<IAccountTransferDetailQueryProcessor, AccountTransferDetailQueryProcessor>();
            services.AddTransient<IAddAccountTransferDetailQueryProcessor, AccountTransferDetailQueryProcessor>();
            services.AddTransient<IUpdateBillingAccountTransferLogQueryProcessor, AccountTransferDetailQueryProcessor>();
            services.AddTransient<IDeleteAccountTransferDetailQueryProcessor, AccountTransferDetailQueryProcessor>();

            // Billing
            services.AddTransient<IBillingQueryProcessor, BillingQueryProcessor>();
            services.AddTransient<IAddBillingQueryProcessor, BillingQueryProcessor>();
            services.AddTransient<IUpdateBillingQueryProcessor, BillingQueryProcessor>();
            services.AddTransient<IBillingExistsQueryProcessor, BillingExistsQueryProcessor>();
            services.AddTransient<IBillingJournalizingQueryProcessor, BillingJournalizingQueryProcessor>();

            services.AddTransient<IBillingAgingListQueryProcessor, BillingAgingListQueryProcessor>();

            // BillingDueAtModify
            services.AddTransient<IBillingDueAtModifySearchQueryProcessor, BillingDueAtModifySearchQueryProcessor>();
            services.AddTransient<IUpdateBillingDueAtModifyQueryProcessor, BillingDueAtModifyQueryProcessor>();

            // BillingBalance
            services.AddTransient<IBillingBalanceQueryProcessor, BillingBalanceQueryProcessor>();
            services.AddTransient<IAddBillingBalanceQueryProcessor, BillingBalanceQueryProcessor>();
            services.AddTransient<IDeleteBillingBalanceQueryProcessor, BillingBalanceQueryProcessor>();

            // BillingBalanceBacck
            services.AddTransient<IAddBillingBalanceBackQueryProcessor, BillingBalanceBackQueryProcessor>();

            // BillingDiscount
            services.AddTransient<IAddBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();
            services.AddTransient<IUpdateBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();
            services.AddTransient<IBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();
            services.AddTransient<IDeleteBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();

            // BillingDivisionContract
            services.AddTransient<IBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();
            services.AddTransient<IAddBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();
            services.AddTransient<IUpdateBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();
            services.AddTransient<IDeleteBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();


            // BillingScheduledIncome
            services.AddTransient<IAddBillingScheduledIncomeQueryProcessor, BillingScheduledIncomeQueryProcessor>();

            // BillingSearch
            services.AddTransient<IBillingSearchQueryProcessor, BillingSearchQueryProcessor>();
            services.AddTransient<IBillingSearchForImportQueryProcessor, BillingSearchQueryProcessor>();

            // BillingMemo
            services.AddTransient<IBillingMemoQueryProcessor, BillingMemoQueryProcessor>();
            services.AddTransient<IUpdateBillingMemoQueryProcessor, BillingMemoQueryProcessor>();
            services.AddTransient<IDeleteBillingMemoQueryProcessor, BillingMemoQueryProcessor>();

            // BillingInput
            services.AddTransient<IAddBillingInputQueryProcessor, BillingInputQueryProcessor>();

            // BillingInvoice
            services.AddTransient<IBillingInvoiceQueryProcessor, BillingInvoiceQueryProcessor>();

            // CustomerLedger
            services.AddTransient<ICustomerLedgerQueryProcessor, CustomerLedgerQueryProcessor>();

            // DataMaintenance
            services.AddTransient<IDataMaintenanceQueryProcessor, DataMaintenanceQueryProcessor>();

            // LogData
            services.AddTransient<ILogDataByCompanyIdQueryProcessor, LogDataQueryProcessor>();
            services.AddTransient<IAddLogDataQueryProcessor, LogDataQueryProcessor>();

            // Collation
            services.AddTransient<ICollationQueryProcessor, CollationQueryProcessor>();

            // Matching
            services.AddTransient<IMatchingQueryProcessor, MatchingQueryProcessor>();
            services.AddTransient<IAddMatchingQueryProcessor, MatchingQueryProcessor>();
            services.AddTransient<ICancelMatchingQueryProcessor, MatchingQueryProcessor>();

            // MatchingHeader
            services.AddTransient<IUpdateMatchingHeaderQueryProcessor, MatchingHeaderQueryProcessor>();

            // MatchingOrder
            services.AddTransient<IMatchingOrderQueryProcessor, MatchingOrderQueryProcessor>();

            // MatchingBillingDiscount
            services.AddTransient<IAddMatchingBillingDiscountQueryProcessor, MatchingBillingDiscountQueryProcessor>();
            services.AddTransient<IDeleteMatchingBillingDiscountQueryProcessor, MatchingBillingDiscountQueryProcessor>();

            // MatchingHistory 消込履歴データ検索
            services.AddTransient<IMatchingHistorySearchQueryProcessor, MatchingHistorySearchQueryProcessor>();
            services.AddTransient<IAddMatchingOutputedQueryProcessor, MatchingOutputedQueryProcessor>();

            // MatchingJournalizing 消込仕訳
            services.AddTransient<IMatchingJournalizingQueryProcessor, MatchingJournalizingQueryProcessor>();
            services.AddTransient<IMatchingJournalizingSummaryQueryProcessor, MatchingJournalizingQueryProcessor>();
            services.AddTransient<IMatchingGeneralJournalizingQueryProcessor, MatchingJournalizingQueryProcessor>();
            services.AddTransient<IUpdateMatchingJournalizingQueryProcessor, MatchingJournalizingQueryProcessor>();
            services.AddTransient<IMatchedReceiptQueryProcessor, MatchingJournalizingQueryProcessor>();
            services.AddTransient<IUpdateReceiptMatchingJournalizingQueryProcessor, ReceiptMatchingJournalizingQueryProcessor>();
            services.AddTransient<IUpdateReceiptExcludeJournalizingQueryProcessor, ReceiptExcludeQueryProcessor>();
            services.AddTransient<IUpdateAdvanceReceivedBackupJournalizingQueryProcessor, AdvanceReceivedBackupQueryProcessor>();
            services.AddTransient<IMatchingJournalizingDetailQueryProcessor, MatchingJournalizingQueryProcessor>();

            //MFBilling
            services.AddTransient<IMFBillingQueryProcessor, MFBillingQueryProcessor>();
            services.AddTransient<IAddMFBillingQueryProcessor, MFBillingQueryProcessor>();

            // MfAggrTransaction
            services.AddTransient<IMfAggrTransactionQueryProcessor     , MfAggrTransactionQueryProcessor>();
            services.AddTransient<IAddMfAggrTransactionQueryProcessor  , MfAggrTransactionQueryProcessor>();

            // Netting
            services.AddTransient<INettingQueryProcessor, NettingQueryProcessor>();
            services.AddTransient<IAddNettingQueryProcessor, NettingQueryProcessor>();
            services.AddTransient<IUpdateNettingQueryProcessor, NettingQueryProcessor>();

            // NettingSearch
            services.AddTransient<INettingSearchQueryProcessor, NettingSearchQueryProcessor>();

            // PreiodicBillingHistory
            services.AddTransient<IAddPeriodicBillingCreatedQueryProcessor, PeriodicBillingCreatedQueryProcessor>();

            // Receipt
            services.AddTransient<IReceiptQueryProcessor, ReceiptQueryProcessor>();
            services.AddTransient<IReceiptExistsQueryProcessor, ReceiptQueryProcessor>();
            services.AddTransient<IReceiptSearchQueryProcessor, ReceiptSearchQueryProcessor>();
            services.AddTransient<IDeleteReceiptQueryProcessor, ReceiptQueryProcessor>();
            services.AddTransient<IUpdateReceiptQueryProcessor, ReceiptQueryProcessor>();
            services.AddTransient<IAddReceiptQueryProcessor, ReceiptQueryProcessor>();
            services.AddTransient<IReceiptJournalizingQueryProcessor, ReceiptJournalizingQueryProcessor>();
            services.AddTransient<IReceiptGeneralJournalizingQueryProcessor, ReceiptJournalizingQueryProcessor>();
            services.AddTransient<IUpdateReceiptJournalizingQueryProcessor, ReceiptJournalizingQueryProcessor>();

            // ReceiptExclude
            services.AddTransient<IReceiptExcludeQueryProcessor, ReceiptExcludeQueryProcessor>();
            services.AddTransient<IAddReceiptExcludeQueryProcessor, ReceiptExcludeQueryProcessor>();
            services.AddTransient<IDeleteReceiptExcludeQueryProcessor, ReceiptExcludeQueryProcessor>();

            // ReceiptMemo
            services.AddTransient<IReceiptMemoQueryProcessor, ReceiptMemoQueryProcessor>();
            services.AddTransient<IAddReceiptMemoQueryProcessor, ReceiptMemoQueryProcessor>();
            services.AddTransient<IDeleteReceiptMemoQueryProcessor, ReceiptMemoQueryProcessor>();


            // Receipt Apportion 入金データ振分
            services.AddTransient<IUpdateReceiptApportionQueryProcessor, ReceiptQueryProcessor>();

            // ReceiptHeader
            services.AddTransient<IUpdateReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();
            services.AddTransient<IReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();
            services.AddTransient<IReceiptApportionByIdQueryProcessor, ReceiptQueryProcessor>();
            services.AddTransient<IAddReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();
            services.AddTransient<IDeleteReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();

            // ReceiptSectionTransfer 入金部門振替処理
            services.AddTransient<IReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();
            services.AddTransient<IAddReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();
            services.AddTransient<IDeleteReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();
            services.AddTransient<IUpdateReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();

            // Report
            services.AddTransient<IArrearagesListQueryProcessor, ArrearagesListQueryProcessor>();
            services.AddTransient<ICollectionScheduleQueryProcessor, CollectionScheduleQueryProcessor>();
            services.AddTransient<ICreditAgingListQueryProcessor, CreditAgingListQueryProcessor>();
            services.AddTransient<IScheduledPaymentListQueryProcessor, ScheduledPaymentListQueryProcessor>();

            // Syncronization
            services.AddTransient<ISynchronizationQueryProcessor, SynchronizationQueryProcessor>();

            // SessionStorage
            services.AddTransient<ISessionStorageQueryProcessor, SessionStorageQueryProcessor>();

            // TaskScheduleHistory
            services.AddTransient<ITaskScheduleHistoryQueryProcessor, TaskScheduleHistoryQueryProcessor>();
            services.AddTransient<IAddTaskScheduleHistoryQueryProcessor, TaskScheduleHistoryQueryProcessor>();

            // ImportFileLog
            services.AddTransient<IImportFileLogQueryProcessor, ImportFileLogQueryProcessor>();
            services.AddTransient<IAddImportFileLogQueryProcessor, ImportFileLogQueryProcessor>();

            // Work***Target
            services.AddTransient<IWorkDepartmentTargetQueryProcessor, WorkDepartmentTargetQueryProcessor>();
            services.AddTransient<IWorkSectionTargetQueryProcessor, WorkSectionTargetQueryProcessor>();

            services.AddTransient<IHatarakuDBJournalizingQueryProcessor, HatarakuDBJournalizingQueryProcessor>();

            // ImportData/ImportDataDetail
            services.AddTransient<IAddImportDataQueryProcessor, ImportDataQueryProcessor>();
            services.AddTransient<IAddImportDataDetailQueryProcessor, ImportDataDetailQueryProcessor>();
            services.AddTransient<IImportDataDetailQueryProcessor, ImportDataDetailQueryProcessor>();

            return services;
        }
    }
}
