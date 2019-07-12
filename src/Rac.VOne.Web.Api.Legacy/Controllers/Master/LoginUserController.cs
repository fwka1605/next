using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  ログインユーザーマスター
    /// </summary>
    public class LoginUserController : ApiControllerAuthorized
    {
        private readonly ILoginUserProcessor loginUserProcessor;
        private readonly ILoginUserPasswordProcessor loginUserPasswordProcessor;
        private readonly ILoginUserFileImportProcessor loginUserImportProcessor;

        /// <summary>
        /// constructor
        /// </summary>
        public LoginUserController(
            ILoginUserProcessor loginUserProcessor,
            ILoginUserPasswordProcessor loginUserPasswordProcessor,
            ILoginUserFileImportProcessor loginUserImportProcessor
            )
        {
            this.loginUserProcessor = loginUserProcessor;
            this.loginUserPasswordProcessor = loginUserPasswordProcessor;
            this.loginUserImportProcessor = loginUserImportProcessor;
        }

        /// <summary>
        ///取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<LoginUser>> GetItems(LoginUserSearch option, CancellationToken token)
            => (await loginUserProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// パスワードのリセット
        /// </summary>
        /// <param name="Id">ログインユーザーID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> ResetPassword([FromBody] int Id, CancellationToken token)
            => await loginUserPasswordProcessor.ResetAsync(Id, token);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="Id">ログインユーザーID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete([FromBody] int Id, CancellationToken token)
            => await loginUserProcessor.DeleteAsync(Id, token);

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="loginUser"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginUser> Save(LoginUser loginUser, CancellationToken token)
            => await loginUserProcessor.SaveAsync(loginUser, token);
 
        /// <summary>
        /// 担当者ID の存在確認
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExitStaff([FromBody] int staffId, CancellationToken token)
            => await loginUserProcessor.ExitStaffAsync(staffId, token);

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await loginUserImportProcessor.ImportAsync(source, token);

    }
}
