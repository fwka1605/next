﻿using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    ///  項目名称設定
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ColumnNameSettingController : ControllerBase
    {

        private readonly IColumnNameSettingProcessor columnNameSettingProcessor;

        /// <summary>constructor</summary>
        public ColumnNameSettingController(
            IColumnNameSettingProcessor columnNameSettingProcessor
            )
        {
            this.columnNameSettingProcessor = columnNameSettingProcessor;
        }

        /// <summary>項目名称取得 単項目</summary>
        /// <param name="setting">単一項目取得 キー項目で検索</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ColumnNameSetting>>> Get(ColumnNameSetting setting, CancellationToken token)
            => (await columnNameSettingProcessor.GetAsync(setting, token)).ToArray();

        /// <summary>項目名称 登録</summary>
        /// <param name="columnName">項目名称</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<ActionResult<ColumnNameSetting>> Save(ColumnNameSetting columnName, CancellationToken token)
            => await columnNameSettingProcessor.SaveAsync(columnName, token);
    }
}
