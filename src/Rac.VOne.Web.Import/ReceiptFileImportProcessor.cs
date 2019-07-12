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
    /// <summary>入金フリーインポーター 検証・インポート処理</summary>
    public class ReceiptFileImportProcessor : IReceiptFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly IImporterSettingProcessor importerSettingProcessor;
        private readonly IImporterSettingDetailProcessor importerSettingDetailProcessor;
        private readonly IGeneralSettingProcessor generalSettingProcessor;
        private readonly ICurrencyProcessor currencyProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly ISectionProcessor sectionProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly IJuridicalPersonalityProcessor juridicalPersonalityProcessor;
        private readonly ICollationSettingProcessor collationSettingProcessor;
        private readonly IColumnNameSettingProcessor columnNameSettingProcessor;
        private readonly IImportDataProcessor importDataProcessor;
        private readonly IReceiptProcessor receiptProcessor;

        private readonly MessagePackSerializer<ReceiptInput> serializer;


        /// <summary>constructor</summary>
        public ReceiptFileImportProcessor(
            ICompanyProcessor companyProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            IImporterSettingProcessor importerSettingProcessor,
            IImporterSettingDetailProcessor importerSettingDetailProcessor,
            IGeneralSettingProcessor generalSettingProcessor,
            ICurrencyProcessor currencyProcessor,
            ICategoryProcessor categoryProcessor,
            ISectionProcessor sectionProcessor,
            ICustomerProcessor customerProcessor,
            IJuridicalPersonalityProcessor juridicalPersonalityProcessor,
            ICollationSettingProcessor collationSettingProcessor,
            IColumnNameSettingProcessor columnNameSettingProcessor,
            IImportDataProcessor importDataProcessor,
            IReceiptProcessor receiptProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.importerSettingProcessor = importerSettingProcessor;
            this.importerSettingDetailProcessor = importerSettingDetailProcessor;
            this.generalSettingProcessor = generalSettingProcessor;
            this.currencyProcessor = currencyProcessor;
            this.categoryProcessor = categoryProcessor;
            this.sectionProcessor = sectionProcessor;
            this.customerProcessor = customerProcessor;
            this.juridicalPersonalityProcessor = juridicalPersonalityProcessor;
            this.collationSettingProcessor = collationSettingProcessor;
            this.columnNameSettingProcessor = columnNameSettingProcessor;
            this.importDataProcessor = importDataProcessor;
            this.receiptProcessor = receiptProcessor;
            this.serializer = MessagePackSerializer.Get<ReceiptInput>(new SerializationContext { DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native });
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

            var importer = new ReceiptImporterBase(applicationControl) {
                CompanyId           = source.CompanyId,
                CompanyCode         = company.Code,
                LoginUserId         = source.LoginUserId,
                ImporterSettingId   = source.ImporterSettingId,
                FilePath            = csv,
                CsvParser           = new CsvParser {
                    Encoding            = encoding,
                    StreamCreator       = new PlainTextMemoryStreamCreator(),
                },
                GetImporterSettingAsync = async settingId => (await importerSettingProcessor.GetAsync(new ImporterSetting { Id = settingId }, token)).FirstOrDefault(),
                GetImporterSettingDetailByIdAsync = async settingId => (await importerSettingDetailProcessor.GetAsync(new ImporterSetting { Id = settingId }, token)).ToList(),
                GetGeneralSettingValueAsync = async (companyId, code) => (await generalSettingProcessor.GetAsync(new GeneralSetting { CompanyId = companyId, Code = code, }, token)).FirstOrDefault()?.Value,
                ReceiptImportDuplicationCheckAsync = async (companyid, items, details) => (await receiptProcessor.ReceiptImportDuplicationCheckAsync(companyid, items, details, token)).ToArray(),

                GetCurrenciesAsync = async (companyId, codes) => (await currencyProcessor.GetAsync(new CurrencySearch { CompanyId = companyId, Codes = codes, }, token)).ToList(),
                GetCategoriesByCodesAsync = async (companyId, categoryType, codes) => (await categoryProcessor.GetAsync(new CategorySearch { CompanyId = companyId, CategoryType = categoryType, Codes = codes, }, token)).ToList(),
                GetSectionByCodesAsync = async (companyId, codes) => (await sectionProcessor.GetAsync(new SectionSearch { CompanyId = companyId, Codes = codes, }, token)).ToList(),
                GetCustomerByCodesAsync = async (companyId, codes) => (await customerProcessor.GetAsync(new CustomerSearch { CompanyId = companyId, Codes = codes, }, token)).ToList(),
                GetLegalPersonaritiesAsync = async (companyId) => (await juridicalPersonalityProcessor.GetAsync(new JuridicalPersonality { CompanyId = companyId, }, token)).Select(x => x.Kana).ToArray(),
                GetCollationSettingAsync = companyId => collationSettingProcessor.GetAsync(companyId, token),
                LoadColumnNameSettingsInnerAsync = async tableName => (await columnNameSettingProcessor.GetAsync(new ColumnNameSetting { CompanyId = source.CompanyId, TableName = tableName, }, token)).ToArray(),

                Serialize           = item => serializer.PackSingleObject(item),
                SaveImportDataAsync = data => importDataProcessor.SaveAsync(data, token),
            };

            var readResult = await importer.ReadCsvAsync();
            return new ImportDataResult {
                ImportData      = importer.ImportData,
                ReadCount       = importer.ReadCount,
                ValidCount      = importer.ValidCount,
                InvalidCount    = importer.InvalidCount,
                Logs            = importer.GetValidationLogs(),
            };
        }

        /// <summary>正常な入金データの取得</summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReceiptInput>> GetValidItemsAsync(long id, CancellationToken token = default(CancellationToken))
        {
            var data = await importDataProcessor.GetAsync(id, objectType: 0, token: token);
            return data.Details.Select(x => serializer.UnpackSingleObject(x.RecordItem)).ToArray();
        }

        /// <summary>不正なデータの取得</summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReceiptInput>> GetInvalidItemsAsync(long id, CancellationToken token = default(CancellationToken))
        {
            var data = await importDataProcessor.GetAsync(id, objectType: 1, token: token);
            return data.Details.Select(x => serializer.UnpackSingleObject(x.RecordItem)).ToArray();

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

            var importer = new ReceiptImporterBase(applicationControl) {
                CompanyId           = source.CompanyId,
                CompanyCode         = company.Code,
                LoginUserId         = source.LoginUserId,
                ImporterSettingId   = source.ImporterSettingId,
                Deserialize         = bytes => serializer.UnpackSingleObject(bytes),
                LoadImportDataAsync = () => importDataProcessor.GetAsync(importDataId, objectType: 0, token: token),
                SaveInnerAsync = items => receiptProcessor.SaveAsync(new ReceiptSaveItem { Receipts = items, }, token),
            };

            var result = await importer.ImportAsync();
            return new ImportDataResult {
                SaveCount       = importer.SaveCount,
                SaveAmount      = importer.SaveAmount,
            };
        }

    }
}