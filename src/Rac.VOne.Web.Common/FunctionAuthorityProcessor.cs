using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class FunctionAuthorityProcessor : IFunctionAuthorityProcessor
    {
        private readonly IAddFunctionAuthorityQueryProcessor addFunctionAuthorityQueryProcessor;
        private readonly IFunctionAuthorityByLoginUserIdQueryProcessor functionAuthorityQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public FunctionAuthorityProcessor(
            IAddFunctionAuthorityQueryProcessor addFunctionAuthorityQueryProcessor,
            IFunctionAuthorityByLoginUserIdQueryProcessor functionAuthorityQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.addFunctionAuthorityQueryProcessor = addFunctionAuthorityQueryProcessor;
            this.functionAuthorityQueryProcessor = functionAuthorityQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<FunctionAuthority>> GetAsync(FunctionAuthoritySearch option, CancellationToken token = default(CancellationToken))
            => await functionAuthorityQueryProcessor.GetAsync(option, token);

        public async Task<IEnumerable<FunctionAuthority>> SaveAsync(IEnumerable<FunctionAuthority> authorities, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<FunctionAuthority>();
                foreach (var authority in authorities)
                    result.Add(await addFunctionAuthorityQueryProcessor.SaveAsync(authority, token));

                scope.Complete();

                return result;
            }
        }
    }
}
