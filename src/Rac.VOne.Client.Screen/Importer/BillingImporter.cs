using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Importers;
using System.Linq;

namespace Rac.VOne.Client.Screen.Importer
{
    public class BillingImporter : BillingImporterBase
    {
        public ILogin Login { get; set; }
        private string SessionKey => Login?.SessionKey;

        public BillingImporter(ILogin login, ApplicationControl applicationControl)
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
            GetImporterSettingAsync = (settingId) => Util.GetImporterSettingAsync(Login, (int)Constants.FreeImporterFormatType.Billing, settingId);
            GetImporterSettingDetailAsync = (settingId) => Util.GetImporterSettingDetailByIdAsync(Login, settingId);
            GetCurrencyAsync = (companyId) => Util.GetCurrenciesAsync(Login, null);
            GetJuridicalParsonalitiesAsync = (companyId) => Util.GetLegalPersonaritiesAsync(Login);
            GetGeneralSettingValueAsync = async (companyId, code) => (await Util.GetGeneralSettingAsync(Login, code))?.Value;
            GetTaxClassAsync = () => Util.GetTaxClassAsync(Login);
            GetBillingDivisionContractByCustomerIdsAsync = (ids) => Util.GetBillingDivisionContractByCustomerIdsAsync(Login, ids.ToArray());
            GetHolidayCalendarsAsync = companyId => Util.GetHolidayCalendarsAsync(Login);
            LoadColumnNameSettingsInnerAsync = tableName => ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result) return result.ColumnNames.Where(x => x.TableName == tableName).ToArray();
                return new ColumnNameSetting[] { };
            });
            BillingImportDuplicationCheckAsync = async (companyId, items, details) => await ServiceProxyFactory.DoAsync(async (BillingServiceClient client) => (await client.BillingImportDuplicationCheckAsync(SessionKey, companyId, items, details)).RowNumbers);
            ImportInnerAsync = (companyId, loginUserId, settingId, items) => ServiceProxyFactory.DoAsync((BillingServiceClient client) => client.ImportAsync(SessionKey, companyId, loginUserId, settingId, items));
            GetCustomerByCodesAsync = (companyId, codes) => Util.GetCustomerByCodesAsync(Login, codes);
            GetDepartmentByCodesAsync = (companyId, codes) => Util.GetDepartmentByCodesAsync(Login, codes);
            GetAccountTitleByCodesAsync = (companyId, codes) => Util.GetAccountTitleByCodesAsync(Login, codes);
            GetStaffByCodesAsync = (companyId, codes) => Util.GetStaffByCodesAsync(Login, codes);
            GetCategoriesByCodesAsync = (companyId, categoryType, codes) => Util.GetCategoriesByCodesAsync(Login, categoryType, codes);
            GetIsEnableToEditNoteAsync = (companyId) => Util.IsControlInputNoteAsync(Login);
        }

    }
}
