using System;
using System.Collections.Generic;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class MatchingHeaderProcessor : IMatchingHeaderProcessor
    {
        private readonly IUpdateMatchingHeaderQueryProcessor updateMatchingHeaderProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MatchingHeaderProcessor(
            IUpdateMatchingHeaderQueryProcessor updateMatchingHeaderProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.updateMatchingHeaderProcessor = updateMatchingHeaderProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<MatchingHeadersResult> ApproveAsync(IEnumerable<MatchingHeader> headers, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var items = new List<MatchingHeader>();
                foreach (var header in headers)
                {
                    var result = await updateMatchingHeaderProcessor.ApproveAsync(header, token);
                    if (result == null) return new MatchingHeadersResult {
                        MatchingHeaders = new List<MatchingHeader>(),
                        ProcessResult   = new ProcessResult {
                            ErrorCode   = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated,
                        },
                    };
                    items.Add(result);
                }

                scope.Complete();

                return new MatchingHeadersResult {
                    ProcessResult   = new ProcessResult { Result = true },
                    MatchingHeaders = items,
                };
            }
        }

        public async Task<MatchingHeadersResult> CancelAsync(IEnumerable<MatchingHeader> headers, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var items = new List<MatchingHeader>();
                foreach (var header in headers)
                {
                    var result = await updateMatchingHeaderProcessor.CancelApprovalAsync(header, token);
                    if (result == null)
                        return new MatchingHeadersResult {
                            MatchingHeaders = new List<MatchingHeader>(),
                            ProcessResult   = new ProcessResult {
                                ErrorCode   = Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated,
                            },
                        };
                    items.Add(result);
                }
                scope.Complete();

                return new MatchingHeadersResult {
                    ProcessResult   = new ProcessResult {  Result = true },
                    MatchingHeaders = items,
                };
            }
        }
    }
}
