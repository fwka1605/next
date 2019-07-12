using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  会社マスター 登録処理が web common に実装されていない
    /// </summary>
    public class CompanyController : ApiControllerAuthorized
    {

        private readonly ICompanyProcessor              companyProcessor;
        private readonly ICompanyLogoProcessor          companyLogoProcessor;
        private readonly ICompanyInitializeProcessor    companyInitializeProcessor;
        /// <summary>constructor</summary>
        public CompanyController(
            ICompanyProcessor               companyProcessor,
            ICompanyLogoProcessor           companyLogoProcessor,
            ICompanyInitializeProcessor     companyInitializeProcessor
            )
        {
            this.companyProcessor               = companyProcessor;
            this.companyLogoProcessor           = companyLogoProcessor;
            this.companyInitializeProcessor     = companyInitializeProcessor;
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Company>> GetItems(CompanySearch option, CancellationToken token)
            => (await companyProcessor.GetAsync(option, token)).ToArray();

        /// <summary>削除</summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<int> Delete([FromBody] int companyId, CancellationToken token)
            => await companyProcessor.DeleteAsync(companyId, token);

        /// <summary>ロゴ削除</summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<int> DeleteLogo([FromBody] int companyId, CancellationToken token)
            => await companyLogoProcessor.DeleteByCompanyIdAsync(companyId, token);

        /// <summary>ロゴ削除</summary>
        /// <param name="logos"></param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<int> DeleteLogos(IEnumerable<CompanyLogo> logos, CancellationToken token)
            => await companyLogoProcessor.DeleteAsync(logos, token);

        /// <summary>会社ロゴ取得</summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<IEnumerable<CompanyLogo>> GetLogos([FromBody] int companyId, CancellationToken token)
            => (await companyLogoProcessor.GetAsync(companyId, token)).ToArray();

        /// <summary>会社マスター 登録</summary>
        /// <param name="company"></param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<Company> Save(Company company, CancellationToken token)
            => await companyProcessor.SaveAsync(company, token);

        /// <summary>会社ロゴ登録 複数件</summary>
        /// <param name="logos"></param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<IEnumerable<CompanyLogo>> SaveLogos(IEnumerable<CompanyLogo> logos, CancellationToken token)
            => (await companyLogoProcessor.SaveAsync(logos, token)).ToArray();

        /// <summary>会社マスター 新規作成 web common で実施すべき内容</summary>
        /// <param name="source">会社マスター 新規登録用のモデル</param>
        /// <param name="token">自動バインド</param>
        [HttpPost]
        public async Task<Company> Initialize(CompanySource source, CancellationToken token)
            => await companyInitializeProcessor.InitializeAsync(source, token);

    }
}
