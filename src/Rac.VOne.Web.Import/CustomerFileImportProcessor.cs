using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Import;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Import
{
    /// <summary>得意先マスター インポート処理</summary>
    public class CustomerFileImportProcessor : ICustomerFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly IStaffProcessor staffProcessor;
        private readonly IJuridicalPersonalityProcessor juridicalPersonalityProcessor;
        private readonly IImporterSettingProcessor importerSettingProcessor;
        private readonly IImporterSettingDetailProcessor importerSettingDetailProcessor;
        private readonly IGeneralSettingProcessor generalSettingProcessor;
        private readonly ICustomerProcessor customerProcessor;

        /// <summary>constructor</summary>
        public CustomerFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ICategoryProcessor categoryProcessor,
            IStaffProcessor staffProcessor,
            IJuridicalPersonalityProcessor juridicalPersonalityProcessor,
            IImporterSettingProcessor importerSettingProcessor,
            IImporterSettingDetailProcessor importerSettingDetailProcessor,
            IGeneralSettingProcessor generalSettingProcessor,
            ICustomerProcessor customerProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.categoryProcessor = categoryProcessor;
            this.staffProcessor = staffProcessor;
            this.juridicalPersonalityProcessor = juridicalPersonalityProcessor;
            this.importerSettingProcessor = importerSettingProcessor;
            this.importerSettingDetailProcessor = importerSettingDetailProcessor;
            this.generalSettingProcessor = generalSettingProcessor;
            this.customerProcessor = customerProcessor;
        }

        /// <summary>インポート処理</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ImportResult> ImportAsync(MasterImportSource source, CancellationToken token = default(CancellationToken))
        {
            var mode = (ImportMethod)source.ImportMethod;
            var encoding = Encoding.GetEncoding(source.EncodingCodePage);
            var csv = encoding.GetString(source.Data);

            var companyTask = companyProcessor.GetAsync(new CompanySearch { Id = source.CompanyId, }, token);
            var loginUsersTask = loginUserProcessor.GetAsync(new LoginUserSearch { CompanyId = source.CompanyId, }, token);
            var appConTask = applicationControlProcessor.GetAsync(source.CompanyId, token);

            await Task.WhenAll(companyTask, loginUsersTask, appConTask);

            var company = companyTask.Result.First();
            var loginUserDictionary = loginUsersTask.Result.ToDictionary(x => x.Code);
            var loginUser = loginUsersTask.Result.First(x => x.Id == source.LoginUserId);
            var appCon = appConTask.Result;

            var importer = new Rac.VOne.Web.Models.Importers.CustomerImporterBase(appCon)
            {
                CompanyId           = source.CompanyId,
                CompanyCode         = company.Code,
                LoginUserId         = source.LoginUserId,
                PatternNo           = source.ImporterSettingCode,
                Parser              = new CsvParser {
                    Encoding        = encoding,
                    StreamCreator   = new PlainTextMemoryStreamCreator(),
                },
                GetCollectCategoryAsync = async () => (await categoryProcessor.GetAsync(new CategorySearch { CompanyId = source.CompanyId, CategoryType = CategoryType.Collect, }, token)).ToList(),
                GetStaffAsync = async () => (await staffProcessor.GetAsync(new StaffSearch { CompanyId = source.CompanyId, }, token)).ToList(),
                GetLeagalPersonaritiesAsync = async () => (await juridicalPersonalityProcessor.GetAsync(new JuridicalPersonality { CompanyId = source.CompanyId, }, token)).Select(x => x.Kana).ToArray(),
                GetCustomerAsync = async () => (await customerProcessor.GetAsync(new CustomerSearch { CompanyId = source.CompanyId, }, token)).ToList(),
                GetImporterSettingAsync = async (formatId, code) => (await importerSettingProcessor.GetAsync(new ImporterSetting { CompanyId = source.CompanyId, FormatId = formatId, Code = code, }, token)).First(),
                GetImporterSettingDetailAsync = async (formatId, code) => (await importerSettingDetailProcessor.GetAsync(new ImporterSetting { CompanyId = source.CompanyId, FormatId = formatId, Code = code, }, token)).ToList(),
                GetRoundingTypeAsync = async () =>
                {
                    var val = (await generalSettingProcessor.GetAsync(new GeneralSetting { CompanyId = source.CompanyId, Code = "取込時端数処理", }, token)).FirstOrDefault()?.Value;
                    RoundingType type = RoundingType.Floor;
                    Enum.TryParse(val, out type);
                    return type;
                },
                GetMasterDataForCustomerGroupParentAsync = async (codes) => (await customerProcessor.GetImportForCustomerGroupParentAsync(source.CompanyId, codes, token)).ToList(),
                GetMasterDataForCustomerGroupChildAsync = async (codes) => (await customerProcessor.GetImportForCustomerGroupChildAsync(source.CompanyId, codes, token)).ToList(),
                GetMasterDataForKanaHistoryAsync = async (codes) => (await customerProcessor.GetImportForKanaHistoryAsync(source.CompanyId, codes, token)).ToList(),
                GetMasterDataForBillingAsync = async (codes) => (await customerProcessor.GetImportForBillingAsync(source.CompanyId, codes, token)).ToList(),
                GetMasterDataForReceiptAsync = async (codes) => (await customerProcessor.GetImportForReceiptAsync(source.CompanyId, codes, token)).ToList(),
                GetMasterDataForNettingAsync = async (codes) => (await customerProcessor.GetImportForNettingAsync(source.CompanyId, codes, token)).ToList(),
                ImportCustomerAsync = (insert, update, delete) => customerProcessor.ImportAsync(insert, update, delete, token),
            };
            await importer.InitializeAsync();
            var result = await importer.ImportAsync(csv, source.ImportMethod, null);
            result.Logs = importer.ErrorList;

            return result;
        }
    }
}