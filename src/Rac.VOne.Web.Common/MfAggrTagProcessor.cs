using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;

namespace Rac.VOne.Web.Common
{
    public class MfAggrTagProcessor : IMfAggrTagProcessor
    {
        private readonly IAddMfAggrTagQueryProcessor addMfAggrTagQueryProcessor;
        private readonly IMfAggrTagQueryProcessor mfAggrTagQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<MfAggrTag> deleteMfAggrTagQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MfAggrTagProcessor(
            IAddMfAggrTagQueryProcessor addMfAggrTagQueryProcessor,
            IMfAggrTagQueryProcessor mfAggrTagQueryProcessor,
            IDeleteTransactionQueryProcessor<MfAggrTag> deleteMfAggrTagQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.addMfAggrTagQueryProcessor     = addMfAggrTagQueryProcessor;
            this.mfAggrTagQueryProcessor        = mfAggrTagQueryProcessor;
            this.deleteMfAggrTagQueryProcessor  = deleteMfAggrTagQueryProcessor;
            this.transactionScopeBuilder        = transactionScopeBuilder;
        }

        public Task<IEnumerable<MfAggrTag>> GetAsync(CancellationToken token = default(CancellationToken))
            => mfAggrTagQueryProcessor.GetAsync(token);

        public async Task<int> SaveAsync(IEnumerable<MfAggrTag> tags, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = 0;
                var dbTagIds = new HashSet<long>((await mfAggrTagQueryProcessor.GetAsync(token)).Select(x => x.Id).ToArray());

                var tagIds = new HashSet<long>();
                foreach (var tag in tags)
                {
                    await addMfAggrTagQueryProcessor.AddAsync(tag, token);
                    tagIds.Add(tag.Id);
                    result++;
                }

                foreach (var id in dbTagIds.Except(tagIds))
                    await deleteMfAggrTagQueryProcessor.DeleteAsync(id, token);


                scope.Complete();
                return result;
            }
        }

    }
}
