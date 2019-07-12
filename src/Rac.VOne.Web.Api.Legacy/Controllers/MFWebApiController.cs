using Rac.VOne.Web.Common.ThirdPartyApis;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;

using MFModels = Rac.VOne.Web.Models.MFModels;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>MFWeb API をコールする Controller</summary>
    public class MFWebApiController : ApiControllerAuthorized
    {
        private readonly IMFWebApiProcessor mfWebApiProcessor;

        /// <summary>constructor</summary>
        public MFWebApiController(
            IMFWebApiProcessor mfWebApiProcessor
            )
        {
            this.mfWebApiProcessor = mfWebApiProcessor;
        }

        /// <summary>MFC請求書 連携設定の登録</summary>
        /// <param name="option">
        /// <see cref="MFWebApiOption.CompanyId"/> 必須,
        /// <see cref="MFWebApiOption.LoginUserId"/> 必須,
        /// <see cref="MFWebApiOption.ClientId"/> 初回必須,
        /// <see cref="MFWebApiOption.ClientSecret"/> 初回必須,
        /// <see cref="MFWebApiOption.AuthorizationCode"/> 初回/再認証時必須,
        /// <see cref="MFWebApiOption.ExtractSetting"/> 初回必須, を指定
        /// </param>
        /// <param name="token"></param>
        /// <returns>0: 成功, -1:認証失敗, -2:登録失敗</returns>
        [HttpPost]
        public async Task<int> Save(MFWebApiOption option, CancellationToken token)
            => await mfWebApiProcessor.SaveSettingAsync(option, token);

        /// <summary>MFC請求書 請求データの取得</summary>
        /// <param name="option">
        /// <see cref="MFWebApiOption.CompanyId"/>,
        /// <see cref="MFWebApiOption.LoginUserId"/>,
        /// <see cref="MFWebApiOption.BillingDateFrom"/>,
        /// <see cref="MFWebApiOption.BillingDateTo"/> を指定
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MFModels.billing>> GetBillings(MFWebApiOption option, CancellationToken token)
            => (await mfWebApiProcessor.GetBillingsAsync(option, token)).ToArray();

        /// <summary>MFC請求書 事業所取得</summary>
        /// <param name="option">
        /// <see cref="MFWebApiOption.CompanyId"/>,
        /// <see cref="MFWebApiOption.LoginUserId"/> を指定
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MFModels.Office> GetOffice(MFWebApiOption option, CancellationToken token)
            => await mfWebApiProcessor.GetOfficesAsync(option, token);

        /// <summary>access_token/refresh_token の有効性検証
        /// 戻り値が false の場合は、再度認可処理が必要</summary>
        /// <param name="option"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ValidateToken(MFWebApiOption option, CancellationToken token)
            => await mfWebApiProcessor.ValidateToken(option, token);

        /// <summary>MFC請求書 入金ステータス更新</summary>
        /// <param name="option">
        /// <see cref="MFWebApiOption.CompanyId"/>,
        /// <see cref="MFWebApiOption.LoginUserId"/>,
        /// <see cref="MFWebApiOption.MFIds"/>,
        /// <see cref="MFWebApiOption.IsMatched"/> true:消込済, false: 未設定 を指定
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> PatchBillings(MFWebApiOption option, CancellationToken token)
            => await mfWebApiProcessor.PatchBillings(option, token);

        /// <summary>MFC請求書 取引先情報取得</summary>
        /// <param name="option">
        /// <see cref="MFWebApiOption.CompanyId"/>,
        /// <see cref="MFWebApiOption.LoginUserId"/>,
        /// <see cref="MFWebApiOption.PartnerId"/> を指定
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MFModels.partner> GetPartner(MFWebApiOption option, CancellationToken token)
            => await mfWebApiProcessor.GetPartnersAsync(option, token);
    }
}
