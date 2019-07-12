using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.MFModels;
using Rac.VOne.Web.Models.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Rac.VOne.Web.Common.ThirdPartyApis
{
    /// <summary>MFC請求書 Web API利用の wrapper</summary>
    /// <remarks>angular から third-party api を 利用する場合 proxy 作成する？</remarks>
    public class MFWebApiProcessor : IMFWebApiProcessor
    {
        private readonly IWebApiSettingQueryProcessor webApiSettingQueryProcessor;
        private readonly IAddWebApiSettingQueryProcessor addWebApiSettingQueryProcessor;

        private readonly MFWebApiClient client;
        private const int ApiTypeId = WebApiType.MoneyForward;

        /// <summary>constructor</summary>
        public MFWebApiProcessor(
            IWebApiSettingQueryProcessor webApiSettingQueryProcessor,
            IAddWebApiSettingQueryProcessor addWebApiSettingQueryProcessor
            )
        {
            this.webApiSettingQueryProcessor = webApiSettingQueryProcessor;
            this.addWebApiSettingQueryProcessor = addWebApiSettingQueryProcessor;
            client = new MFWebApiClient();
        }

        private async Task<WebApiSetting> LoadWebApiSettingAsync(MFWebApiOption option, CancellationToken token)
        {
            var setting = (await webApiSettingQueryProcessor.GetByIdAsync(option.CompanyId, ApiTypeId, token)) ?? new WebApiSetting
            {
                CompanyId       = option.CompanyId,
                ApiTypeId       = ApiTypeId,
                ApiVersion      = option.ApiVersion,
                ClientId        = option.ClientId,
                ClientSecret    = option.ClientSecret,
                ExtractSetting  = option.ExtractSetting,
                CreateBy        = option.LoginUserId,
                UpdateBy        = option.LoginUserId,
            };
            if (!string.IsNullOrEmpty(option.ClientId       )) setting.ClientId         = option.ClientId;
            if (!string.IsNullOrEmpty(option.ClientSecret   )) setting.ClientSecret     = option.ClientSecret;
            if (!string.IsNullOrEmpty(option.ExtractSetting )) setting.ExtractSetting   = option.ExtractSetting;
            return setting;
        }

        private async Task<int> SaveSettingInnerAsync(MFWebApiClient client, MFWebApiOption option, CancellationToken token)
        {
            var result = 0;
            if (client.TokenRefreshed)
            {
                var setting = client.WebApiSetting;
                setting.UpdateBy = option.LoginUserId;
                result = await addWebApiSettingQueryProcessor.SaveAsync(setting, token);

                client.TokenRefreshed = false;
            }
            return result;
        }

        /// <summary>登録・認証処理</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns>0:成功, -1:認証失敗, -2:登録失敗</returns>
        public async Task<int> SaveSettingAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken))
        {
            const int Success = 0;
            const int AuthenticationFailure = -1;
            const int SaveFailure = -2;

            var setting = await LoadWebApiSettingAsync(option, token);
            client.AuthorizationCode    = option.AuthorizationCode;
            client.WebApiSetting        = setting;

            var authResult = false;
            if (!string.IsNullOrEmpty(option.AuthorizationCode))
            {
                authResult = await client.AuthorizeAsync();
            }
            else
            {
                authResult = await client.ValidateToken();
            }
            if (!authResult) return AuthenticationFailure;

            setting = client.WebApiSetting;
            var saveResult = await addWebApiSettingQueryProcessor.SaveAsync(setting, token);
            if (saveResult != 1)
            {
                return SaveFailure;
            }

            return Success;
        }

        /// <summary>請求データ取得処理</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<billing>> GetBillingsAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken))
        {
            var setting = await LoadWebApiSettingAsync(option, token);
            client.WebApiSetting = setting;
            var billings = (await client.GetBillingAsync(option.BillingDateFrom, option.BillingDateTo)).ToArray();

            await SaveSettingInnerAsync(client, option, token);

            return billings;
        }

        /// <summary>事業所取得処理</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Office> GetOfficesAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken))
        {
            var setting = await LoadWebApiSettingAsync(option, token);
            client.WebApiSetting = setting;

            var office = await client.GetOfficeAsync();

            await SaveSettingInnerAsync(client, option, token);

            return office;
        }


        /// <summary>MFC請求書 入金ステータスの更新処理</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> PatchBillings(MFWebApiOption option, CancellationToken token = default(CancellationToken))
        {
            var setting = await LoadWebApiSettingAsync(option, token);
            client.WebApiSetting = setting;

            var result = await client.PatchPaymentAsync(option.MFIds, option.IsMatched);

            await SaveSettingInnerAsync(client, option, token);

            return result;
        }

        /// <summary>access_token/refresh_token の有効性を確認</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ValidateToken(MFWebApiOption option, CancellationToken token = default(CancellationToken))
        {
            var setting = await LoadWebApiSettingAsync(option, token);
            client.WebApiSetting = setting;
            var valid = await client.ValidateToken();

            await SaveSettingInnerAsync(client, option, token);

            return valid;
        }

        /// <summary>取引先情報取得処理</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<partner> GetPartnersAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken))
        {
            var setting = await LoadWebApiSettingAsync(option, token);
            client.WebApiSetting = setting;

            var partner = await client.GetPartnersAsync(option.PartnerId);

            await SaveSettingInnerAsync(client, option, token);

            return partner;
        }
    }
}