using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Common;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common.Importer.PaymentSchedule;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Importers;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen.Importer
{
    public class PaymentScheduleImporter : PaymentScheduleImporterBase
    {
        public ILogin Login { get; set; }
        private string SessionKey => Login?.SessionKey;

        public PaymentScheduleImporter(ILogin login, ApplicationControl applicationControl)
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
            GetImporterSettingAsync = (settingId) => Util.GetImporterSettingAsync(Login, (int)FreeImporterFormatType.PaymentSchedule, settingId);
            GetImporterSettingDetailByIdAsync = (settingId) => Util.GetImporterSettingDetailByIdAsync(Login, settingId);
            GetGeneralSettingValueAsync = async (companId, code) => (await Util.GetGeneralSettingAsync(Login, code))?.Value;
            GetItemsForScheduledPaymentImportAsync = async (companyId, items, details)
                => await ServiceProxyFactory.DoAsync(async (BillingServiceClient client)
                => (await client.GetItemsForScheduledPaymentImportAsync(SessionKey, companyId, items, details))?.Billings ?? new List<Billing>());
            SaveInnerAsync = (companyId, loginUserId, settingId, items)
                => ServiceProxyFactory.DoAsync((BillingServiceClient client)
                => client.ImportScheduledPaymentAsync(SessionKey, companyId, loginUserId, settingId, items));
            GetCustomerByCodesAsync = (companyId, codes) => Util.GetCustomerByCodesAsync(Login, codes);
            GetDepartmentByCodesAsync = (companyId, codes) => Util.GetDepartmentByCodesAsync(Login, codes);
            GetAccountTitleByCodesAsync = (companyId, codes) => Util.GetAccountTitleByCodesAsync(Login, codes);
            GetCategoriesByCodesAsync = (companyId, categoryType, codes) => Util.GetCategoriesByCodesAsync(Login, categoryType, codes);
            GetCurrenciesAsync = (companyId, codes) => Util.GetCurrenciesAsync(Login, codes);
        }

    }
}
