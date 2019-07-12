using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class BankAccountTypeProcessor : IBankAccountTypeProcessor
    {
        private readonly IBankAccountTypeQueryProcessor bankAccountTypeQueryProcessor;

        public BankAccountTypeProcessor(
            IBankAccountTypeQueryProcessor bankAccountTypeQueryProcessor
            )
        {
            this.bankAccountTypeQueryProcessor = bankAccountTypeQueryProcessor;
        }

        public async Task<IEnumerable<BankAccountType>> GetAsync(CancellationToken token = default(CancellationToken))
            => await bankAccountTypeQueryProcessor.GetAsync(token);

    }
}
