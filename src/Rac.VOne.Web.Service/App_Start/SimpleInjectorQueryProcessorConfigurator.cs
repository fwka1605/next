using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Data.SqlServer;
using Rac.VOne.Data.SqlServer.QueryProcessors;

namespace Rac.VOne.Web.Service
{
    public class SimpleInjectorQueryProcessorConfigurator
    {
        private readonly Container container;

        public SimpleInjectorQueryProcessorConfigurator(Container container)
        {
            this.container = container;
        }
        public void Configure()
        {
            ConfigureGenericsQueryProcessors();
            ConfigureQueryProcessors();
            ConfigureMastersQueryProcessors();
            ConfigureTransactionsQueryProcessors();
        }

        private void ConfigureGenericsQueryProcessors()
        {
            container.Register(typeof(IIdenticalEntityExistByIdQueryprocessor<>), typeof(IdenticalEntityQueryProcessor<>));
            container.Register(typeof(IIdenticalEntityGetByIdsQueryProcessor<>), typeof(IdenticalEntityQueryProcessor<>));
            container.Register(typeof(IDeleteIdenticalEntityQueryProcessor<>), typeof(IdenticalEntityQueryProcessor<>));

            container.Register(typeof(IMasterGetByCodeQueryProcessor<>), typeof(MasterQueryProcessor<>));
            container.Register(typeof(IMasterGetByCodesQueryProcessor<>), typeof(MasterQueryProcessor<>));
            container.Register(typeof(IMasterGetIdByCodeQueryProcessor<>), typeof(MasterQueryProcessor<>));
            container.Register(typeof(IMasterGetItemsQueryProcessor<>), typeof(MasterQueryProcessor<>));

            container.Register(typeof(ITransactionalGetByIdQueryProcessor<>), typeof(TransactionQueryProcessor<>));
            container.Register(typeof(IDeleteTransactionQueryProcessor<>), typeof(TransactionQueryProcessor<>));
            container.Register(typeof(ITransactionalGetByIdsQueryProcessor<>), typeof(TransactionQueryProcessor<>));
            container.Register(typeof(ISynchronizationQueryProcessor), typeof(SynchronizationQueryProcessor));

            container.Register(typeof(IByCompanyGetEntityQueryProcessor<>), typeof(ByCompanyQueryProcessor<>));
            container.Register(typeof(IByCompanyGetEntitiesQueryProcessor<>), typeof(ByCompanyQueryProcessor<>));
            container.Register(typeof(IByCompanyExistQueryProcessor<>), typeof(ByCompanyQueryProcessor<>));
            container.Register(typeof(IDeleteByCompanyQueryProcessor<>), typeof(ByCompanyQueryProcessor<>));

            container.Register(typeof(IDeleteTransactionDataQueryProcessor<>), typeof(TransactionDataQueryProcessor<>));
        }

        private void ConfigureQueryProcessors()
        {
            // AdvanceReceivedBackup
            container.Register<IAdvanceReceivedBackupQueryProcessor, AdvanceReceivedBackupQueryProcessor>();

            container.Register<IAuthenticationQueryProcessor, AuthencationQueryProcessor>();
            container.Register<IAuthorizationQueryProcessor, AuthorizationQueryProcessor>();
            // DbFunction
            container.Register<ICreateClientKeyQueryProcessor, DbFunctionQueryProcessor>();
            container.Register<IDbSystemDateTimeQueryProcessor, DbFunctionQueryProcessor>();

            // trunk Logs
            container.Register<ILogsQueryProcessor, LogsQueryProcessor>();
        }

