using System.Web.Http;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  銀行支店マスター 口座振替などで利用
    /// </summary>
    /// <remarks>
    ///  全銀協からCSVファイルでリリースされている
    ///  銀行・支店 の統廃合があるので、そこそこの頻度でメンテナンスが必要となる
    /// </remarks>
    public class BankBranchController : ApiControllerAuthorized
    {
        private IBankBranchProcessor bankBranchProcess;
        private IBankBranchFileImportProcessor bankBranchImportProcessor;

        /// <summary>constructor</summary>
        public BankBranchController(
            IBankBranchProcessor bankBranchProcess,
            IBankBranchFileImportProcessor bankBranchImportProcessor
            )
        {
            this.bankBranchProcess = bankBranchProcess;
            this.bankBranchImportProcessor = bankBranchImportProcessor;
        }

        /// <summary>銀行支店一覧取得</summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<BankBranch>> GetItems(BankBranchSearch option, CancellationToken token)
            => (await bankBranchProcess.GetAsync(option)).ToArray();

        /// <summary>銀行支店マスター 削除</summary>
        /// <param name="branch">検索用 model キー項目で検索 会社ID,銀行コード,支店コード</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(BankBranch branch, CancellationToken token)
            => await bankBranchProcess.DeleteAsync(branch.CompanyId, branch.BankCode, branch.BranchCode, token);

        /// <summary>銀行支店マスター 登録</summary>
        /// <param name="bankBranch"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BankBranch> Save(BankBranch bankBranch, CancellationToken token)
            => await bankBranchProcess.SaveAsync(bankBranch, token);

        /// <summary>銀行支店マスター インポート処理</summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResult> Import(MasterImportSource source, CancellationToken token)
            => await bankBranchImportProcessor.ImportAsync(source, token);

    }
}