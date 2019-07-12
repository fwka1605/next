using MsgPack.Serialization;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Importers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Import
{
    /// <summary>請求フリーインポーター 検証・インポート処理</summary>
    public class BillingFileImportProcessor : IBillingFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IImporterSettingProcessor importerSettingProcessor;
        private readonly IImporterSettingDetailProcessor importerSettingDetailProcessor;
        private readonly IGeneralSettingProcessor generalSettingProcessor;
        private readonly ICurrencyProcessor currencyProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly IJuridicalPersonalityProcessor juridicalPersonalityProcessor;
        private readonly ITaxClassProcessor taxClassProcessor;
        private readonly IHolidayCalendarProcessor holidayCalendarProcessor;
        private readonly ICollationSettingProcessor collationSettingProcessor;
        private readonly IColumnNameSettingProcessor columnNameSettingProcessor;
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly IAccountTitleProcessor accountTitleProcessor;
        private readonly IStaffProcessor staffProcessor;
        private readonly IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor;
        private readonly IImportDataProcessor importDataProcessor;

        private readonly IBillingProcessor billingProcessor;
        private readonly IBillingImporterProcessor billingImporterProcessor;
        private readonly IBillingDivisionContractProcessor billingDivisionContractProcessor;

        private readonly MessagePackSerializer<BillingImport> serializer;

        /// <summary>constructor</summary>
        public BillingFileImportProcessor(
            ICompanyProcessor companyProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IImporterSettingProcessor importerSettingProcessor,
            IImporterSettingDetailProcessor importerSettingDetailProcessor,
            IGeneralSettingProcessor generalSettingProcessor,
            ICurrencyProcessor currencyProcessor,
            ICategoryProcessor categoryProcessor,
            ICustomerProcessor customerProcessor,
            IJuridicalPersonalityProcessor juridicalPersonalityProcessor,
            ITaxClassProcessor taxClassProcessor,
            IHolidayCalendarProcessor holidayCalendarProcessor,
            ICollationSettingProcessor collationSettingProcessor,
            IColumnNameSettingProcessor columnNameSettingProcessor,
            IDepartmentProcessor departmentProcessor,
            IAccountTitleProcessor accountTitleProcessor,
            IStaffProcessor staffProcessor,
            IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor,
            IImportDataProcessor importDataProcessor,

            IBillingProcessor billingProcessor,
            IBillingImporterProcessor billingImporterProcessor,
            IBillingDivisionContractProcessor billingDivisionContractProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.importerSettingProcessor = importerSettingProcessor;
            this.importerSettingDetailProcessor = importerSettingDetailProcessor;
            this.generalSettingProcessor = generalSettingProcessor;
            this.currencyProcessor = currencyProcessor;
            this.categoryProcessor = categoryProcessor;
            this.customerProcessor = customerProcessor;
            this.juridicalPersonalityProcessor = juridicalPersonalityProcessor;
            this.taxClassProcessor = taxClassProcessor;
            this.holidayCalendarProcessor = holidayCalendarProcessor;
            this.collationSettingProcessor = collationSettingProcessor;
            this.columnNameSettingProcessor = columnNameSettingProcessor;
            this.departmentProcessor = departmentProcessor;
            this.accountTitleProcessor = accountTitleProcessor;
            this.staffProcessor = staffProcessor;
            this.invoiceCommonSettingProcessor = invoiceCommonSettingProcessor;
            this.importDataProcessor = importDataProcessor;

            this.billingProcessor = billingProcessor;
            this.billingImporterProcessor = billingImporterProcessor;
            this.billingDivisionContractProcessor = billingDivisionContractProcessor;

            serializer = MessagePackSerializer.Get<BillingImport>(new SerializationContext { DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native });
        }

        private async Task<ImporterSetting> GetImporterSettingAsync(int settingId, CancellationToken token)
            => (await importerSettingProcessor.GetAsync(new ImporterSetting { Id = settingId, }, token)).FirstOrDefault();

        private async Task<List<ImporterSettingDetail>> GetImporterSettingDetailsAsync(int settingId, CancellationToken token)
            => (await importerSettingDetailProcessor.GetAsync(new ImporterSetting { Id = settingId, }, token)).ToList();

        private async Task<List<Currency>> GetCurrenciesAsync(int companyId, CancellationToken token)
            => (await currencyProcessor.GetAsync(new CurrencySearch { CompanyId = companyId, }, token)).ToList();

        private async Task<IEnumerable<string>> GetJuridicalParsonalitiesAsync(int companyId, CancellationToken token)
            => (await juridicalPersonalityProcessor.GetAsync(new JuridicalPersonality { CompanyId = companyId, }, token)).Select(x => x.Kana).ToArray();

        private async Task<string> GetGeneralSettingValueAsync(int companyId, string code, CancellationToken token)
            => (await generalSettingProcessor.GetAsync(new GeneralSetting { CompanyId = companyId, Code = code, }, token)).FirstOrDefault()?.Value;

        private async Task<List<TaxClass>> GetTaxClassesAsync(CancellationToken token)
            => (await taxClassProcessor.GetAsync(token)).ToList();

        private async Task<List<BillingDivisionContract>> GetBillingDivisionsByCustomerIdsAsync(IEnumerable<int> ids, CancellationToken token)
            => (await billingDivisionContractProcessor.GetAsync(new BillingDivisionContractSearch { CustomerIds = ids.ToArray(), }, token)).ToList();

        private async Task<List<HolidayCalendar>> GetHolidayCalendarsAsync(int companyId, CancellationToken token)
            => (await holidayCalendarProcessor.GetAsync(new HolidayCalendarSearch { CompanyId = companyId, }, token)).ToList();

        private async Task<ColumnNameSetting[]> GetColumnNamesAsync(int companyId, string tableName, CancellationToken token)
            => (await columnNameSettingProcessor.GetAsync(new ColumnNameSetting { CompanyId = companyId, TableName = tableName, }, token)).ToArray();

        private async Task<int[]> GetDuplicationRows(int companyId, BillingImportDuplicationWithCode[] items, ImporterSettingDetail[] details, CancellationToken token)
            => (await billingProcessor.BillingImportDuplicationCheckAsync(companyId, items, details, token)).ToArray();

        private async Task<List<Customer>> GetCustomersAsync(int companyId, string[] codes, CancellationToken token)
            => (await customerProcessor.GetAsync(new CustomerSearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<Department>> GetDepartmntsAdync(int companyId, string[] codes, CancellationToken token)
            => (await departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<AccountTitle>> GetAccountTitlesAsync(int companyId, string[] codes, CancellationToken token)
            => (await accountTitleProcessor.GetAsync(new AccountTitleSearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<Staff>> GetStaffsAsync(int companyId, string[] codes, CancellationToken token)
            => (await staffProcessor.GetAsync(new StaffSearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<Category>> GetCategoriesAsync(int companyId, int categoryType, string[] codes, CancellationToken token)
            => (await categoryProcessor.GetAsync(new CategorySearch { CompanyId = companyId, CategoryType = categoryType, Codes = codes, }, token)).ToList();

        private async Task<bool> GetIsEnableToEditNoteAsync(int companyId, CancellationToken token)
        {
            var result = await invoiceCommonSettingProcessor.GetAsync(companyId, token);
            return result?.ControlInputCharacter == 1;
        }


        /// <summary>読み込み・検証・一時データ登録処理</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ImportDataResult> ReadAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var encoding = Encoding.GetEncoding(source.EncodingCodePage);
            var csv = encoding.GetString(source.Data);

            var companyTask = companyProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, appConTask);

            var company = companyTask.Result.First();
            var applicationControl = appConTask.Result;

            var importer = new BillingImporterBase(applicationControl) {
                CompanyId           = source.CompanyId,
                CompanyCode         = company.Code,
                LoginUserId         = source.LoginUserId,
                ImporterSettingId   = source.ImporterSettingId,
                FilePath            = csv,
                CsvParser           = new CsvParser {
                    Encoding            = encoding,
                    StreamCreator       = new PlainTextMemoryStreamCreator(),
                },
                GetImporterSettingAsync         = settingId => GetImporterSettingAsync(settingId, token),
                GetImporterSettingDetailAsync   = settingId => GetImporterSettingDetailsAsync(settingId, token),
                GetCurrencyAsync                = companyId => GetCurrenciesAsync(companyId, token),
                GetJuridicalParsonalitiesAsync  = companyId => GetJuridicalParsonalitiesAsync(companyId, token),
                GetGeneralSettingValueAsync     = (companyId, code) => GetGeneralSettingValueAsync(companyId, code, token),
                GetTaxClassAsync                = () => GetTaxClassesAsync(token),
                GetBillingDivisionContractByCustomerIdsAsync = ids => GetBillingDivisionsByCustomerIdsAsync(ids, token),
                GetHolidayCalendarsAsync        = companyId => GetHolidayCalendarsAsync(companyId, token),
                LoadColumnNameSettingsInnerAsync = tableName => GetColumnNamesAsync(source.CompanyId, tableName, token),
                BillingImportDuplicationCheckAsync = (companyId, items, details) => GetDuplicationRows(companyId, items, details, token),
                GetCustomerByCodesAsync = (companyId, codes) => GetCustomersAsync(companyId, codes, token),
                GetDepartmentByCodesAsync = (companyId, codes) => GetDepartmntsAdync(companyId, codes, token),
                GetAccountTitleByCodesAsync = (companyId, codes) => GetAccountTitlesAsync(companyId, codes, token),
                GetStaffByCodesAsync = (companyId, codes) => GetStaffsAsync(companyId, codes, token),
                GetCategoriesByCodesAsync = (companyId, categoryType, codes) => GetCategoriesAsync(companyId, categoryType, codes, token),
                GetIsEnableToEditNoteAsync = companyId => GetIsEnableToEditNoteAsync(companyId, token),

                Serialize  = item => serializer.PackSingleObject(item),
                SaveImportDataAsync = data => importDataProcessor.SaveAsync(data, token),
            };
            var readResult = await importer.ReadCsvAsync();
            return new ImportDataResult {
                ImportData      = importer.ImportData,
                ReadCount       = importer.ReadCount,
                ValidCount      = importer.ValidCount,
                InvalidCount    = importer.InvalidCount,
                NewCustomerCreationCount = importer.NewCustomerCreationCount,
                Logs            = importer.GetValidationLogs(),
            };
        }


        /// <summary>インポート処理 データ登録</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ImportDataResult> ImportAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var companyTask = companyProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, appConTask);

            var company = companyTask.Result.First();
            var applicationControl = appConTask.Result;

            var importDataId = source.ImportDataId ?? 0;

            var importer = new BillingImporterBase(applicationControl) {
                CompanyId                   = source.CompanyId,
                CompanyCode                 = company.Code,
                LoginUserId                 = source.LoginUserId,
                ImporterSettingId           = source.ImporterSettingId,
                Deserialize         = bytes => serializer.UnpackSingleObject(bytes),
                LoadImportDataAsync = () => importDataProcessor.GetAsync(importDataId, objectType: 0, token: token),
                ImportInnerAsync = (companyId, loginUserId, settingId, items) => billingImporterProcessor.ImportAsync(new BillingImportItems {
                    CompanyId           = companyId,
                    LoginUserId         = loginUserId,
                    ImporterSettingId   = settingId,
                    Items               = items,
                }, token),
            };

            var result = await importer.ImportAsync();
            return new ImportDataResult {
                SaveCount = importer.SaveCount,
                SaveAmount = importer.SaveAmount,
            };
        }

        /// <summary>得意先取得</summary>
        /// <param name="source">CompanyId, ImprotSettingId, ImportDataId</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Customer>> GetCustomers(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var importDataId    = source.ImportDataId ?? 0;
            var detailsTask     = GetImporterSettingDetailsAsync(source.ImporterSettingId, token);
            var importDataTask  = importDataProcessor.GetAsync(importDataId);
            var categoryTask    = categoryProcessor.GetAsync(new CategorySearch { CompanyId = source.CompanyId, CategoryType = Rac.VOne.Common.CategoryType.Collect, }, token);

            await Task.WhenAll(detailsTask, importDataTask, categoryTask);

            var details             = detailsTask.Result;
            var importData          = importDataTask.Result;
            var categoryDictionary  = categoryTask.Result.ToDictionary(x => x.Code);

            var billings = importData.Details.Select(x => serializer.UnpackSingleObject(x.RecordItem));

            return billings.ConvertToCustomers(details, source.LoginUserId, categoryDictionary).ToArray();

        }
    }
}