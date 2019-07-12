using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers.Master
{
    /// <summary>
    /// MF明細連携 マスター用コントローラー
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MfAggrMasterController : ControllerBase
    {
        private readonly IMfAggrTagProcessor mfAggrTagProcessor;
        private readonly IMfAggrAccountProcessor mfAggrAccountProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public MfAggrMasterController(
            IMfAggrTagProcessor mfAggrTagProcessor,
            IMfAggrAccountProcessor mfAggrAccountProcessor
            )
        {
            this.mfAggrTagProcessor = mfAggrTagProcessor;
            this.mfAggrAccountProcessor = mfAggrAccountProcessor;
        }


        /// <summary>
        /// MF明細連携 VONEに登録してあるタグ情報の取得
        /// </summary>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MfAggrTag[]> GetTags(CancellationToken token)
            => (await mfAggrTagProcessor.GetAsync(token)).ToArray();

        /// <summary>
        /// MF明細連携 タグの登録処理
        /// </summary>
        /// <param name="tags">タグ情報</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> SaveTags([FromBody] MfAggrTag[] tags, CancellationToken token)
            => await mfAggrTagProcessor.SaveAsync(tags, token);

        /// <summary>
        /// MF明細連携 口座情報（サブアカウント含む）の取得
        /// </summary>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MfAggrAccount[]> GetAccounts(CancellationToken token)
            => (await mfAggrAccountProcessor.GetAsync(token)).ToArray();

        /// <summary>
        /// MF明細連携 口座情報（サブアカウント含む）の登録
        /// </summary>
        /// <param name="accounts">アカウント情報</param>
        /// <param name="token">自動バインド</param>
        /// <returns>登録件数</returns>
        [HttpPost]
        public async Task<int> SaveAccounts([FromBody] MfAggrAccount[] accounts, CancellationToken token)
            => await mfAggrAccountProcessor.SaveAsync(accounts, token);
    }
}