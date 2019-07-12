using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class BillingBalanceBackProcessor: IBillingBalanceBackProcessor
    {
        private readonly IAddBillingBalanceBackQueryProcessor addBillingBalanceBackProcessor;
        private readonly IDeleteByCompanyQueryProcessor<BillingBalanceBack> deleteBillingBalanceBackProcessor;

        public BillingBalanceBackProcessor(
            IAddBillingBalanceBackQueryProcessor addBillingBalanceBackProcessor,
            IDeleteByCompanyQueryProcessor<BillingBalanceBack> deleteBillingBalanceBackProcessor
            )
        {
            this.addBillingBalanceBackProcessor = addBillingBalanceBackProcessor;
            this.deleteBillingBalanceBackProcessor = deleteBillingBalanceBackProcessor;
        }

        public Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => deleteBillingBalanceBackProcessor.DeleteAsync(CompanyId, token);

        public Task<IEnumerable<BillingBalanceBack>> SaveAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => addBillingBalanceBackProcessor.SaveAsync(CompanyId, token);
    }
}
