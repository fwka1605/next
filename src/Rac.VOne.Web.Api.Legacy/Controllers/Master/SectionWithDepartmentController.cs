using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    /// 入金部門 ・請求部門関連マスター
    /// </summary>
    public class SectionWithDepartmentController : ApiControllerAuthorized
    {
        private ISectionWithDepartmentProcessor sectionWithDepartmentProcessor;
        private ISectionWithDepartmentFileImportProcessor sectionWithDepartmentImportProcessor;

        /// <summary>constructor</summary>
        public SectionWithDepartmentController(
            ISectionWithDepartmentProcessor sectionWithDepartmentProcessor,
            ISectionWithDepartmentFileImportProcessor sectionWithDepartmentImportProcessor
            )
        {
            this.sectionWithDepartmentProcessor = sectionWithDepartmentProcessor;
            this.sectionWithDepartmentImportProcessor = sectionWithDepartmentImportProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="data">マスターインポート用 insert / delete に値を設定する</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<SectionWithDepartment>> Save(MasterImportData<SectionWithDepartment> data, CancellationToken token)
            => (await sectionWithDepartmentProcessor.SaveAsync(data.InsertItems, data.DeleteItems, token)).ToArray();


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<SectionWithDepartment>> GetItems(SectionWithDepartmentSearch option, CancellationToken token)
            => (await sectionWithDepartmentProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await sectionWithDepartmentImportProcessor.ImportAsync(source, token);

        /// <summary>
        /// 入金部門ID の登録確認
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistSection([FromBody] int sectionId, CancellationToken token)
            => await sectionWithDepartmentProcessor.ExistSectionAsync(sectionId, token);

        /// <summary>
        /// 請求部門ID の登録確認
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistDepartment([FromBody] int departmentId, CancellationToken token)
            => await sectionWithDepartmentProcessor.ExistDepartmentAsync(departmentId, token);
    }
}
