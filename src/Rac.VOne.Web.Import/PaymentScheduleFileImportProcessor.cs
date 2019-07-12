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
    /// <summary>入金予定フリーインポーター 読込・検証・登録処理</summary>
    public class PaymentScheduleFileImportProcessor : IPaymentScheduleFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IImporterSettingProcessor importerSettingProcessor;
        private readonly IImporterSettingDetailProcessor importerSettingDetailProcessor;
        private readonly IGeneralSettingProcessor generalSettingProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly ICurrencyProcessor currencyProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly IAccountTitleProcessor accountTitleProcessor;
        private readonly IBillingScheduledPaymentProcessor billingScheduledPaymentProcessor;
        private readonly IImportDataProcessor importDataProcessor;

        private readonly MessagePackSerializer<ScheduledPaymentImport> serializerScheduledPaymentImport;
        private readonly MessagePackSerializer<Models.Files.PaymentSchedule> serializerPaymentSchedule;

        /// <summary>constructor</summary>
        public PaymentScheduleFileImportProcessor(
            ICompanyProcessor companyProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IImporterSettingProcessor importerSettingProcessor,
            IImporterSettingDetailProcessor importerSettingDetailProcessor,
            IGeneralSettingProcessor generalSettingProcessor,
            ICustomerProcessor customerProcessor,
            ICurrencyProcessor currencyProcessor,
            ICategoryProcessor categoryProcessor,
            IDepartmentProcessor departmentProcessor,
            IAccountTitleProcessor accountTitleProcessor,
            IBillingScheduledPaymentProcessor billingScheduledPaymentProcessor,
            IImportDataProcessor importDataProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.importerSettingProcessor = importerSettingProcessor;
            this.importerSettingDetailProcessor = importerSettingDetailProcessor;
            this.generalSettingProcessor = generalSettingProcessor;
            this.customerProcessor = customerProcessor;
            this.currencyProcessor = currencyProcessor;
            this.categoryProcessor = categoryProcessor;
            this.departmentProcessor = departmentProcessor;
            this.accountTitleProcessor = accountTitleProcessor;
            this.billingScheduledPaymentProcessor = billingScheduledPaymentProcessor;
            this.importDataProcessor = importDataProcessor;

            serializerScheduledPaymentImport = MessagePackSerializer.Get<ScheduledPaymentImport>(new SerializationContext { DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native });
            serializerPaymentSchedule = MessagePackSerializer.Get<Models.Files.PaymentSchedule>(new SerializationContext { DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native });
        }

        private async Task<Company> GetCompanyAsync(int companyId, CancellationToken token)
            => (await companyProcessor.GetAsync(new CompanySearch { Id = companyId, }, token)).First();

        private async Task<ApplicationControl> GetApplicationControlAsync(int companyId, CancellationToken token)
            => await applicationControlProcessor.GetAsync(companyId, token);

        private async Task<ImporterSetting> GetImporterSettingAsync(int settingId, CancellationToken token)
            => (await importerSettingProcessor.GetAsync(new ImporterSetting { Id = settingId }, token)).FirstOrDefault();

        private async Task<List<ImporterSettingDetail>> GetImporterSettingDetailsAsync(int settingId, CancellationToken token)
            => (await importerSettingDetailProcessor.GetAsync(new ImporterSetting { Id = settingId }, token)).ToList();

        private async Task<string> GetGeneranSettingValueAsync(int companyId, string code, CancellationToken token)
            => (await generalSettingProcessor.GetAsync(new GeneralSetting { CompanyId = companyId, Code = code, }, token)).FirstOrDefault()?.Value;

        private async Task<List<Customer>> GetCustomersAsync(int companyId, string[] codes, CancellationToken token)
            => (await customerProcessor.GetAsync(new CustomerSearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<Department>> GetDepartmentsAsync(int companyId, string[] codes, CancellationToken token)
            => (await departmentProcessor.GetAsync(new DepartmentSearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<AccountTitle>> GetAccountTitlesAsync(int companyId, string[] codes, CancellationToken token)
            => (await accountTitleProcessor.GetAsync(new AccountTitleSearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<Category>> GetCategoriesAsync(int companyId, int categoryType, string[] codes, CancellationToken token)
            => (await categoryProcessor.GetAsync(new CategorySearch { CompanyId = companyId, CategoryType = categoryType, Codes = codes, }, token)).ToList();

        private async Task<List<Currency>> GetCurrenciesAsync(int companyId, string[] codes, CancellationToken token)
            => (await currencyProcessor.GetAsync(new CurrencySearch { CompanyId = companyId, Codes = codes, }, token)).ToList();

        private async Task<List<Billing>> GetBillingsAsync(int companyId, ScheduledPaymentImport[] items, ImporterSettingDetail[] details, CancellationToken token)
            => (await billingScheduledPaymentProcessor.GetAsync(new BillingScheduledPaymentImportSource {
                CompanyId   = companyId,
                Details     = details,
                Items       = items,
            }, token)).ToList();

        /// <summary>読込・検証処理</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ImportDataResult> ReadAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var encoding = Encoding.GetEncoding(source.EncodingCodePage);
            var csv = encoding.GetString(source.Data);

            var companyTask = GetCompanyAsync(source.CompanyId, token);
            var appConTask  = GetApplicationControlAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, appConTask);

            var company = companyTask.Result;
            var applicationControl = appConTask.Result;

            var importer = new PaymentScheduleImporterBase(applicationControl) {
                CompanyId           = source.CompanyId,
                CompanyCode         = company.Code,
                LoginUserId         = source.LoginUserId,
                ImporterSettingId   = source.ImporterSettingId,
                FilePath            = csv,
                CsvParser           = new CsvParser {
                    Encoding            = encoding,
                    StreamCreator       = new PlainTextMemoryStreamCreator(),
                },
                DoTargetNotMatchedData      = source.DoTargetNotMatchedData,
                DoReplaceAmount             = source.DoReplaceAmount,
                DoIgnoreSameCustomerGroup   = source.DoIgnoreSameCustomerGroup,

                GetImporterSettingAsync = settingId => GetImporterSettingAsync(settingId, token),
                GetImporterSettingDetailByIdAsync = settingId => GetImporterSettingDetailsAsync(settingId, token),
                GetItemsForScheduledPaymentImportAsync = (companyId, items, details) => GetBillingsAsync(companyId, items, details, token),
                GetGeneralSettingValueAsync = (companyId, code) => GetGeneranSettingValueAsync(companyId, code, token),
                GetCustomerByCodesAsync = (companyId, codes) => GetCustomersAsync(companyId, codes, token),
                GetDepartmentByCodesAsync = (companyId, codes) => GetDepartmentsAsync(companyId, codes, token),
                GetAccountTitleByCodesAsync = (companyId, codes) => GetAccountTitlesAsync(companyId, codes, token),
                GetCategoriesByCodesAsync = (companyId, categoryType, codes) => GetCategoriesAsync(companyId, categoryType, codes, token),
                GetCurrenciesAsync = (companyId, codes) => GetCurrenciesAsync(companyId, codes, token),

                SerializeScheduledPaymentImport = item => serializerScheduledPaymentImport.PackSingleObject(item),
                SerializePaymentSchedule = item => serializerPaymentSchedule.PackSingleObject(item),
                SaveImportDataAsync = data => importDataProcessor.SaveAsync(data, token),
            };

            var readResult = importer.ReadCsvAsync();
            return new ImportDataResult {
                ImportData      = importer.ImportData,
                ReadCount       = importer.ReadCount,
                ValidCount      = importer.ValidCount,
                InvalidCount    = importer.InvalidCount,
                Logs            = importer.GetValidationLogs(),
            };
        }

        /// <summary>登録処理</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ImportDataResult> ImportAsync(TransactionImportSource source, CancellationToken token = default(CancellationToken))
        {
            var companyTask = GetCompanyAsync(source.CompanyId, token);
            var appConTask = GetApplicationControlAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, appConTask);

            var company = companyTask.Result;
            var applicationControl = appConTask.Result;

            var importDataId = source.ImportDataId ?? 0;

            var importer = new PaymentScheduleImporterBase(applicationControl) {
                CompanyId           = source.CompanyId,
                CompanyCode         = company.Code,
                LoginUserId         = source.LoginUserId,
                ImporterSettingId   = source.ImporterSettingId,

                DoTargetNotMatchedData      = source.DoTargetNotMatchedData,
                DoReplaceAmount             = source.DoReplaceAmount,
                DoIgnoreSameCustomerGroup   = source.DoIgnoreSameCustomerGroup,

                Deserialize = bytes => serializerScheduledPaymentImport.UnpackSingleObject(bytes),
                LoadImportDataAsync = () => importDataProcessor.GetAsync(importDataId, objectType: 0, token: token),
                SaveInnerAsync = async (companyId, loginUserId, settingId, items) => {
                    var results = (await billingScheduledPaymentProcessor.ImportAsync(new BillingScheduledPaymentImportSource { }, token)).ToArray();
                    return new ScheduledPaymentImportResult {
                        ProcessResult = new ProcessResult { Result = true, },
                        ScheduledPaymentImport = results,
                    };
                },
            };

            var result = await importer.ImportAsync();
            return new ImportDataResult {
                SaveCount   = importer.SaveCount,
                SaveAmount  = importer.SaveAmount,
            };
        }
    }
}