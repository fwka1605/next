using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Client;
using Rac.VOne.Web.Models;
using Rac.VOne.EbData.ApplicationControlMasterService;
using Rac.VOne.EbData.BankAccountMasterService;
using Rac.VOne.EbData.CategoryMasterService;
using Rac.VOne.EbData.CollationSettingMasterService;
using Rac.VOne.EbData.CurrencyMasterService;
using Rac.VOne.EbData.CustomerMasterService;
using Rac.VOne.EbData.EBExcludeAccountSettingMasterService;
using Rac.VOne.EbData.IgnoreKanaMasterService;
using Rac.VOne.EbData.JuridicalPersonalityMasterService;
using Rac.VOne.EbData.SectionMasterService;
using Rac.VOne.EbData.ImportFileLogService;
using Rac.VOne.Client.Common;
using Rac.VOne.Common;

namespace Rac.VOne.EbData
{
    public partial class EbDataImporter : EbDataImporterBase
    {

        private ILogin login;
        public ILogin Login
        {
            get { return login; }
            set {
                Helper.CompanyId    = value.CompanyId;
                Helper.LoginUserId  = value.UserId;
                login = value;

            }
        }

        private string SessionKey => login?.SessionKey;


        public EbDataImporter() : base() {

            Helper.Initialize = () => {
                ServiceProxyFactory.Do((ApplicationControlMasterClient client) => {
                    var result = client.Get(SessionKey, Login.CompanyId);
                    if (result.ProcessResult.Result)
                        Helper.ApplicationControl = result.ApplicationControl;
                });
                ServiceProxyFactory.Do((CollationSettingMasterClient client) => {
                    var result = client.Get(SessionKey, Login.CompanyId);
                    if (result.ProcessResult.Result)
                        Helper.CollationSetting = result.CollationSetting;
                });
                ServiceProxyFactory.Do((CategoryMasterClient client) => {
                    var result = client.GetByCode(SessionKey, Login.CompanyId, CategoryType.Receipt, new[] { "01" });
                    if (result.ProcessResult.Result)
                        Helper.DefaultReceiptCategory = result.Categories.First();
                });
                ServiceProxyFactory.Do((CurrencyMasterClient client) => {
                    var result = client.GetByCode(SessionKey, Login.CompanyId, new[] { Constants.DefaultCurrencyCode });
                    if (result.ProcessResult.Result)
                        Helper.DefaultCurrency = result.Currencies.First();
                });
                ServiceProxyFactory.Do((JuridicalPersonalityMasterClient client) => {
                    var result = client.GetItems(SessionKey, Login.CompanyId);
                    if (result.ProcessResult.Result)
                        Helper.LegalPersonalities = result.JuridicalPersonalities.Select(x => x.Kana).ToArray();
                    else
                        Helper.LegalPersonalities = new string[] { };
                });
            };

            Helper.InitializeAsync = async token => {
                var applicationControlTask = ServiceProxyFactory.DoAsync(async (ApplicationControlMasterClient client)
                    => (await client.GetAsync(SessionKey, Login.CompanyId)).ApplicationControl);

                var collationSettingTask = ServiceProxyFactory.DoAsync(async (CollationSettingMasterClient client)
                    => (await client.GetAsync(SessionKey, Login.CompanyId)).CollationSetting);

                var defaultCurrencyTask = ServiceProxyFactory.DoAsync(async (CurrencyMasterClient client)
                    => (await client.GetByCodeAsync(SessionKey, Login.CompanyId, new[] { Constants.DefaultCurrencyCode })).Currencies.First());

                var defaultReceiptCategoryTask = ServiceProxyFactory.DoAsync(async (CategoryMasterClient client)
                    => (await client.GetByCodeAsync(SessionKey, Login.CompanyId, CategoryType.Receipt, new[] { "01" })).Categories.First());

                var juridicalPersonalityTask = ServiceProxyFactory.DoAsync(async (JuridicalPersonalityMasterClient client)
                    => (await client.GetItemsAsync(SessionKey, Login.CompanyId))?.JuridicalPersonalities.Select(x => x.Kana).ToArray() ?? new string[] { });

                await Task.WhenAll(
                    applicationControlTask,
                    collationSettingTask,
                    defaultCurrencyTask,
                    defaultReceiptCategoryTask,
                    juridicalPersonalityTask);

                Helper.ApplicationControl = applicationControlTask.Result;
                Helper.CollationSetting = collationSettingTask.Result;
                Helper.DefaultCurrency = defaultCurrencyTask.Result;
                Helper.DefaultReceiptCategory = defaultReceiptCategoryTask.Result;
                Helper.LegalPersonalities = juridicalPersonalityTask.Result;
            };

            Helper.GetBankAccount = (bankCode, branchCode, accountTypeId, accountNumber)
                => ServiceProxyFactory.Do((BankAccountMasterClient client)
                => client.GetByCode(SessionKey, Login.CompanyId, bankCode, branchCode, accountTypeId, accountNumber).BankAccount);

            Helper.GetBankAccountAsync = (bankCode, branchCode, accountTypeId, accountNumber, token)
                => ServiceProxyFactory.DoAsync(async (BankAccountMasterClient client)
                => (await client.GetByCodeAsync(SessionKey, Login.CompanyId, bankCode, branchCode, accountTypeId, accountNumber)).BankAccount);

            Helper.GetBankAccountByBankName = (bankName, branchName, accountTypeId, accountNumber)
                => ServiceProxyFactory.Do((BankAccountMasterClient client) => {
                    var result = client.GetItems(SessionKey, Login.CompanyId, new BankAccountSearch {
                        CompanyId       = Login.CompanyId,
                        BankName        = bankName,
                        BranchName      = branchName,
                        AccountTypeId   = accountTypeId,
                        AccountNumber   = accountNumber,
                    });
                    return result.BankAccounts?.FirstOrDefault();
                });

            Helper.GetBankAccountByBankNameAsync = (bankName, branchName, accountTypeId, accountNumber, token)
                => ServiceProxyFactory.DoAsync(async (BankAccountMasterClient client) => {
                    var result = await client.GetItemsAsync(SessionKey, Login.CompanyId, new BankAccountSearch {
                        CompanyId       = Login.CompanyId,
                        BankName        = bankName,
                        BranchName      = branchName,
                        AccountTypeId   = accountTypeId,
                        AccountNumber   = accountNumber,
                    });
                    return result.BankAccounts?.FirstOrDefault();
                });

            Helper.GetBankAccountByBranchName = (bankCode, branchName)
                => ServiceProxyFactory.Do((BankAccountMasterClient client) => {
                    var result = client.GetItems(SessionKey, Login.CompanyId, new BankAccountSearch {
                        CompanyId       = Login.CompanyId,
                        BankCodes       = new[] { bankCode },
                        BranchName      = branchName,
                    });
                    return result.BankAccounts?.FirstOrDefault();
                });

            Helper.GetBankAccountByBranchNameAsync = (bankCode, branchName, token)
                => ServiceProxyFactory.DoAsync(async (BankAccountMasterClient client) => {
                    var result = await client.GetItemsAsync(SessionKey, Login.CompanyId, new BankAccountSearch {
                        CompanyId       = Login.CompanyId,
                        BankCodes       = new[] { bankCode },
                        BranchName      = branchName,
                    });
                    return result.BankAccounts?.FirstOrDefault();
                });

            Helper.GetBankAccountByBranchNameAndNumber = (bankCode, branchName, accountTypeId, accountNumber)
                => ServiceProxyFactory.Do((BankAccountMasterClient client)
                => client.GetByBranchName(SessionKey, Login.CompanyId, bankCode, branchName, accountTypeId, accountNumber).BankAccount);

            Helper.GetBankAccountByBranchNameAndNumberAsync = (bankCode, branchName, accountTypeId, accountNumber, token)
                => ServiceProxyFactory.DoAsync(async (BankAccountMasterClient client)
                => (await client.GetByBranchNameAsync(SessionKey, Login.CompanyId, bankCode, branchName, accountTypeId, accountNumber)).BankAccount);


            Helper.GetCustomerIdByExclusiveInfo = (bankCode, branchCode, payerCode)
                => ServiceProxyFactory.Do((CustomerMasterClient client) => {
                    var result = client.GetTopCustomer(SessionKey, new Customer {
                        CompanyId               = Login.CompanyId,
                        ExclusiveBankCode       = bankCode,
                        ExclusiveBranchCode     = branchCode,
                        ExclusiveAccountNumber  = payerCode,
                    });
                    return result.Customer?.Id;
                });

            Helper.GetCustomerIdByExclusiveInfoAsync = (bankCode, branchCode, payerCode, token)
                => ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) => {
                    var result = await client.GetTopCustomerAsync(SessionKey, new Customer {
                        CompanyId               = Login.CompanyId,
                        ExclusiveBankCode       = bankCode,
                        ExclusiveBranchCode     = branchCode,
                        ExclusiveAccountNumber  = payerCode,
                    });
                    return result.Customer?.Id;
                });

