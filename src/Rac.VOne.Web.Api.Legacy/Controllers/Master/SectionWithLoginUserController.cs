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
    /// Company Master操作用コントロール
    /// </summary>
    public class SectionWithLoginUserController : ApiControllerAuthorized
    {
        private readonly ISectionWithLoginUserProcessor sectionWithLoginUserProcessor;
        private readonly ISectionWithLoginUserFileImportProcessor sectionWithLoginUserImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public SectionWithLoginUserController(
            ISectionWithLoginUserProcessor sectionWithLoginUserProcessor,
            ISectionWithLoginUserFileImportProcessor sectionWithLoginUserImportProcessor
            )
        {
            this.sectionWithLoginUserProcessor = sectionWithLoginUserProcessor;
            this.sectionWithLoginUserImportProcessor = sectionWithLoginUserImportProcessor;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="data"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<SectionWithLoginUser>> Save(MasterImportData<SectionWithLoginUser> data, CancellationToken token)
            => (await sectionWithLoginUserProcessor.SaveAsync(data.InsertItems, data.UpdateItems, token)).ToArray();


        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<SectionWithLoginUser>> GetItems(SectionWithLoginUserSearch option, CancellationToken token)
            => (await sectionWithLoginUserProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// インポート
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task< ImportResult > Import(MasterImportSource source, CancellationToken token)
            => await sectionWithLoginUserImportProcessor.ImportAsync(source, token);

        /// <summary>
        /// ログインユーザーIDの存在確認
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistLoginUser([FromBody] int loginUserId, CancellationToken token)
            => await sectionWithLoginUserProcessor.ExistLoginUserAsync(loginUserId, token);

        /// <summary>
        /// 入金部門IDの存在確認
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistSection([FromBody] int sectionId, CancellationToken token)
            => await sectionWithLoginUserProcessor.ExistSectionAsync(sectionId, token);

    }
}
