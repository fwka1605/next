using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 帳票設定マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportSettingController : ControllerBase
    {
        private readonly IReportSettingProcessor reportSettingProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public ReportSettingController(
            IReportSettingProcessor reportSettingProcessor
            )
        {
            this.reportSettingProcessor = reportSettingProcessor;
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="setting">会社ID、レポートID を設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReportSetting>>> GetItems(ReportSetting setting, CancellationToken token = default(CancellationToken))
            => (await reportSettingProcessor.GetAsync(setting.CompanyId, setting.ReportId, token)).ToArray();

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReportSetting>>> Save(IEnumerable<ReportSetting> settings, CancellationToken token = default(CancellationToken))
            => (await reportSettingProcessor.SaveAsync(settings, token)).ToArray();
    }
}
