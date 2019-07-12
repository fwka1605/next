using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.ImporterSettingService;
using Rac.VOne.Client.Screen.JuridicalPersonalityMasterService;
using Rac.VOne.Client.Screen.StaffMasterService;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Importers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Screen.Importer
{
    public class CustomerImporter : CustomerImporterBase
    {
        private ILogin Login { get; set; }
        private string SessionKey { get { return Login?.SessionKey; } }

        public CustomerImporter(ILogin login, ApplicationControl appControl) : base(appControl)
        {
            Login       = login;
            CompanyId   = login.CompanyId;
            CompanyCode = login.CompanyCode;
            LoginUserId = login.UserId;
            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            OutputErrorLog = (path, errorInfo, sourceFilePath) => {
                var exists = File.Exists(path);
                using (var stream = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (var writer = new StreamWriter(stream, Encoding.GetEncoding(932)))
                {
                    if (exists) writer.WriteLine();
                    var now = DateTime.Now;
                    writer.WriteLine($"{now:yyyy年MM月dd日 HH時mm分ss秒}");
                    writer.WriteLine($"得意先データ：{Path.GetFileName(sourceFilePath)}");
                    foreach (var error in errorInfo)
                        writer.WriteLine(error);
                }
            };

            ImportCustomerAsync = async (InsertList, UpdateList, DeleteList) => {
                ImportResult result = null;
                try
                {
                    await ServiceProxyFactory.DoAsync<CustomerMasterClient>(async client
                        => result = await client.ImportAsync(SessionKey,
                            InsertList.ToArray(), UpdateList.ToArray(), DeleteList.ToArray()));
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }
                return result ?? new ImportResult();
            };

            GetCollectCategoryAsync = async () => await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey,
                    new CategorySearch
                    {
                        CompanyId = CompanyId,
                        CategoryType = CollectCategoryType,
                    });
                if (result.ProcessResult.Result)
                    return result.Categories;
                return new List<Category>();
            });

            GetStaffAsync = async () => await ServiceProxyFactory.DoAsync(async (StaffMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, new StaffSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    return result.Staffs;
                return new List<Staff>();
            });

            GetCustomerAsync = async () => await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId, new CustomerSearch { CompanyId = CompanyId });
                if (result.ProcessResult.Result)
                    return result.Customers;
                return new List<Customer>();
            });

            GetLeagalPersonaritiesAsync = async () => await ServiceProxyFactory.DoAsync(async (JuridicalPersonalityMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    return result.JuridicalPersonalities.Select(x => x.Kana);
                return Enumerable.Empty<string>();
            });

            GetImporterSettingAsync = async (int formatId, string code) => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client) => {
                var result = await client.GetHeaderByCodeAsync(SessionKey, CompanyId, formatId, code);
                if (result.ProcessResult.Result)
                    return result.ImporterSetting;
                return null;
            });

            GetImporterSettingDetailAsync = async (int formatId, string code) => await ServiceProxyFactory.DoAsync(async (ImporterSettingServiceClient client) => {
                var result = await client.GetDetailByCodeAsync(SessionKey, CompanyId, formatId, code);
                if (result.ProcessResult.Result)
                    return result.ImporterSettingDetails;
                return new List<ImporterSettingDetail>();
            });

            GetRoundingTypeAsync = async () => {
                var res = await Util.GetGeneralSettingAsync(Login, "取込時端数処理");
                if (!Enum.TryParse(res.Value, out roundingType))
                {
                    throw new Exception("取込時端数処理");
                }
                return roundingType;
            };


            GetMasterDataForCustomerGroupParentAsync = async (string[] codes)
                => await GetMasterDataAsync(async client
                    => await client.GetImportItemsForCustomerGroupParentAsync(SessionKey, CompanyId, codes));

            GetMasterDataForCustomerGroupChildAsync = async (string[] codes)
                => await GetMasterDataAsync(async client
                    => await client.GetImportItemsForCustomerGroupChildAsync(SessionKey, CompanyId, codes));

            GetMasterDataForKanaHistoryAsync = async (string[] codes)
                => await GetMasterDataAsync(async client
                    => await client.GetImportItemsForKanaHistoryAsync(SessionKey, CompanyId, codes));

            GetMasterDataForBillingAsync = async (string[] codes)
                => await GetMasterDataAsync(async client
                    => await client.GetImportItemsForBillingAsync(SessionKey, CompanyId, codes));

            GetMasterDataForReceiptAsync = async (string[] codes)
                => await GetMasterDataAsync(async client
                    => await client.GetImportItemsForReceiptAsync(SessionKey, CompanyId, codes));

            GetMasterDataForNettingAsync = async (string[] codes)
                => await GetMasterDataAsync(async client
                    => await client.GetImportItemsForNettingAsync(SessionKey, CompanyId, codes));

            LogError = ex => NLogHandler.WriteErrorLog(this, ex, SessionKey);
        }

        #region call web service

        private async Task<List<MasterData>> GetMasterDataAsync(Func<CustomerMasterClient, Task<MasterDatasResult>> getter)
            => await ServiceProxyFactory.DoAsync(async (CustomerMasterClient client) =>
            {
                var result = await getter(client);
                if (result.ProcessResult.Result)
                    return result.MasterDatas;
                return new List<MasterData>();
            });

        #endregion

    }
}
