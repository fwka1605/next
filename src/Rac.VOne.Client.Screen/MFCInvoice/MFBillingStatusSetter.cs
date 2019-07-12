using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.WebApiSettingMasterService;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen.MFCInvoice
{
    public class MFBillingStatusSetter : IDisposable
    {
        private readonly MFWebApiClient client = new MFWebApiClient();
        public ILogin Login { get; set; }
        /// <summary>消込済を連携するか否か
        /// 一括消込/個別 消込 → IsMatched = true
        /// 一括消込  消込解除 → IsMatched = false
        /// 個別消込画面などの IsMatched の フラグと反転しているので注意
        /// </summary>
        public bool IsMatched { get; set; }
        public IWin32Window OwnerForm { get; set; }
        public WebApiSetting WebApiSetting
        {
            set { client.WebApiSetting = value; }
        }

        public void Dispose()
        {
            client?.Dispose();
        }

        public async Task<bool> InitializeAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(0));
            return true;
        }

        public bool Update(IEnumerable<ITransactionData> transactions)
        {
            var billingIds = transactions.Select(x => (x as Matching).BillingId).Distinct().ToArray();
            var getTask = GetMFBillings(billingIds);
            ProgressDialog.Start(OwnerForm, getTask, false, Login.SessionKey);
            var billings = getTask.Result;
            // mfc invoice ids
            var ids = billings.Select(x => x.Id).Distinct().ToArray();

            if (!ids.Any()) return true;

            var patchTask = client.PatchPaymentAsync(ids, IsMatched);
            ProgressDialog.Start(OwnerForm, patchTask, false, Login.SessionKey);

            var result = patchTask.Result;

            if (client.TokenRefreshed)
            {
                var setting = client.WebApiSetting;
                var saveTask = SaveSettingAsync(setting);
                ProgressDialog.Start(OwnerForm, saveTask, false, Login.SessionKey);
            }

            return result;
        }

        private async Task SaveSettingAsync(WebApiSetting setting)
            => await ServiceProxyFactory.DoAsync(async (WebApiSettingMasterClient client)
                    => await client.SaveAsync(Login.SessionKey, setting));

        private async Task<IEnumerable<MFBilling>> GetMFBillings(IEnumerable<long> ids)
            => await ServiceProxyFactory.DoAsync(async (MFBillingService.MFBillingServiceClient client) => {
                var result = await client.GetByBillingIdsAsync(Login.SessionKey, ids.ToArray(), IsMatched);
                if (result.ProcessResult.Result)
                    return result.MFBillings;
                return Enumerable.Empty<MFBilling>();
            });


    }
}
