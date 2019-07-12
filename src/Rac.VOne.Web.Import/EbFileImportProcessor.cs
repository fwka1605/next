using Rac.VOne.Common;
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

using Rac.VOne.EbData;

namespace Rac.VOne.Web.Import
{
    /// <summary>
    /// EBファイル インポート処理
    /// </summary>
    public class EbFileImportProcessor : IEbFileImportProcessor
    {
        private readonly ICompanyProcessor companyProcessor;
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly IApplicationControlProcessor applicationControlProcessor;
        private readonly ICollationSettingProcessor collationSettingProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly ICurrencyProcessor currencyProcessor;
        private readonly IJuridicalPersonalityProcessor juridicalPersonalityProcessor;
        private readonly IBankAccountProcessor bankAccountProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly IIgnoreKanaProcessor ignoreKanaProcessor;
        private readonly IEBExcludeAccountSettingProcessor ebExcludeAccountSettingProcessor;
        private readonly ISectionProcessor sectionProcessor;
        private readonly IImportFileLogProcessor importFileLogProcessor;

        private readonly EbDataImporterBase importer;


        /// <summary>constructor</summary>
        public EbFileImportProcessor(
            ICompanyProcessor companyProcessor,
            ILoginUserProcessor loginUserProcessor,
            IApplicationControlProcessor applicationControlProcessor,
            ICollationSettingProcessor collationSettingProcessor,
            ICategoryProcessor categoryProcessor,
            ICurrencyProcessor currencyProcessor,
            IJuridicalPersonalityProcessor juridicalPersonalityProcessor,
            IBankAccountProcessor bankAccountProcessor,
            ICustomerProcessor customerProcessor,
            IIgnoreKanaProcessor ignoreKanaProcessor,
            IEBExcludeAccountSettingProcessor ebExcludeAccountSettingProcessor,
            ISectionProcessor sectionProcessor,
            IImportFileLogProcessor importFileLogProcessor
            )
        {
            this.companyProcessor = companyProcessor;
            this.loginUserProcessor = loginUserProcessor;
            this.applicationControlProcessor = applicationControlProcessor;
            this.collationSettingProcessor = collationSettingProcessor;
            this.categoryProcessor = categoryProcessor;
            this.currencyProcessor = currencyProcessor;
            this.juridicalPersonalityProcessor = juridicalPersonalityProcessor;
            this.bankAccountProcessor = bankAccountProcessor;
            this.ignoreKanaProcessor = ignoreKanaProcessor;
            this.customerProcessor = customerProcessor;
            this.ebExcludeAccountSettingProcessor = ebExcludeAccountSettingProcessor;
            this.sectionProcessor = sectionProcessor;
            this.importFileLogProcessor = importFileLogProcessor;

            importer = new EbDataImporterBase {
                IsAsync         = true,
                IsPlainText     = true,
            };

            importer.Helper.InitializeAsync = async token => {
                var applicationControlTask = applicationControlProcessor.GetAsync(importer.Helper.CompanyId, token);
                var collationSettingTask = collationSettingProcessor.GetAsync(importer.Helper.CompanyId, token);
                var defaultCurrencyTask = currencyProcessor.GetAsync(new CurrencySearch {
                    CompanyId   = importer.Helper.CompanyId,
                    Codes       = new[] { Rac.VOne.Common.Constants.DefaultCurrencyCode },
                }, token);
                var defaultReceiptCategoryTask = categoryProcessor.GetAsync(new CategorySearch {
                    CompanyId       = importer.Helper.CompanyId,
                    CategoryType    = CategoryType.Receipt,
                    Codes           = new[] { "01" },
                }, token);
                var juridicalPersonalityTask = juridicalPersonalityProcessor.GetAsync(new JuridicalPersonality { CompanyId = importer.Helper.CompanyId }, token);

                await Task.WhenAll(
                    applicationControlTask,
                    collationSettingTask,
                    defaultCurrencyTask,
                    defaultReceiptCategoryTask,
                    juridicalPersonalityTask
                    );

                importer.Helper.ApplicationControl      = applicationControlTask.Result;
                importer.Helper.CollationSetting        = collationSettingTask.Result;
                importer.Helper.DefaultCurrency         = defaultCurrencyTask.Result.First();
                importer.Helper.DefaultReceiptCategory  = defaultReceiptCategoryTask.Result.First();
                importer.Helper.LegalPersonalities      = juridicalPersonalityTask.Result.Select(x => x.Kana).ToArray();
            };

            importer.Helper.GetBankAccountAsync = async (bankCode, branchCode, accountTypeId, accountNumber, token) => {
                var result = await bankAccountProcessor.GetAsync(new BankAccountSearch {
                    CompanyId       = importer.Helper.CompanyId,
                    BankCodes       = new[] { bankCode },
                    BranchCodes     = new[] { branchCode },
                    AccountTypeId   = accountTypeId,
                    AccountNumber   = accountNumber,
                }, token);
                return result.FirstOrDefault();
            };

            importer.Helper.GetBankAccountByBankNameAsync = async (bankName, branchName, accountTypeId, accountNumber, token) => {
                var result = await bankAccountProcessor.GetAsync(new BankAccountSearch {
                    CompanyId       = importer.Helper.CompanyId,
                    BankName        = bankName,
                    BranchName      = branchName,
                    AccountTypeId   = accountTypeId,
                    AccountNumber   = accountNumber,
                }, token);
                return result.FirstOrDefault();
            };

            importer.Helper.GetBankAccountByBranchNameAsync = async (bankCode, branchName, token) => {
                var result = await bankAccountProcessor.GetAsync(new BankAccountSearch {
                    CompanyId       = importer.Helper.CompanyId,
                    BankCodes       = new[] { bankCode },
                    BranchName      = branchName,
                }, token);
                return result.FirstOrDefault();
            };

            importer.Helper.GetBankAccountByBranchNameAndNumberAsync = async (bankCode, branchName, accountTypeId, accountNumber, token) => {
                var result = await bankAccountProcessor.GetAsync(new BankAccountSearch {
                    CompanyId       = importer.Helper.CompanyId,
                    BankCodes       = new[] { bankCode },
                    BranchName      = branchName,
                    AccountTypeId   = accountTypeId,
                    AccountNumber   = accountNumber,
                }, token);
                return result.FirstOrDefault();
            };

            importer.Helper.GetCustomerIdByExclusiveInfoAsync = async (bankCode, branchCode, payerCode, token) => {
                var result = await customerProcessor.GetAsync(new CustomerSearch {
                    CompanyId               = importer.Helper.CompanyId,
                    ExclusiveBankCode       = bankCode,
                    ExclusiveBranchCode     = branchCode,
                    ExclusiveAccountNumber  = payerCode,
                }, token);
                return result.FirstOrDefault()?.Id;
            };

            importer.Helper.GetSectionIdByPayerCodeAsync = async (payerCode, token) => {
                var result = await sectionProcessor.GetAsync(new SectionSearch {
                    CompanyId   = importer.Helper.CompanyId,
                    PayerCodes  = new[] { payerCode },
                }, token);
                return result.FirstOrDefault()?.Id;
            };

            importer.Helper.GetExcludeCategoryIdAsync = async (payerName, token) => {
                var result = await ignoreKanaProcessor.GetAsync(new IgnoreKana {
                    CompanyId   = importer.Helper.CompanyId,
                    Kana        = payerName,
                }, token);
                return result.FirstOrDefault()?.ExcludeCategoryId;
            };

            importer.Helper.GetEBExcludeAccountSettingListAsync = async token
                => (await ebExcludeAccountSettingProcessor.GetAsync(importer.Helper.CompanyId, token)).ToList();

            importer.Helper.SaveDataInnerAsync = async (logs, token)
                => (await importFileLogProcessor.SaveAsync(logs, token)).ToList();
        }

        /// <summary>インポート処理</summary>
        /// <param name="models"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EbFileInformation>> ImportAsync(IEnumerable<EbFileInformation> models, CancellationToken token = default(CancellationToken))
        {
            var first = models.FirstOrDefault();
            if (first == null) return new EbFileInformation[] { };

            importer.Helper.CompanyId   = first.CompanyId;
            importer.Helper.LoginUserId = first.LoginUserId;
            importer.Year               = first.Year;

            var files = models.Select((x, i) => new FileInformation(x, i)).ToList();

            var readResult = await importer.ReadFilesAsync(files, token);
            if (readResult.Any(x => x.Result != EbData.ImportResult.Success))
                return readResult.Select(x => x.ConvertToModel()).ToArray();

            var saveResult = await importer.SaveFilesAsync(readResult, token);

            return saveResult.Select(x => x.ConvertToModel()).ToArray();
        }
    }
}