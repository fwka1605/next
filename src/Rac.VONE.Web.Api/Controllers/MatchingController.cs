using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Api.Extensions;
using Rac.VOne.Web.Api.Hubs;

namespace Rac.VOne.Web.Api.Controllers
{
    /// <summary>
    /// 消込関連
    /// </summary>
    [TypeFilter(typeof(Filters.AuthorizationFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MatchingController : ControllerBase
    {
        private readonly ICollationSettingProcessor collationSettingProcessor;
        private readonly ICollationProcessor collationProcessor;
        private readonly IMatchingHeaderProcessor matchingHeaderProcessor;
        private readonly IMatchingProcessor matchingProcessor;
        private readonly IMatchingSequentialProcessor matchingSequentialProcessor;
        private readonly IMatchingIndividualProcessor matchingIndividualProcessor;
        private readonly IMatchingCancellationProcessor matchingCancellationProcessor;
        private readonly IMatchingSolveProcessor matchingSolveProcessor;
        private readonly IMatchingCombinationSolveProcessor matchingCombinationSolveProcessor;
        private readonly IMatchingJournalizingProcessor matchingJournalizingProcessor;
        private readonly IHubContext<ProgressHub> hubContext;

        /// <summary>constructor</summary>
        public MatchingController(IAuthorizationProcessor authorizationProcessor,
            ICollationSettingProcessor collationSettingProcessor,
            ICollationProcessor collationProcessor,
            IMatchingHeaderProcessor matchingHeaderProcessor,
            IMatchingProcessor matchingProcessor,
            IMatchingSequentialProcessor matchingSequentialProcessor,
            IMatchingIndividualProcessor matchingIndividualProcessor,
            IMatchingCancellationProcessor matchingCancellationProcessor,
            IMatchingSolveProcessor matchingSolveProcessor,
            IMatchingCombinationSolveProcessor matchingCombinationSolveProcessor,
            IMatchingJournalizingProcessor matchingJournalizingProcessor,
            IHubContext<ProgressHub> hubContext
            )

        {
            this.collationSettingProcessor = collationSettingProcessor;
            this.collationProcessor = collationProcessor;
            this.matchingHeaderProcessor = matchingHeaderProcessor;
            this.matchingProcessor = matchingProcessor;
            this.matchingSequentialProcessor = matchingSequentialProcessor;
            this.matchingIndividualProcessor = matchingIndividualProcessor;
            this.matchingCancellationProcessor = matchingCancellationProcessor;
            this.matchingSolveProcessor = matchingSolveProcessor;
            this.matchingCombinationSolveProcessor = matchingCombinationSolveProcessor;
            this.matchingJournalizingProcessor = matchingJournalizingProcessor;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// 照合処理
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< Collation >>> CollateR(CollationSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token) =>(await collationProcessor.CollateAsync(option, token, notifier)).ToArray());

        /// <summary>
        /// 照合処理
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Collation>>> Collate(CollationSearch option, CancellationToken token)
            => (await collationProcessor.CollateAsync(option, token, null)).ToArray();

        /// <summary>
        /// 一括消込
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< MatchingResult >> SequentialMatchingR(MatchingSequentialSource source)
            => await hubContext.DoAsync(source.Option.ConnectionId, async (notifier, token) => await matchingSequentialProcessor.MatchAsync(source.Collations, source.Option, token, notifier));

        /// <summary>
        /// 一括消込
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MatchingResult>> SequentialMatching(MatchingSequentialSource source, CancellationToken token)
            => await matchingSequentialProcessor.MatchAsync(source.Collations, source.Option, token, null);

        /// <summary>
        /// シミュレーション機能
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<int>>> SimulateR(MatchingSimulationSource source)
            => await hubContext.DoAsync(async token =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(0));
                return matchingCombinationSolveProcessor.Solve(source.Billings, source.TargetAmount);
            });

        /// <summary>
        /// シミュレーション機能
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<int>>> Simulate(MatchingSimulationSource source)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(0));
            return matchingCombinationSolveProcessor.Solve(source.Billings, source.TargetAmount);
        }

        /// <summary>
        /// 消込結果確認
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MatchingSource>> SolveR(MatchingSource source)
            => await hubContext.DoAsync(async token
                => await matchingSolveProcessor.SolveAsync(source, source.Option, token: token));
        /// <summary>
        /// 消込結果確認
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MatchingSource>> Solve(MatchingSource source, CancellationToken token)
            => await matchingSolveProcessor.SolveAsync(source, source.Option, token: token);

        /// <summary>
        /// 消込（個別）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< MatchingResult >> MatchingIndividuallyR(MatchingSource source)
            => await hubContext.DoAsync(async token => await matchingIndividualProcessor.MatchAsync(source, token));

        /// <summary>
        /// 消込（個別）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MatchingResult>> MatchingIndividually(MatchingSource source, CancellationToken token)
            => await matchingIndividualProcessor.MatchAsync(source, token);