            Helper.GetSectionIdByPayerCode = payerCode
                => ServiceProxyFactory.Do((SectionMasterClient client) => {
                    var result = client.GetImportItemsForSection(SessionKey, Login.CompanyId, new[] { payerCode });
                    return result.Sections?.FirstOrDefault()?.Id;
                });

            Helper.GetSectionIdByPayerCodeAsync = (payerCode, token)
                => ServiceProxyFactory.DoAsync(async (SectionMasterClient client) => {
                    var result = await client.GetImportItemsForSectionAsync(SessionKey, Login.CompanyId, new[] { payerCode });
                    return result.Sections?.FirstOrDefault()?.Id;
                });

            Helper.GetExcludeCategoryId = payerName
                => ServiceProxyFactory.Do((IgnoreKanaMasterClient client) => {
                    var result = client.Get(SessionKey, Login.CompanyId, payerName);
                    return result?.IgnoreKana?.ExcludeCategoryId;
                });

            Helper.GetExcludeCategoryIdAsync = (payerName, token)
                => ServiceProxyFactory.DoAsync(async (IgnoreKanaMasterClient client) => {
                    var result = await client.GetAsync(SessionKey, Login.CompanyId, payerName);
                    return result?.IgnoreKana?.ExcludeCategoryId;
                });

            Helper.GetEBExcludeAccountSettingList = ()
                => ServiceProxyFactory.Do((EBExcludeAccountSettingMasterClient client) => {
                    var result = client.GetItems(SessionKey, Login.CompanyId);
                    return result?.EBExcludeAccountSettingList;
                });

            Helper.GetEBExcludeAccountSettingListAsync = token
                => ServiceProxyFactory.DoAsync(async (EBExcludeAccountSettingMasterClient client) => {
                    var result = await client.GetItemsAsync(SessionKey, Login.CompanyId);
                    return result?.EBExcludeAccountSettingList;
                });

            Helper.SaveDataInner = logs
                => ServiceProxyFactory.Do((ImportFileLogServiceClient client) => {
                    var result = client.SaveImportFileLog(SessionKey, logs.ToArray());
                    return result.ImportFileLogs;
                });

            Helper.SaveDataInnerAsync = (logs, token)
                => ServiceProxyFactory.DoAsync(async (ImportFileLogServiceClient client) => {
                    var result = await client.SaveImportFileLogAsync(SessionKey, logs.ToArray());
                    return result.ImportFileLogs;
                });

        }

    }
}
