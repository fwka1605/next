using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 営業担当者マスター
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffProcessor staffProcessor;
        private readonly IStaffFileImportProcessor staffImportProcessor;

        /// <summary>constructor</summary>
        public StaffController(
            IStaffProcessor staffProcessor,
            IStaffFileImportProcessor staffImportProcessor
            )
        {
            this.staffProcessor = staffProcessor;
            this.staffImportProcessor = staffImportProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="staff"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< Staff >> Save(Staff staff, CancellationToken token)
            => await staffProcessor.SaveAsync(staff, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="staff">IDを指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> Delete(Staff staff, CancellationToken token)
            => await staffProcessor.DeleteAsync(staff.Id, token);


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件 会社ID、 担当者コード 配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Staff>>> GetItems(StaffSearch option, CancellationToken token)
            => (await staffProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// 請求部門ID の登録があるか確認
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> ExistDepartment([FromBody] int departmentId, CancellationToken token)
            => await staffProcessor.ExistDepartmentAsync(departmentId, token);


        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ImportResult>> Import(MasterImportSource source, CancellationToken token)
            => await staffImportProcessor.ImportAsync(source, token);

    }
}