        /// <summary>
        /// 消込済データの取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< MatchingHeader >>> SearchMatchedDataR(CollationSearch option)
            => await hubContext.DoAsync(option.ConnectionId, async (notifier, token) =>
            {
                var result = (await matchingProcessor.SearchMatchedDataAsync(option, token)).ToArray();
                notifier?.UpdateState();

                return result;

            });

        /// <summary>
        /// 消込済データの取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MatchingHeader>>> SearchMatchedData(CollationSearch option, CancellationToken token)
        {
            var result = (await matchingProcessor.SearchMatchedDataAsync(option, token)).ToArray();
            return result;
        }

        /// <summary>消込解除</summary>
        /// <param name="source">解除する <see cref="MatchingHeader"/>の配列 LoginUserId, ConnectionId を指定</param>
        /// <returns></returns>
        /// <remarks>
        ///  消込解除を行う
        ///   請求/入金データ いずれかが 論理削除されている場合、消込解除を行わない
        ///   期日現金消込後に作成された 請求データが一部でも消込済の場合に、処理中断
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult< MatchingResult >> CancelMatchingR(MatchingCancelSource source)
            => await hubContext.DoAsync(source.ConnectionId, async (notifier, token)
                => await matchingCancellationProcessor.CancelAsync(source.Headers, source.LoginUserId, token, notifier));

