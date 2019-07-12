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
    public class NettingProcessor:INettingProcessor
    {
        private readonly INettingQueryProcessor nettingQueryProcessor;
        private readonly IAddNettingQueryProcessor updateNettingQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<Netting> nettingDeleteTrancactionQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public NettingProcessor(
            INettingQueryProcessor nettingQueryProcessor,
            IAddNettingQueryProcessor updateNettingQueryProcessor,
            IDeleteTransactionQueryProcessor<Netting> nettingDeleteTrancactionQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.nettingQueryProcessor = nettingQueryProcessor;
            this.updateNettingQueryProcessor = updateNettingQueryProcessor;
            this.nettingDeleteTrancactionQueryProcessor = nettingDeleteTrancactionQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<bool> ExistReceiptCategoryAsync(int ReceiptCatgoryid, CancellationToken token = default(CancellationToken))
            => await nettingQueryProcessor.ExistReceiptCategoryAsync(ReceiptCatgoryid, token);

        public async Task<bool> ExistCustomerAsync(int CustomerId, CancellationToken token = default(CancellationToken))
            => await nettingQueryProcessor.ExistCustomerAsync(CustomerId, token);

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
            => await nettingQueryProcessor.ExistSectionAsync(SectionId, token);

        public async Task<bool> ExistCurrencyAsync(int CurrencyId, CancellationToken token = default(CancellationToken))
            => await nettingQueryProcessor.ExistCurrencyAsync(CurrencyId, token);

        public async Task<IEnumerable<Netting>> SaveAsync(IEnumerable<Netting> nettings, CancellationToken token = default(CancellationToken))
        {
            var result = new List<Netting>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var netting in nettings)
                    result.Add(await updateNettingQueryProcessor.SaveAsync(netting, token));
                scope.Complete();
            }
            return result;
        }

        public async Task<int> DeleteAsync(IEnumerable<long> ids, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var id in ids)
                    result += await nettingDeleteTrancactionQueryProcessor.DeleteAsync(id, token);
                scope.Complete();
            }
            return result;
        }

    }
}
