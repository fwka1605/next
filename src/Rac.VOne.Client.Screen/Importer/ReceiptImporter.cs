using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Importers;
using System.Linq;


namespace Rac.VOne.Client.Screen.Importer
{
    public class ReceiptImporter : ReceiptImporterBase
    {
        public ILogin Login { get; set; }
        private string SessionKey => Login?.SessionKey;

        public ReceiptImporter(ILogin login, ApplicationControl applicationControl)
            : base(applicationControl)
        {
            Login       = login;
            CompanyId   = login.CompanyId;
            CompanyCode = login.CompanyCode;
            LoginUserId = login.UserId;
            Initialize();
        }

        private void Initialize()
        {
            GetImporterSettingAsync = settingId => Util.GetImporterSettingAsync(Login, (int)Rac.VOne.Common.Constants.FreeImporterFormatType.Receipt, settingId);
            GetImporterSettingDetailByIdAsync = settingId => Util.GetImporterSettingDetailByIdAsync(Login, settingId);
            GetGeneralSettingValueAsync = async (companyId, code) => (await Util.GetGeneralSettingAsync(Login, code))?.Value;

            ReceiptImportDuplicationCheckAsync = async (companyId, items, details) => await ServiceProxyFactory.DoAsync(async (ReceiptServiceClient client) => (await client.ReceiptImportDuplicationCheckAsync(SessionKey, CompanyId, items, details)).RowNumbers);
            SaveInnerAsync = items => ServiceProxyFactory.DoAsync((ReceiptServiceClient client) => client.SaveAsync(SessionKey, items, null, 0));

            GetCurrenciesAsync = (companyId, codes) => Util.GetCurrenciesAsync(Login, codes);
            GetCategoriesByCodesAsync = (companyId, categoryType, codes) => Util.GetCategoriesByCodesAsync(Login, categoryType, codes);
            GetSectionByCodesAsync = (companyId, codes) => Util.GetSectionByCodesAsync(Login, codes);
            GetCustomerByCodesAsync = (companyId, codes) => Util.GetCustomerByCodesAsync(Login, codes);
            GetLegalPersonaritiesAsync = companyId => Util.GetLegalPersonaritiesAsync(Login);
            GetCollationSettingAsync = companyId => Util.GetCollationSettingAsync(Login);

            LoadColumnNameSettingsInnerAsync = tableName => ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result) return result.ColumnNames.Where(x => x.TableName == tableName).ToArray();
                return new ColumnNameSetting[] { };
            });
        }
    }
}
