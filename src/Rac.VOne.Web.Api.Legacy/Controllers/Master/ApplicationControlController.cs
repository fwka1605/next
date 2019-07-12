using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  アプリケーション設定 取得/保存
    /// </summary>
    public class ApplicationControlController : ApiControllerAuthorized
    {
        private readonly IApplicationControlProcessor applicationControlProcessor;

        /// <summary>コンストラクタ</summary>
        public ApplicationControlController(
            IApplicationControlProcessor applicationControlProcessor
            )
        {
            this.applicationControlProcessor = applicationControlProcessor;
        }

        /// <summary>
        /// アプリケーションの設定を取得
        /// </summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApplicationControl> Get([FromBody] int companyId, CancellationToken token)
            => await applicationControlProcessor.GetAsync(companyId, token);

        /// <summary>
        /// ログ保存するかどうかだけ変更する web api
        /// </summary>
        /// <param name="applicationControl"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Save(ApplicationControl applicationControl, CancellationToken token)
            => await applicationControlProcessor.UpdateUseOperationLogAsync(applicationControl, token);


        /// <summary>
        /// アプリケーション設定の新規登録
        /// 新会社作成のタイミングで呼び出し実施
        /// </summary>
        /// <param name="applicationControl"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApplicationControl> Add(ApplicationControl applicationControl, CancellationToken token)
            => await applicationControlProcessor.AddAsync(applicationControl, token);

    }
}