        private void ConfigureMastersQueryProcessors()
        {
            // AccountTitle
            container.Register<IAccountTitleQueryProcessor, AccountTitleQueryProcessor>();
            container.Register<IAccountTitleForImportQueryProcessor, AccountTitleQueryProcessor>();
            container.Register<IAddAccountTitleQueryProcessor, AccountTitleQueryProcessor>();

            // ApplicationControl
            container.Register<IAddApplicationControlQueryProcessor, ApplicationControlQueryProcessor>();
            container.Register<IUpdateApplicationControlQueryProcessor, ApplicationControlQueryProcessor>();

            // BankAccount
            container.Register<IBankAccountQueryProcessor, BankAccountQueryProcessor>();
            container.Register<IAddBankAccountQueryProcessor, BankAccountQueryProcessor>();

            // BankAccountType
            container.Register<IBankAccountTypeQueryProcessor, BankAccountTypeQueyProcessor>();

            // BankBranch
            container.Register<IBankBranchQueryProcessor, BankBranchQueryProcessor>();
            container.Register<IAddBankBranchQueryProcessor, BankBranchQueryProcessor>();
            container.Register<IDeleteBankBranchQueryProcessor, BankBranchQueryProcessor>();

            // Category
            container.Register<ICategoryByCodeQueryProcessor, CategoryQueryProcessor>();
            container.Register<IAddCategoryQueryProcessor, CategoryQueryProcessor>();
            container.Register<ICategoriesQueryProcessor, CategoryQueryProcessor>();

            // Closing
            container.Register<IClosingQueryProcessor, ClosingQueryProcessor>();
            container.Register<IClosingSettingQueryProcessor, ClosingSettingQueryProcessor>();

            // Company
            container.Register<ICompanyQueryProcessor, CompanyQueryProcessor>();
            container.Register<IAddCompanyQueryProcessor, CompanyQueryProcessor>();
            container.Register<IDeleteCompanyQueryProcessor, CompanyQueryProcessor>();

            // CompanyLogo
            container.Register<IAddCompanyLogoQueryProcessor, CompanyLogoQueryProcessor>();
            container.Register<IDeleteCompanyLogoQueryProcessor, CompanyLogoQueryProcessor>();

            // ColumnName
            container.Register<IColumnNameSettingQueryProcessor, ColumnNameSettingQueryProcessor>();
            container.Register<IAddColumnNameSettingQueryProcessor, ColumnNameSettingQueryProcessor>();

            // CollationOrder
            container.Register<ICollationOrderQueryProcessor, CollationOrderQueryProcessor>();

            // CollationSetting
            container.Register<IAddCollationSettingQueryProcessor, CollationSettingQueryProcessor>();
            container.Register<ICollationSettingByCompanyIdQueryProcessor, CollationSettingQueryProcessor>();

            // ControlColor
            container.Register<IControlColorQueryProcessor, ControlColorQueryProcessor>();
            container.Register<IAddControlColorQueryProcessor, ControlColorQueryProcessor>();

            // Currency
            container.Register<ICurrencyQueryProcessor, CurrencyQueryProcessor>();
            container.Register<IAddCurrencyQueryProcessor, CurrencyQueryProcessor>();

            // Customer
            container.Register<IAddCustomerQueryProcessor, CustomerQueryProcessor>();
            container.Register<IUpdateCustomerQueryProcessor, CustomerQueryProcessor>();
            container.Register<ICustomerQueryProcessor, CustomerQueryProcessor>();

            container.Register<ICustomerMinQueryProcessor, CustomerQueryProcessor>();
            container.Register<ICustomerExistsQueryProcessor, CustomerQueryProcessor>();
            container.Register<ICustomerImportQueryProcessor, CustomerQueryProcessor>();

            // Customer Discount
            container.Register<ICustomerDiscountQueryProcessor, CustomerDiscountQueryProcessor>();
            container.Register<IAddCustomerDiscountQueryProcessor, CustomerDiscountQueryProcessor>();
            container.Register<IDeleteCustomerDiscountQueryProcessor, CustomerDiscountQueryProcessor>();

            // CustomerGroup
            container.Register<ICustomerGroupByIdQueryProcessor, CustomerGroupQueryProcessor>();
            container.Register<IAddCustomerGroupQueryProcessor, CustomerGroupQueryProcessor>();
            container.Register<IDeleteCustomerGroupQueryProcessor, CustomerGroupQueryProcessor>();

            // CustomerPaymentContract
            container.Register<ICustomerPaymentContractQueryProcessor, CustomerPaymentContractQueryProcessor>();
            container.Register<IAddCustomerPaymentContractQueryProcessor, CustomerPaymentContractQueryProcessor>();
            container.Register<IDeleteCustomerPaymentContractQueryProcessor, CustomerPaymentContractQueryProcessor>();

            // CustomerFee
            container.Register<ICustomerFeeQueryProcessor, CustomerFeeQueryProcessor>();
            container.Register<IAddCustomerFeeQueryProcessor, CustomerFeeQueryProcessor>();
            container.Register<IDeleteCustomerFeeQueryProcessor, CustomerFeeQueryProcessor>();

            // Department
            container.Register<IAddDepartmentQueryProcessor, DepartmentQueryProcessor>();
            container.Register<IDepartmentByCodeQueryProcessor, DepartmentQueryProcessor>();

            // Destination
            container.Register<IAddDestinationQueryProcessor, DestinationQueryProcessor>();
            container.Register<IDestinationQueryProcessor, DestinationQueryProcessor>();

            // EBExcludeAccountSetting
            container.Register<IEBExcludeAccountSettingQueryProcessor, EBExcludeAccountSettingQueryProcessor>();

            // EBFomat
            container.Register<IEBFormatQueryProcessor, EBFormatQueryProcessor>();

            // EBFileStting
            container.Register<IEBFileSettingQueryProcessor, EBFileSettingQueryProcessor>();
            container.Register<IAddEBFileSettingQueryProcessor, EBFileSettingQueryProcessor>();
            container.Register<IUpdateEBFileSettingQueryProcessor, EBFileSettingQueryProcessor>();

            // ExportField 出力項目設定
            container.Register<IExportFieldSettingQueryProcessor, ExportFieldSettingQueryProcessor>();
            container.Register<IAddExportFieldSettingQueryProcessor, ExportFieldSettingQueryProcessor>();

            // FunctionAuthority
            container.Register<IAddFunctionAuthorityQueryProcessor, FunctionAuthorityQueryProcessor>();
            container.Register<IFunctionAuthorityByLoginUserIdQueryProcessor, FunctionAuthorityQueryProcessor>();

            // GeneralSetting
            container.Register<IUpdateGeneralSettingQueryProcessor, GeneralSettingQueryProcessor>();
            container.Register<IGeneralSettingQueryProcessor, GeneralSettingQueryProcessor>();

            // GridSetting
            container.Register<IGridSettingQueryProcessor, GridSettingQueryProcessor>();
            container.Register<IAddGridSettingQueryProcessor, GridSettingQueryProcessor>();

            // HolidayCalendar
            container.Register<IHolidayCalendarQueryProcessor, HolidayCalendarQueryProcessor>();
            container.Register<IAddHolidayCalendarQueryProcessor, HolidayCalendarQueryProcessor>();
            container.Register<IDeleteHolidayCalendarQueryProcessor, HolidayCalendarQueryProcessor>();

            // IgnoreKana
            container.Register<IIgnoreKanasByCompanyIdQueryProcessor, IgnoreKanaQueryProcessor>();
            container.Register<IIgnoreKanaByCodeQueryProcessor, IgnoreKanaQueryProcessor>();
            container.Register<IAddIgnoreKanaQueryProcessor, IgnoreKanaQueryProcessor>();
            container.Register<IDeleteIgnoreKanaQueryProcessor, IgnoreKanaQueryProcessor>();

            // ImporterSetting
            container.Register<IImporterSettingQueryProcessor, ImporterSettingQueryProcessor>();
            container.Register<IAddImporterSettingQueryProcessor, ImporterSettingQueryProcessor>();

            // ImporterSettingDetail
            container.Register<IImporterSettingDetailQueryProcessor, ImporterSettingDetailQueryProcessor>();
            container.Register<IAddImporterSettingDetailQueryProcessor, ImporterSettingDetailQueryProcessor>();

            // InputControl
            container.Register<IInputControlQueryProcessor, InputControlQueryProcessor>();
            container.Register<IAddInputControlQueryProcessor, InputControlQueryProcessor>();
            container.Register<IDeleteInputControlQueryProcessor, InputControlQueryProcessor>();

            //InvoiceCommonSetting
            container.Register<IAddInvoiceCommonSettingQueryProcessor, InvoiceCommonSettingQueryProcessor>();

            //InvoiceNumberHistory
            container.Register<IAddInvoiceNumberHistoryQueryProcessor, InvoiceNumberHistoryQueryProcessor>();

            //InvoiceNumberSetting
            container.Register<IAddInvoiceNumberSettingQueryProcessor, InvoiceNumberSettingQueryProcessor>();

            //InvoiceTemplateSetting
            container.Register<IInvoiceTemplateSettingQueryProcessor, InvoiceTemplateSettingQueryProcessor>();
            container.Register<IAddInvoiceTemplateSettingQueryProcessor, InvoiceTemplateSettingQueryProcessor>();

            // JuridicalPersonality
            container.Register<IAddJuridicalPersonalityQueryProcessor, JuridicalPersonalityQueryProcessor>();
            container.Register<IDeleteJuridicalPersonalityQueryProcessor, JuridicalPersonalityQueryProcessor>();
            container.Register<IJuridicalPersonalityQueryProcessor, JuridicalPersonalityQueryProcessor>();

            // MasterImportSetting
            container.Register<IUpdateImportSettingQueryProcessor, ImportSettingQueryProcessor>();
            container.Register<IImportSettingQueryProcessor, ImportSettingQueryProcessor>();
            container.Register<IInitializeImportSettingQueryProcessor, ImportSettingQueryProcessor>();

            // MenuAuthority
            container.Register<IAddMenuAuthorityQueryProcessor, MenuAuthorityQueryProcessor>();
            container.Register<IMenuAuthorityQueryProcessor, MenuAuthorityQueryProcessor>();

            // KanaHistoryCustomer
            container.Register<IKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();
            container.Register<IAddKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();
            container.Register<IUpdateKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();
            container.Register<IDeleteKanaHistoryCustomerQueryProcessor, KanaHistoryCustomerQueryProcessor>();

            // LoginUser
            container.Register<ILoginUserLicenseQueryProcessor, LoginUserLicenseQueryProcessor>();
            container.Register<ILoginUserQueryProcessor, LoginUserQueryProcessor>();
            container.Register<IAddLoginUserQueryProcessor, LoginUserQueryProcessor>();
            container.Register<IDeleteLoginUserPasswordQueryProcessor, LoginUserPasswordQueryProcessor>();

            // LoginUserPassword
            container.Register<ILoginUserPasswordQueryProcessor, LoginUserPasswordQueryProcessor>();
            container.Register<IAddLoginUserPasswordQueryProcessor, LoginUserPasswordQueryProcessor>();

            // KanaHistoryPaymentAgency
            container.Register<IKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();
            container.Register<IAddKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();
            container.Register<IUpdateKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();
            container.Register<IDeleteKanaHistoryPaymentAgencyQueryProcessor, KanaHistoryPaymentAgencyQueryProcessor>();

            // PasswordPolicy
            container.Register<IAddPasswordPolicyQueryProcessor, PasswordPolicyQueryProcessor>();

            // PaymentAgency
            container.Register<IAddPaymentAgencyQueryProcessor, PaymentAgencyQueryProcessor>();

            // PaymentAgencyFee
            container.Register<IAddPaymentAgencyFeeQueryProcessor, PaymentAgencyFeeQueryProcessor>();
            container.Register<IPaymentAgencyFeeQueryProcessor, PaymentAgencyFeeQueryProcessor>();
            container.Register<IDeletePaymentAgencyFeeQueryProcessor, PaymentAgencyFeeQueryProcessor>();

            // PdfOutputSetting
            container.Register<IPdfOutputSettingQueryProcessor, PdfOutputSettingQueryProcessor>();
            container.Register<IAddPdfOutputSettingQueryProcessor, PdfOutputSettingQueryProcessor>();

            // PeriodicBillingSetting
            container.Register<IPeriodicBillingSettingQueryProcessor, PeriodicBillingSettingQueryProcessor>();
            container.Register<IAddPeriodicBillingSettingQueryProcessor, PeriodicBillingSettingQueryProcessor>();

            // PeriodicBillingSettingDetail
            container.Register<IPeriodicBillingSettingDetailQueryProcessor, PeriodicBillingSettingDetailQueryProcessor>();
            container.Register<IAddPeriodicBillingSettingDetailQueryProcessor, PeriodicBillingSettingDetailQueryProcessor>();
            container.Register<IDeletePeriodicBillingSettingDetailQueryProcessor, PeriodicBillingSettingDetailQueryProcessor>();


            //Reminder
            container.Register<IReminderQueryProcessor, ReminderQueryProcessor>();
            container.Register<IAddReminderQueryProcessor, ReminderQueryProcessor>();
            container.Register<IUpdateReminderQueryProcessor, ReminderQueryProcessor>();
            container.Register<IUpdateBillingReminderQueryProcessor, ReminderQueryProcessor>();
            container.Register<IDeleteReminderSummaryQueryProcessor, ReminderQueryProcessor>();

            //ReminderHistory
            container.Register<IReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();
            container.Register<IAddReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();
            container.Register<IUpdateReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();
            container.Register<IDeleteReminderHistoryQueryProcessor, ReminderHistoryQueryProcessor>();

            //ReminderOutputed
            container.Register<IReminderOutputedQueryProcessor, ReminderOutputedQueryProcessor>();
            container.Register<IReminderOutputedExistsQueryProcessor, ReminderOutputedQueryProcessor>();

            //ReminderCommonSetting
            container.Register<IAddReminderCommonSettingQueryProcessor, ReminderCommonSettingQueryProcessor>();

            //ReminderTemplateSetting
            container.Register<IAddReminderTemplateSettingQueryProcessor, ReminderTemplateSettingQueryProcessor>();

            //ReminderLevelSetting
            container.Register<IReminderLevelSettingQueryProcessor, ReminderLevelSettingQueryProcessor>();
            container.Register<IAddReminderLevelSettingQueryProcessor, ReminderLevelSettingQueryProcessor>();
            container.Register<IDeleteReminderLevelSettingQueryProcessor, ReminderLevelSettingQueryProcessor>();

            //ReminderSummarySetting
            container.Register<IAddReminderSummarySettingQueryProcessor, ReminderSummarySettingQueryProcessor>();
            container.Register<IReminderSummarySettingQueryProcessor, ReminderSummarySettingQueryProcessor>();

            // ReportSetting
            container.Register<IReportSettingQueryProcessor, ReportSettingQueryProcessor>();
            container.Register<IAddReportSettingQueryProcessor, ReportSettingQueryProcessor>();
            container.Register<IDeleteReportSettingQueryProcessor, ReportSettingQueryProcessor>();


            // Staff
            container.Register<IStaffQueryProcessor, StaffQueryProcessor>();
            container.Register<IAddStaffQueryProcessor, StaffQueryProcessor>();

            // Section
            container.Register<ISectionQueryProcessor, SectionQueryProcessor>();
            container.Register<IAddSectionQueryProcessor, SectionQueryProcessor>();

            // SectionWithDepartment
            container.Register<ISectionWithDepartmentQueryProcessor, SectionWithDepartmentQueryProcessor>();
            container.Register<IAddSectionWithDepartmentQueryProcessor, SectionWithDepartmentQueryProcessor>();
            container.Register<IDeleteSectionWithDepartmentQueryProcessor, SectionWithDepartmentQueryProcessor>();

            // SectionWithLoginUser
            container.Register<ISectionWithLoginUserQueryProcessor, SectionWithLoginUserQueryProcessor>();
            container.Register<IAddSectionWithLoginUserQueryProcessor, SectionWithLoginUserQueryProcessor>();
            container.Register<IDeleteSectionWithLoginUserQueryProcessor, SectionWithLoginUserQueryProcessor>();

            // Setting
            container.Register<ISettingQueryProcessor, SettingQueryProcessor>();

            //Status
            container.Register<IAddStatusQueryProcessor, StatusQueryProcessor>();
            container.Register<IDeleteStatusQueryProcessor, StatusQueryProcessor>();
            container.Register<IStatusQueryProcessor, StatusQueryProcessor>();
            container.Register<IStatusExistsQueryProcessor, StatusQueryProcessor>();

            // TaskSchedule
            container.Register<ITaskScheduleQueryProcessor, TaskScheduleQueryProcessor>();

            // TaxClass
            container.Register<ITaxClassQueryProcessor, TaxClassQueryProcessor>();

            // PaymentFileFormat
            container.Register<IPaymentFileFormatQueryProcessor, PaymentFileFormatQueryProcessor>();

            // WebApiSetting
            container.Register<IWebApiSettingQueryProcessor, WebApiSettingQueryProcessor>();
            container.Register<IAddWebApiSettingQueryProcessor, WebApiSettingQueryProcessor>();
            container.Register<IDeleteWebApiSettingQueryProcessor, WebApiSettingQueryProcessor>();

        }

