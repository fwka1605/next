using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// 営業担当者マスター
    /// </summary>
    public class StaffController : ApiControllerAuthorized
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
        public async Task< Staff > Save(Staff staff, CancellationToken token)
            => await staffProcessor.SaveAsync(staff, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="staff">IDを指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(Staff staff, CancellationToken token)
            => await staffProcessor.DeleteAsync(staff.Id, token);


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件 会社ID、 担当者コード 配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Staff>> GetItems(StaffSearch option, CancellationToken token)
            => (await staffProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// 請求部門ID の登録があるか確認
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistDepartment([FromBody] int departmentId, CancellationToken token)
            => await staffProcessor.ExistDepartmentAsync(departmentId, token);

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await staffImportProcessor.ImportAsync(source, token);


        /// <summary>
        /// インポート用
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsLoginUser(MasterSearchOption option, CancellationToken token)
            => (await staffProcessor.GetImportItemsLoginUserAsync(option.CompanyId, option.Codes, token)).ToArray();

        /// <summary>
        /// インポート用
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsCustomer(MasterSearchOption option, CancellationToken token)
            => (await staffProcessor.GetImportItemsCustomerAsync(option.CompanyId, option.Codes, token)).ToArray();

        /// <summary>
        /// インポート用
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsBilling(MasterSearchOption option, CancellationToken token)
            => (await staffProcessor.GetImportItemsBillingAsync(option.CompanyId, option.Codes, token)).ToArray();

    }
}
