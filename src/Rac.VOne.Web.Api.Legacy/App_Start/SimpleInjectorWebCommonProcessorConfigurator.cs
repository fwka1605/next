using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;
using Rac.VOne.Web.Common;

using Rac.VOne.Web.Common.ThirdPartyApis;

namespace Rac.VOne.Web.Api.Legacy
{
    /// <summary>
    /// DI 登録 Web Common
    /// </summary>
    public class SimpleInjectorWebCommonProcessorConfigurator
    {
        private readonly Container container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public SimpleInjectorWebCommonProcessorConfigurator(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// 登録
        /// </summary>
        public void Configure()
        {
            ConfigureWebCommonProcessors();
        }

        private void ConfigureWebCommonProcessors()
        {
            container.Register<IAuthenticationProcessor, AuthenticationProcessor>();
            container.Register<IAuthenticationWebApiProcessor, AuthenticationWebApiProcessor>();
            container.Register<IAuthorizationProcessor, AuthorizationProcessor>();

            container.Register<IAccountTitleProcessor, AccountTitleProcessor>();
            container.Register<IAccountTransferProcessor, AccountTransferProcessor>();
            container.Register<IAdvanceReceivedBackupProcessor, AdvanceReceivedBackupProcessor>();
            container.Register<IApplicationControlProcessor, ApplicationControlProcessor>();

            container.Register<IBankAccountProcessor, BankAccountProcessor>();
            container.Register<IBankAccountTypeProcessor, BankAccountTypeProcessor>();
            container.Register<IBankBranchProcessor, BankBranchProcessor>();

            container.Register<IBillingAgingListProcessor, BillingAgingListProcessor>();

            container.Register<IBillingBalanceBackProcessor, BillingBalanceBackProcessor>();
            container.Register<IBillingBalanceProcessor, BillingBalanceProcessor>();
            container.Register<IBillingDiscountProcessor, BillingDiscountProcessor>();
            container.Register<IBillingDivisionContractProcessor, BillingDivisionContractProcessor>();
            container.Register<IBillingDivisionSettingProcessor, BillingDivisionSettingProcessor>();
            container.Register<IBillingDueAtModifyProcessor, BillingDueAtModifyProcessor>();
            container.Register<IBillingInputProcessor, BillingInputProcessor>();
            container.Register<IBillingInvoiceProcessor, BillingInvoiceProcessor>();
            container.Register<IBillingSaveProcessor, BillingSaveProcessor>();
            container.Register<IBillingSaveForInputProcessor, BillingSaveProcessor>();
            container.Register<IBillingProcessor, BillingProcessor>();
            container.Register<IBillingJournalizingProcessor, BillingJournalizingProcessor>();
            container.Register<IBillingMemoProcessor, BillingMemoProcessor>();
            container.Register<IBillingScheduledIncomeProcessor, BillingScheduledIncomeProcessor>();
            container.Register<IBillingSearchProcessor, BillingSearchProcessor>();
            container.Register<IBillingImporterProcessor, BillingImporterProcessor>();
            container.Register<IBillingImporterCodeToIdSolveProcessor, BillingImporterCodeToIdSolveProcessor>();
            container.Register<IBillingScheduledPaymentProcessor, BillingScheduledPaymentProcessor>();
            container.Register<IBillingAccountTransferProcessor, BillingAccountTransferProcessor>();

            container.Register<ICategoryProcessor, CategoryProcessor>();

            container.Register<IClosingProcessor, ClosingProcessor>();
            container.Register<IClosingSettingProcessor, ClosingSettingProcessor>();

            container.Register<ICollationSettingProcessor, CollationSettingProcessor>();
            container.Register<ICollectionScheduleProcessor, CollectionScheduleProcessor>();
            container.Register<IColumnNameSettingProcessor, ColumnNameSettingProcessor>();

            container.Register<IControlColorProcessor, ControlColorProcessor>();

            container.Register<ICompanyProcessor, CompanyProcessor>();
            container.Register<ICompanyLogoProcessor, CompanyLogoProcessor>();
            container.Register<ICompanyInitializeProcessor, CompanyInitializeProcessor>();

            container.Register<ICreditAgingListProcessor, CreditAgingListProcessor>();

            container.Register<ICurrencyProcessor, CurrencyProcessor>();
            container.Register<ICustomerGroupProcessor, CustomerGroupProcessor>();
            container.Register<ICustomerDiscountProcessor, CustomerDiscountProcessor>();
            container.Register<ICustomerFeeProcessor, CustomerFeeProcessor>();
            container.Register<ICustomerLedgerProcessor, CustomerLedgerProcessor>();
            container.Register<ICustomerPaymentContractProcessor, CustomerPaymentContractProcessor>();
            container.Register<ICustomerProcessor, CustomerProcessor>();

            container.Register<IDataMaintenanceProcessor, DataMaintenanceProcessor>();

            container.Register<IDbFunctionProcessor, DbFunctionProcessor>();
            container.Register<IDepartmentProcessor, DepartmentProcessor>();
            container.Register<IDestinationProcessor, DestinationProcessor>();

            container.Register<IEBExcludeAccountSettingProcessor, EBExcludeAccountSettingProcessor>();

            container.Register<IEBFormatProcessor, EBFormatProcessor>();
            container.Register<IEBFileSettingProcessor, EBFileSettingProcessor>();

            container.Register<IExportFieldSettingProcessor, ExportFieldSettingProcessor>();

            container.Register<IFunctionAuthorityProcessor, FunctionAuthorityProcessor>();

            container.Register<IGeneralSettingProcessor, GeneralSettingProcessor>();
            container.Register<IGridSettingProcessor, GridSettingProcessor>();

            container.Register<IHolidayCalendarProcessor, HolidayCalendarProcessor>();

            container.Register<IIgnoreKanaProcessor, IgnoreKanaProcessor>();

            container.Register<IImporterSettingDetailProcessor, ImporterSettingDetailProcessor>();
            container.Register<IImporterSettingProcessor, ImporterSettingProcessor>();

            container.Register<IImportFileLogProcessor, ImportFileLogProcessor>();

            container.Register<IImportSettingProcessor, ImportSettingProcessor>();

            container.Register<IInputControlProcessor, InputControlProcessor>();

            container.Register<IInvoiceCommonSettingProcessor, InvoiceCommonSettingProcessor>();
            container.Register<IInvoiceNumberHistoryProcessor, InvoiceNumberHistoryProcessor>();
            container.Register<IInvoiceNumberSettingProcessor, InvoiceNumberSettingProcessor>();
            container.Register<IInvoiceTemplateSettingProcessor, InvoiceTemplateSettingProcessor>();

            container.Register<IJuridicalPersonalityProcessor, JuridicalPersonalityProcessor>();

            container.Register<IKanaHistoryCustomerProcessor, KanaHistoryCustomerProcessor>();
            container.Register<IKanaHistoryPaymentAgencyProcessor, KanaHistoryPaymentAgencyProcessor>();

            container.Register<ILogDataProcessor, LogDataProcessor>();
            container.Register<ILoginUserLicenseProcessor, LoginUserLicenseProcessor>();
            container.Register<ILoginUserPasswordProcessor, LoginUserPasswordProcessor>();
            container.Register<ILoginUserProcessor, LoginUserProcessor>();
            container.Register<ILogsProcessor, LogsProcessor>();

            container.Register<ICollationProcessor, CollationProcessor>();
            container.Register<IMatchingCombinationSolveProcessor, MatchingCombinationSolveProcessor>();
            container.Register<IMatchingHeaderProcessor, MatchingHeaderProcessor>();
            container.Register<IMatchingHistorySearchProcessor, MatchingHistorySearchProcessor>();
            container.Register<IMatchingOutputedProcessor, MatchingOutputedProcessor>();
            container.Register<IMatchingProcessor, MatchingProcessor>();
            container.Register<IMatchingSaveProcessor, MatchingSaveProcessor>();
            container.Register<IMatchingSequentialProcessor, MatchingSequentialProcessor>();
            container.Register<IMatchingIndividualProcessor, MatchingIndividualProcessor>();
            container.Register<IMatchingCancellationProcessor, MatchingCancellationProcessor>();
            container.Register<IMatchingSolveProcessor, MatchingSolveProcessor>();
            container.Register<IMatchingJournalizingProcessor, MatchingJournalizingProcessor>();

            container.Register<IMenuAuthorityProcessor, MenuAuthorityProcessor>();

            container.Register<IMFBillingProcessor, MFBillingProcessor>();

            container.Register<INettingProcessor, NettingProcessor>();
            container.Register<INettingSearchProcessor, NettingSearchProcessor>();

            container.Register<IPasswordPolicyProcessor, PasswordPolicyProcessor>();

            container.Register<IPaymentAgencyProcessor, PaymentAgencyProcessor>();
            container.Register<IPaymentAgencyFeeProcessor, PaymentAgencyFeeProcessor>();
            container.Register<IPaymentFileFormatProcessor, PaymentFileFormatProcessor>();

            container.Register<IPdfOutputSettingProcessor, PdfOutputSettingProcessor>();

            container.Register<IPeriodicBillingSettingProcessor, PeriodicBillingSettingProcessor>();
            container.Register<IPeriodicBillingProcesssor, PeriodicBillingProcessor>();

            container.Register<IReceiptExcludeProcessor, ReceiptExcludeProcessor>();
            container.Register<IReceiptHeaderProcessor, ReceiptHeaderProcessor>();
            container.Register<IReceiptMemoProcessor, ReceiptMemoProcessor>();
            container.Register<IReceiptProcessor, ReceiptProcessor>();
            container.Register<IReceiptApportionProcessor, ReceiptApportionProcessor>();
            container.Register<IReceiptSearchProcessor, ReceiptSearchProcessor>();
            container.Register<IReceiptSectionTransferProcessor, ReceiptSectionTransferProcessor>();
            container.Register<IReceiptJournalizingProcessor, ReceiptJournalizingProcessor>();
            container.Register<IAdvanceReceivedProcessor, AdvanceReceivedProcessor>();
            container.Register<IAdvanceReceivedSplitProcessor, AdvanceReceivedSplitProcessor>();

            container.Register<IReminderProcessor, ReminderProcessor>();
            container.Register<IReminderHistoryProcessor, ReminderHistoryProcessor>();
            container.Register<IReminderCommonSettingProcessor, ReminderCommonSettingProcessor>();
            container.Register<IReminderTemplateSettingProcessor, ReminderTemplateSettingProcessor>();
            container.Register<IReminderLevelSettingProcessor, ReminderLevelSettingProcessor>();
            container.Register<IReminderSummarySettingProcessor, ReminderSummarySettingProcessor>();

            container.Register<IReportSettingProcessor, ReportSettingProcessor>();

            container.Register<IArrearagesListProcessor, ArrearagesListProcessor>();
            container.Register<IScheduledPaymentListProcessor, ScheduledPaymentListProcessor>();


            container.Register<ISectionProcessor, SectionProcessor>();
            container.Register<ISectionWithDepartmentProcessor, SectionWithDepartmentProcessor>();
            container.Register<ISectionWithLoginUserProcessor, SectionWithLoginUserProcessor>();
            container.Register<ISettingProcessor, SettingProcessor>();
            container.Register<IStaffProcessor, StaffProcessor>();
            container.Register<IStatusProcessor, StatusProcessor>();

            container.Register<ISessionStorageProcessor, SessionStorageProcessor>();

            container.Register<ISynchronizationProcessor, SynchronizationProcessor>();

            container.Register<ITaskScheduleProcessor, TaskScheduleProcessor>();
            container.Register<ITaskScheduleHistoryProcessor, TaskScheduleHistoryProcessor>();
            container.Register<ITaxClassProcessor, TaxClassProcessor>();

            container.Register<IWebApiSettingProcessor, WebApiSettingProcessor>();
            container.Register<IHatarakuDBJournalizingProcessor, HatarakuDBJournalizingProcessor>();


            container.Register<IImportDataProcessor, ImportDataProcessor>();

            container.Register<IMFWebApiProcessor, MFWebApiProcessor>();
        }
    }
}