        /// <summary>消込解除</summary>
        /// <param name="source">解除する <see cref="MatchingHeader"/>の配列 LoginUserId を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        /// <remarks>
        ///  消込解除を行う
        ///   請求/入金データ いずれかが 論理削除されている場合、消込解除を行わない
        ///   期日現金消込後に作成された 請求データが一部でも消込済の場合に、処理中断
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<MatchingResult>> CancelMatching(MatchingCancelSource source, CancellationToken token)
            => await matchingCancellationProcessor.CancelAsync(source.Headers, source.LoginUserId, token, null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Billing>>> SearchBillingData(MatchingBillingSearch option, CancellationToken token)
        {
            List<Billing> result = null;
            option.Orders = (await collationSettingProcessor
                .GetMatchingBillingOrderAsync(option.CompanyId, token))
                .Where(x => (x.Available == 1)).ToArray();


            if (!option.MatchingHeaderId.HasValue)
            {
                result = (await matchingProcessor.SearchBillingDataAsync(option, token)).ToList();
            }
            else
            {
                result = (await matchingProcessor.GetMatchedBillingsAsync(option, token)).ToList();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Receipt>>> SearchReceiptData(MatchingReceiptSearch option, CancellationToken token)
        {
            List<Receipt> result = null;
            option.Orders = (await collationSettingProcessor
                .GetMatchingReceiptOrderAsync(option.CompanyId, token))
                .Where(x => (x.Available == 1)).ToArray();

            if (!option.MatchingHeaderId.HasValue)
            {
                result = (await matchingProcessor.SearchReceiptDataAsync(option, token)).ToList();
            }
            else
            {
                result = (await matchingProcessor.GetMatchedReceiptsAsync(option, token)).ToList();
            }
            return result;
        }

        /// <summary>
        /// 承認処理
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MatchingHeadersResult>> Approve(IEnumerable<MatchingHeader> headers, CancellationToken token)
            => await matchingHeaderProcessor.ApproveAsync(headers, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MatchingHeadersResult>> CancelApproval(IEnumerable<MatchingHeader> headers, CancellationToken token)
            => await matchingHeaderProcessor.CancelAsync(headers, token);

        /// <summary>
        /// IDから 入金データ取得 個別消込画面から利用？
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< Receipt >>> SearchReceiptById(long[] ids, CancellationToken token)
            => (await matchingProcessor.SearchReceiptByIdAsync(ids, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> SaveWorkDepartmentTargetR(WorkDepartmentTargetSource source)
            => await hubContext.DoAsync(async token =>
            {
                return await matchingProcessor.SaveWorkDepartmentTargetAsync(source.ClientKey, source.CompanyId, source.DepartmentIds, token);
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> SaveWorkDepartmentTarget(WorkDepartmentTargetSource source, CancellationToken token)
        {
            return await matchingProcessor.SaveWorkDepartmentTargetAsync(source.ClientKey, source.CompanyId, source.DepartmentIds, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> SaveWorkSectionTargetR(WorkSectionTargetSource source)
            => await hubContext.DoAsync(async token =>
            {
                return await matchingProcessor.SaveWorkSectionTargetAsync(source.ClientKey, source.CompanyId, source.SectionIds, token);
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> SaveWorkSectionTarget(WorkSectionTargetSource source, CancellationToken token)
        {
            return await matchingProcessor.SaveWorkSectionTargetAsync(source.ClientKey, source.CompanyId, source.SectionIds, token);
        }

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Matching>>> Get([FromBody] long[] ids, CancellationToken token)
            => (await matchingProcessor.GetAsync(ids, token)).ToArray();

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< MatchingHeader >>> GetHeaderItems([FromBody] long[] ids, CancellationToken token)
            => (await matchingProcessor.GetHeaderItemsAsync(ids, token)).ToArray();

        #region journalizing

        /// <summary>
        /// 仕訳明細 配列 取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< MatchingJournalizingDetail >>> GetMatchingJournalizingDetailR(JournalizingOption option)
            => await hubContext.DoAsync(async token
                => (await matchingJournalizingProcessor.GetDetailsAsync(option, token)).ToArray());

        /// <summary>
        /// 仕訳明細 配列 取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MatchingJournalizingDetail>>> GetMatchingJournalizingDetail(JournalizingOption option, CancellationToken token)
            => (await matchingJournalizingProcessor.GetDetailsAsync(option, token)).ToArray();

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult< MatchingJournalizingProcessResult >> CancelMatchingJournalizingDetailR(
            IEnumerable<MatchingJournalizingDetail> details)
            => await hubContext.DoAsync(async token =>
            {
                await matchingJournalizingProcessor.CancelDetailsAsync(details, token);
                return new MatchingJournalizingProcessResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            });

        /// <summary>取消</summary>
        /// <param name="details"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MatchingJournalizingProcessResult>> CancelMatchingJournalizingDetail(
            IEnumerable<MatchingJournalizingDetail> details, CancellationToken token)
        {
            await matchingJournalizingProcessor.CancelDetailsAsync(details, token);
            return new MatchingJournalizingProcessResult
            {
                ProcessResult = new ProcessResult { Result = true },
            };
        }

        /// <summary>仕訳履歴取得</summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< JournalizingSummary >>> GetMatchingJournalizingSummaryR(JournalizingOption option)
            => await hubContext.DoAsync(async token
                => (await matchingJournalizingProcessor.GetSummaryAsync(option, token)).ToArray());

        /// <summary>仕訳履歴取得</summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<JournalizingSummary>>> GetMatchingJournalizingSummary(JournalizingOption option, CancellationToken token)
            => (await matchingJournalizingProcessor.GetSummaryAsync(option, token)).ToArray();

        /// <summary>
        /// 仕訳取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< MatchingJournalizing >>> ExtractMatchingJournalizingR(JournalizingOption option)
            => await hubContext.DoAsync(async token
                => (await matchingJournalizingProcessor.ExtractAsync(option, token)).ToArray());

        /// <summary>
        /// 仕訳取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MatchingJournalizing>>> ExtractMatchingJournalizing(JournalizingOption option, CancellationToken token)
            => (await matchingJournalizingProcessor.ExtractAsync(option, token)).ToArray();

        /// <summary>
        /// 汎用仕訳取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< GeneralJournalizing >>> ExtractGeneralJournalizingR(JournalizingOption option)
            => await hubContext.DoAsync(async token
                => (await matchingJournalizingProcessor.ExtractGeneralJournalizingAsync(option, token)).ToArray());

        /// <summary>
        /// 汎用仕訳取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GeneralJournalizing>>> ExtractGeneralJournalizing(JournalizingOption option, CancellationToken token)
            => (await matchingJournalizingProcessor.ExtractGeneralJournalizingAsync(option, token)).ToArray();

        /// <summary>
        /// 消込済入金データ 取得
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< MatchedReceipt >>> GetMatchedReceiptR(JournalizingOption option)
            => await hubContext.DoAsync(async token
                => (await matchingJournalizingProcessor.GetMatchedReceiptAsync(option, token)).ToArray());

        /// <summary>
        /// 消込済入金データ 取得
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MatchedReceipt>>> GetMatchedReceipt(JournalizingOption option, CancellationToken token)
            => (await matchingJournalizingProcessor.GetMatchedReceiptAsync(option, token)).ToArray();

        /// <summary>
        /// 仕訳出力済 フラグ更新処理
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdateOutputAtR(JournalizingOption option)
            => await hubContext.DoAsync(async token => await matchingJournalizingProcessor.UpdateAsync(option, token));

        /// <summary>
        /// 仕訳出力済 フラグ更新処理
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> UpdateOutputAt(JournalizingOption option, CancellationToken token)
            => await matchingJournalizingProcessor.UpdateAsync(option, token);

        /// <summary>
        /// 仕訳出力取消処理
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CancelMatchingJournalizingR(JournalizingOption option)
            => await hubContext.DoAsync(async token => await matchingJournalizingProcessor.CancelAsync(option, token));

        /// <summary>
        /// 仕訳出力取消処理
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CancelMatchingJournalizing(JournalizingOption option, CancellationToken token)
            => await matchingJournalizingProcessor.CancelAsync(option, token);

        /// <summary>
        /// MF仕訳出力抽出
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable< MatchingJournalizing >>> MFExtractMatchingJournalizingR(JournalizingOption option)
            => await hubContext.DoAsync(async token
                => (await matchingJournalizingProcessor.MFExtractAsync(option, token)).ToArray());

        /// <summary>
        /// MF仕訳出力抽出
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MatchingJournalizing>>> MFExtractMatchingJournalizing(JournalizingOption option, CancellationToken token)
            => (await matchingJournalizingProcessor.MFExtractAsync(option, token)).ToArray();

        #endregion
    }
}
