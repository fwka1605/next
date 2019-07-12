﻿using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  管理マスター
    /// </summary>
    public class GeneralSettingController : ApiControllerAuthorized
    {
        private readonly IGeneralSettingProcessor generalSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public GeneralSettingController(
            IGeneralSettingProcessor generalSettingProcessor
            )
        {
            this.generalSettingProcessor = generalSettingProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="setting">会社ID 必須, コード 任意
        /// コード未入力の場合、会社IDが同一の すべてのレコードを取得</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<GeneralSetting>> GetItems(GeneralSetting setting, CancellationToken token)
            => (await generalSettingProcessor.GetAsync(setting, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GeneralSetting> Save(GeneralSetting setting, CancellationToken token)
            => await generalSettingProcessor.SaveAsync(setting, token);


    }
}