        private void ConfigureTransactionsQueryProcessors()
        {
            // AccountTransfer Log / Detail
            container.Register<IAccountTransferLogQueryProcessor, AccountTransferLogQueryProcessor>();
            container.Register<IAddAccountTransferLogQueryProcessor, AccountTransferLogQueryProcessor>();
            container.Register<IAccountTransferDetailQueryProcessor, AccountTransferDetailQueryProcessor>();
            container.Register<IAddAccountTransferDetailQueryProcessor, AccountTransferDetailQueryProcessor>();
            container.Register<IUpdateBillingAccountTransferLogQueryProcessor, AccountTransferDetailQueryProcessor>();
            container.Register<IDeleteAccountTransferDetailQueryProcessor, AccountTransferDetailQueryProcessor>();

            // Billing
            container.Register<IBillingQueryProcessor, BillingQueryProcessor>();
            container.Register<IAddBillingQueryProcessor, BillingQueryProcessor>();
            container.Register<IUpdateBillingQueryProcessor, BillingQueryProcessor>();
            container.Register<IBillingExistsQueryProcessor, BillingExistsQueryProcessor>();
            container.Register<IBillingJournalizingQueryProcessor, BillingJournalizingQueryProcessor>();

            container.Register<IBillingAgingListQueryProcessor, BillingAgingListQueryProcessor>();

            // BillingDueAtModify
            container.Register<IBillingDueAtModifySearchQueryProcessor, BillingDueAtModifySearchQueryProcessor>();
            container.Register<IUpdateBillingDueAtModifyQueryProcessor, BillingDueAtModifyQueryProcessor>();

            // BillingBalance
            container.Register<IBillingBalanceQueryProcessor, BillingBalanceQueryProcessor>();
            container.Register<IAddBillingBalanceQueryProcessor, BillingBalanceQueryProcessor>();
            container.Register<IDeleteBillingBalanceQueryProcessor, BillingBalanceQueryProcessor>();

            // BillingBalanceBacck
            container.Register<IAddBillingBalanceBackQueryProcessor, BillingBalanceBackQueryProcessor>();

            // BillingDiscount
            container.Register<IAddBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();
            container.Register<IUpdateBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();
            container.Register<IBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();
            container.Register<IDeleteBillingDiscountQueryProcessor, BillingDiscountQueryProcessor>();

            // BillingDivisionContract
            container.Register<IBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();
            container.Register<IAddBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();
            container.Register<IUpdateBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();
            container.Register<IDeleteBillingDivisionContractQueryProcessor, BillingDivisionContractQueryProcessor>();


            // BillingScheduledIncome
            container.Register<IAddBillingScheduledIncomeQueryProcessor, BillingScheduledIncomeQueryProcessor>();

            // BillingSearch
            container.Register<IBillingSearchQueryProcessor, BillingSearchQueryProcessor>();
            container.Register<IBillingSearchForImportQueryProcessor, BillingSearchQueryProcessor>();

            // BillingMemo
            container.Register<IBillingMemoQueryProcessor, BillingMemoQueryProcessor>();
            container.Register<IUpdateBillingMemoQueryProcessor, BillingMemoQueryProcessor>();
            container.Register<IDeleteBillingMemoQueryProcessor, BillingMemoQueryProcessor>();

            // BillingInput
            container.Register<IAddBillingInputQueryProcessor, BillingInputQueryProcessor>();

            // BillingInvoice
            container.Register<IBillingInvoiceQueryProcessor, BillingInvoiceQueryProcessor>();

            // CustomerLedger
            container.Register<ICustomerLedgerQueryProcessor, CustomerLedgerQueryProcessor>();

            // DataMaintenance
            container.Register<IDataMaintenanceQueryProcessor, DataMaintenanceQueryProcessor>();

            // LogData
            container.Register<ILogDataByCompanyIdQueryProcessor, LogDataQueryProcessor>();
            container.Register<IAddLogDataQueryProcessor, LogDataQueryProcessor>();

            // Collation
            container.Register<ICollationQueryProcessor, CollationQueryProcessor>();

            // Matching
            container.Register<IMatchingQueryProcessor, MatchingQueryProcessor>();
            container.Register<IAddMatchingQueryProcessor, MatchingQueryProcessor>();
            container.Register<ICancelMatchingQueryProcessor, MatchingQueryProcessor>();

            // MatchingHeader
            container.Register<IUpdateMatchingHeaderQueryProcessor, MatchingHeaderQueryProcessor>();

            // MatchingOrder
            container.Register<IMatchingOrderQueryProcessor, MatchingOrderQueryProcessor>();

            // MatchingBillingDiscount
            container.Register<IAddMatchingBillingDiscountQueryProcessor, MatchingBillingDiscountQueryProcessor>();
            container.Register<IDeleteMatchingBillingDiscountQueryProcessor, MatchingBillingDiscountQueryProcessor>();

            // MatchingHistory 消込履歴データ検索
            container.Register<IMatchingHistorySearchQueryProcessor, MatchingHistorySearchQueryProcessor>();
            container.Register<IAddMatchingOutputedQueryProcessor, MatchingOutputedQueryProcessor>();

            // MatchingJournalizing 消込仕訳
            container.Register<IMatchingJournalizingQueryProcessor, MatchingJournalizingQueryProcessor>();
            container.Register<IMatchingJournalizingSummaryQueryProcessor, MatchingJournalizingQueryProcessor>();
            container.Register<IMatchingGeneralJournalizingQueryProcessor, MatchingJournalizingQueryProcessor>();
            container.Register<IUpdateMatchingJournalizingQueryProcessor, MatchingJournalizingQueryProcessor>();
            container.Register<IMatchedReceiptQueryProcessor, MatchingJournalizingQueryProcessor>();
            container.Register<IUpdateReceiptMatchingJournalizingQueryProcessor, ReceiptMatchingJournalizingQueryProcessor>();
            container.Register<IUpdateReceiptExcludeJournalizingQueryProcessor, ReceiptExcludeQueryProcessor>();
            container.Register<IUpdateAdvanceReceivedBackupJournalizingQueryProcessor, AdvanceReceivedBackupQueryProcessor>();
            container.Register<IMatchingJournalizingDetailQueryProcessor, MatchingJournalizingQueryProcessor>();

            //MFBilling
            container.Register<IMFBillingQueryProcessor, MFBillingQueryProcessor>();
            container.Register<IAddMFBillingQueryProcessor, MFBillingQueryProcessor>();

            // Netting
            container.Register<INettingQueryProcessor, NettingQueryProcessor>();
            container.Register<IAddNettingQueryProcessor, NettingQueryProcessor>();
            container.Register<IUpdateNettingQueryProcessor, NettingQueryProcessor>();

            // NettingSearch
            container.Register<INettingSearchQueryProcessor, NettingSearchQueryProcessor>();

            // PreiodicBillingHistory
            container.Register<IAddPeriodicBillingCreatedQueryProcessor, PeriodicBillingCreatedQueryProcessor>();

            // Receipt
            container.Register<IReceiptQueryProcessor, ReceiptQueryProcessor>();
            container.Register<IReceiptExistsQueryProcessor, ReceiptQueryProcessor>();
            container.Register<IReceiptSearchQueryProcessor, ReceiptSearchQueryProcessor>();
            container.Register<IDeleteReceiptQueryProcessor, ReceiptQueryProcessor>();
            container.Register<IUpdateReceiptQueryProcessor, ReceiptQueryProcessor>();
            container.Register<IAddReceiptQueryProcessor, ReceiptQueryProcessor>();
            container.Register<IReceiptJournalizingQueryProcessor, ReceiptJournalizingQueryProcessor>();
            container.Register<IReceiptGeneralJournalizingQueryProcessor, ReceiptJournalizingQueryProcessor>();
            container.Register<IUpdateReceiptJournalizingQueryProcessor, ReceiptJournalizingQueryProcessor>();

            // ReceiptExclude
            container.Register<IReceiptExcludeQueryProcessor, ReceiptExcludeQueryProcessor>();
            container.Register<IAddReceiptExcludeQueryProcessor, ReceiptExcludeQueryProcessor>();
            container.Register<IDeleteReceiptExcludeQueryProcessor, ReceiptExcludeQueryProcessor>();

            // ReceiptMemo
            container.Register<IReceiptMemoQueryProcessor, ReceiptMemoQueryProcessor>();
            container.Register<IAddReceiptMemoQueryProcessor, ReceiptMemoQueryProcessor>();
            container.Register<IDeleteReceiptMemoQueryProcessor, ReceiptMemoQueryProcessor>();


            // Receipt Apportion 入金データ振分
            container.Register<IUpdateReceiptApportionQueryProcessor, ReceiptQueryProcessor>();

            // ReceiptHeader
            container.Register<IUpdateReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();
            container.Register<IReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();
            container.Register<IReceiptApportionByIdQueryProcessor, ReceiptQueryProcessor>();
            container.Register<IAddReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();
            container.Register<IDeleteReceiptHeaderQueryProcessor, ReceiptHeaderQueryProcessor>();

            // ReceiptSectionTransfer 入金部門振替処理
            container.Register<IReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();
            container.Register<IAddReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();
            container.Register<IDeleteReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();
            container.Register<IUpdateReceiptSectionTransferQueryProcessor, ReceiptSectionTransferQueryProcessor>();

            // Report
            container.Register<IArrearagesListQueryProcessor, ArrearagesListQueryProcessor>();
            container.Register<ICollectionScheduleQueryProcessor, CollectionScheduleQueryProcessor>();
            container.Register<ICreditAgingListQueryProcessor, CreditAgingListQueryProcessor>();
            container.Register<IScheduledPaymentListQueryProcessor, ScheduledPaymentListQueryProcessor>();

            // SessionStorage
            container.Register<ISessionStorageQueryProcessor, SessionStorageQueryProcessor>();

            // TaskScheduleHistory
            container.Register<ITaskScheduleHistoryQueryProcessor, TaskScheduleHistoryQueryProcessor>();
            container.Register<IAddTaskScheduleHistoryQueryProcessor, TaskScheduleHistoryQueryProcessor>();

            // ImportFileLog
            container.Register<IImportFileLogQueryProcessor, ImportFileLogQueryProcessor>();
            container.Register<IAddImportFileLogQueryProcessor, ImportFileLogQueryProcessor>();

            // Work***Target
            container.Register<IWorkDepartmentTargetQueryProcessor, WorkDepartmentTargetQueryProcessor>();
            container.Register<IWorkSectionTargetQueryProcessor, WorkSectionTargetQueryProcessor>();

            container.Register<IHatarakuDBJournalizingQueryProcessor, HatarakuDBJournalizingQueryProcessor>();


        }

    }
}
