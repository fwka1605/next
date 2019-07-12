using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Common
{
    public class MatchingOutputedProcessor : IMatchingOutputedProcessor
    {
        private readonly IAddMatchingOutputedQueryProcessor addMatchingOutputedQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MatchingOutputedProcessor(
            IAddMatchingOutputedQueryProcessor addMatchingOutputedQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )

        {
            this.addMatchingOutputedQueryProcessor = addMatchingOutputedQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<int> SaveOutputAtAsync(IEnumerable<long> MatchingHeaderIds, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var id in MatchingHeaderIds)
                    result += await addMatchingOutputedQueryProcessor.SaveOutputAtAsync(id, token);
                scope.Complete();
            }
            return result;
        }
    }
}
