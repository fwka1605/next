using Rac.VOne.Import;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Api.Legacy.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;


namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  請求部門マスター
    /// </summary>
    public class DepartmentController : ApiControllerAuthorized
    {
        private readonly IDepartmentProcessor departmentProcessor;
        private readonly IDepartmentFileImportProcessor departmentImportProcessor;
        /// <summary>
        /// constructor
        /// </summary>
        public DepartmentController(
            IDepartmentProcessor departmentProcessor,
            IDepartmentFileImportProcessor departmentImportProcessor
            )
        {
            this.departmentProcessor = departmentProcessor;
            this.departmentImportProcessor = departmentImportProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="departments">請求部門</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Department>> Save(IEnumerable<Department> departments, CancellationToken token)
            => (await departmentProcessor.SaveAsync(departments, token)).ToArray();

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="department">請求部門ID を指定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(Department department, CancellationToken token)
            => await departmentProcessor.DeleteAsync(department.Id, token);

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Department>> GetItems(DepartmentSearch option, CancellationToken token)
            => (await departmentProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source">インポート用モデル</param>
        /// <param name="token">自動バインド</param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await departmentImportProcessor.ImportAsync(source, token);

        /// <summary>
        /// 連携される請求部門コード配列 に 含まれない 営業担当者が登録されている 請求部門 一覧
        /// </summary>
        /// <param name="option">会社ID、除外したい 請求部門コード配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsStaff(MasterSearchOption option, CancellationToken token)
            => (await departmentProcessor.GetImportItemsStaffAsync(option.CompanyId, option.Codes, token)).ToArray();

        /// <summary>
        /// 連携される請求部門コード配列に 含まれない 入金部門 関連テーブルへの登録がある 請求部門一覧
        /// </summary>
        /// <param name="option">会社ID、除外したい 請求部門コード配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartment(MasterSearchOption option, CancellationToken token)
            => (await departmentProcessor.GetImportItemsSectionWithDepartmentAsync(option.CompanyId, option.Codes, token)).ToArray();

        /// <summary>
        /// 連携される請求部門コード配列に 含まれない 請求データへの登録がある 請求部門一覧
        /// </summary>
        /// <param name="option">会社ID、除外したい 請求部門コード配列 を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MasterData>> GetImportItemsBilling(MasterSearchOption option, CancellationToken token)
            => (await departmentProcessor.GetImportItemsBillingAsync(option.CompanyId, option.Codes, token)).ToArray();

    }
}
