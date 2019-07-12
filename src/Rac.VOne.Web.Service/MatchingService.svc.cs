using Microsoft.AspNet.SignalR;
using NLog;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class MatchingService : IMatchingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
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
        private readonly ILogger logger;
        private readonly IHubContext hubContext;

        public MatchingService(IAuthorizationProcessor authorizationProcessor,
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
            ILogManager logManager
            )

        {
            this.authorizationProcessor = authorizationProcessor;
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
            logger = logManager.GetLogger(typeof(MatchingService));
            hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.ProgressHub>();
        }

        public async Task<CollationsResult> CollateAsync(string SessionKey, CollationSearch CollationSearch, string connectionId)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                try
                {
                    var result = (await collationProcessor.CollateAsync(CollationSearch, token, notifier)).ToList();
                    return new CollationsResult
                    {
                        ProcessResult = new ProcessResult { Result = true },
                        Collation = result,
                    };
                }
                catch (Exception) // cancellation
                {
                    notifier?.Abort();
                    throw;
                }
            }, logger, connectionId);

        public async Task< MatchingResult> SequentialMatchingAsync(string SessionKey,
            Collation[] Collations,
            CollationSearch CollationSearch,
            string connectionId)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                try
                {
                    var result = await matchingSequentialProcessor.MatchAsync(Collations, CollationSearch, token, notifier);
                    return result;
                }
                catch (Exception)
                {
                    notifier?.Abort(); // todo: cancellation
                    throw;
                }
            }, logger, connectionId);

        public async Task<BillingIndicesResult> SimulateAsync(string SessionKey, Billing[] MatchingBilling, decimal SearchValue)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1));
                var result = matchingCombinationSolveProcessor.Solve(MatchingBilling, SearchValue);
                return new BillingIndicesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Indices = result,
                };
            }, logger);

        public async Task<MatchingSourceResult> SolveAsync(string SessionKey, MatchingSource source, CollationSearch option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingSolveProcessor.SolveAsync(source, option, token: token);
                return new MatchingSourceResult
                {
                    MatchingSource = result,
                    ProcessResult = new ProcessResult { Result = true},
                };
            }, logger);

        public async Task<MatchingResult> MatchingIndividuallyAsync(string SessionKey, MatchingSource source)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingIndividualProcessor.MatchAsync(source, token);
                return result;
            }, logger);

        public async Task<MatchingHeadersResult> SearchMatchedDataAsync(string SessionKey, CollationSearch CollationSearch, string connectionId)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = (await matchingProcessor.SearchMatchedDataAsync(CollationSearch, token)).ToList();
                notifier?.UpdateState();
                return new MatchingHeadersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingHeaders = result,
                };
            }, logger, connectionId);

        /// <summary>消込解除</summary>
        /// <param name="SessionKey"></param>
        /// <param name="MatchingHeader">解除する <see cref="MatchingHeader"/>の配列 </param>
        /// <param name="LoginUserId">解除時のログインユーザーID</param>
        /// <returns></returns>
        /// <remarks>
        ///  消込解除を行う
        ///   請求/入金データ いずれかが 論理削除されている場合、消込解除を行わない
        ///   期日現金消込後に作成された 請求データが一部でも消込済の場合に、処理中断
        /// </remarks>
        public async Task<MatchingResult> CancelMatchingAsync(string SessionKey, MatchingHeader[] MatchingHeader, int LoginUserId, string connectionId)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var notifier = hubContext.CreateNotifier(connectionId);
                var result = await matchingCancellationProcessor.CancelAsync(MatchingHeader, LoginUserId, token, notifier);
                return result;
            }, logger, connectionId);

        public async Task<BillingsResult> SearchBillingDataAsync(string SessionKey, MatchingBillingSearch MatchingBillingSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                List<Billing> result = null;
                MatchingBillingSearch.Orders = (await collationSettingProcessor
                    .GetMatchingBillingOrderAsync(MatchingBillingSearch.CompanyId, token))
                    .Where(x => (x.Available == 1)).ToArray();

                if (!MatchingBillingSearch.MatchingHeaderId.HasValue)
                {
                    result = (await matchingProcessor.SearchBillingDataAsync(MatchingBillingSearch, token)).ToList();
                }
                else
                {
                    result = (await matchingProcessor.GetMatchedBillingsAsync(MatchingBillingSearch, token)).ToList();
                }
                return new BillingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Billings = result,
                };
            }, logger);
        }

        public async Task<ReceiptsResult> SearchReceiptDataAsync(string SessionKey, MatchingReceiptSearch MatchingReceiptSearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                List<Receipt> result = null;
                MatchingReceiptSearch.Orders = (await collationSettingProcessor
                    .GetMatchingReceiptOrderAsync(MatchingReceiptSearch.CompanyId, token))
                    .Where(x => (x.Available == 1)).ToArray();

                if (!MatchingReceiptSearch.MatchingHeaderId.HasValue)
                {
                    result = (await matchingProcessor.SearchReceiptDataAsync(MatchingReceiptSearch, token)).ToList();
                }
                else
                {
                    result = (await matchingProcessor.GetMatchedReceiptsAsync(MatchingReceiptSearch, token)).ToList();
                }
                return new ReceiptsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Receipts = result,
                };
            }, logger);
        }

        public async Task<MatchingHeadersResult> ApproveAsync(string SessionKey, MatchingHeader[] headers)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return await matchingHeaderProcessor.ApproveAsync(headers, token);
            }, logger);
        }

        public async Task<MatchingHeadersResult> CancelApprovalAsync(string SessionKey, MatchingHeader[] headers)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                return await matchingHeaderProcessor.CancelAsync(headers, token);
            }, logger);
        }

        public async Task<ReceiptsResult> searchReceiptByIdAsync(string SessionKey, long[] ReceiptId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await matchingProcessor.SearchReceiptByIdAsync(ReceiptId, token)).ToList();
                return new ReceiptsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Receipts = result,
                };
            }, logger);
        }

        public async Task<CountResult> SaveWorkDepartmentTargetAsync(string SessionKey, int CompanyId, byte[] ClientKey, int[] DepartmentIds)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var count = await matchingProcessor.SaveWorkDepartmentTargetAsync(ClientKey, CompanyId, DepartmentIds, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true, },
                    Count = count,
                };
            }, logger);

        public async Task<CountResult> SaveWorkSectionTargetAsync(string SessionKey, int CompanyId, byte[] ClientKey, int[] SectionIds)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var count = await matchingProcessor.SaveWorkSectionTargetAsync(ClientKey, CompanyId, SectionIds, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = count,
                };
            }, logger);

        public async Task<MatchingsResult> GetAsync(string SessionKey, long[] Ids)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var entities = (await matchingProcessor.GetAsync(Ids, token)).ToList();
                return new MatchingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Matchings = entities
                };
            }, logger);
        }

        public async Task<MatchingHeadersResult> GetHeaderItemsAsync(string SessionKey, long[] Ids)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var entities = (await matchingProcessor.GetHeaderItemsAsync(Ids, token)).ToList();
                return new MatchingHeadersResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingHeaders = entities,
                };
            }, logger);

        }

        #region journalizing
        public async Task<MatchingJournalizingDetailsResult> GetMatchingJournalizingDetailAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingJournalizingProcessor.GetDetailsAsync(option, token);
                return new MatchingJournalizingDetailsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingJournalizingDetails = result.ToList(),
                };
            }, logger);

        public async Task<MatchingJournalizingProcessResult> CancelMatchingJournalizingDetailAsync(string SessionKey,
            MatchingJournalizingDetail[] MatchingJournalizingDetail)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await matchingJournalizingProcessor.CancelDetailsAsync(MatchingJournalizingDetail, token);
                return new MatchingJournalizingProcessResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                };
            }, logger);

        public async Task<JournalizingSummariesResult> GetMatchingJournalizingSummaryAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingJournalizingProcessor.GetSummaryAsync(option, token);
                return new JournalizingSummariesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    JournalizingsSummaries = result.ToList(),
                };
            }, logger);

        public async Task<MatchingJournalizingsResult> ExtractMatchingJournalizingAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingJournalizingProcessor.ExtractAsync(option, token);
                return new MatchingJournalizingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingJournalizings = result.ToList(),
                };
            }, logger);

        public async Task<GeneralJournalizingsResult> ExtractGeneralJournalizingAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingJournalizingProcessor.ExtractGeneralJournalizingAsync(option, token);
                return new GeneralJournalizingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    GeneralJournalizings = result.ToList(),
                };
            }, logger);

        public async Task<MatchedReceiptsResult> GetMatchedReceiptAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingJournalizingProcessor.GetMatchedReceiptAsync(option, token);
                return new MatchedReceiptsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchedReceipts = result.ToList(),
                };
            }, logger);

        public async Task<MatchingJournalizingProcessResult> UpdateOutputAtAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await matchingJournalizingProcessor.UpdateAsync(option, token);
                return new MatchingJournalizingProcessResult { ProcessResult = new ProcessResult { Result = true } };
            }, logger);

        public async Task<MatchingJournalizingProcessResult> CancelMatchingJournalizingAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                await matchingJournalizingProcessor.CancelAsync(option, token);
                return new MatchingJournalizingProcessResult { ProcessResult = new ProcessResult { Result = true } };
            }, logger);

        public async Task<MatchingJournalizingsResult> MFExtractMatchingJournalizingAsync(string SessionKey, JournalizingOption option)
            => await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await matchingJournalizingProcessor.MFExtractAsync(option, token);
                return new MatchingJournalizingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    MatchingJournalizings = result.ToList(),
                };
            }, logger);

        #endregion
    }
}
