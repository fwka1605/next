using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Web.Common
{
    public class MfAggrAccountProcessor : IMfAggrAccountProcessor
    {
        private readonly IMfAggrAccountQueryProcessor                       mfAggrAccountQueryProcessor;
        private readonly IMfAggrSubAccountQueryProcessor                    mfAggrSubAccountQueryProcessor;
        private readonly IAddMfAggrAccountQueryProcessor                    addMfAggrAccountQueryProcessor;
        private readonly IAddMfAggrSubAccountQueryProcessor                 addMfAggrSubAccountQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<MfAggrAccount>    deleteMfAggrAccountQueryProcessor;
        private readonly IDeleteTransactionQueryProcessor<MfAggrSubAccount> deleteMfAggrSubAccountQueryProcessor;

        private readonly IMfAggrTagRelQueryProcessor        mfAggrTagRelQueryProcessor;
        private readonly IAddMfAggrTagRelQueryProcessor     addMfAggrTagRelQueryProcessor;
        private readonly IDeleteMfAggrTagRelQueryProcessor  deleteMfAggrTagRelQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MfAggrAccountProcessor(
            IMfAggrAccountQueryProcessor mfAggrAccountQueryProcessor,
            IMfAggrSubAccountQueryProcessor mfAggrSubAccountQueryProcessor,
            IAddMfAggrAccountQueryProcessor addMfAggrAccountQueryProcessor,
            IAddMfAggrSubAccountQueryProcessor addMfAggrSubAccountQueryProcessor,
            IDeleteTransactionQueryProcessor<MfAggrAccount> deleteMfAggrAccountQueryProcessor,
            IDeleteTransactionQueryProcessor<MfAggrSubAccount> deleteMfAggrSubAccountQueryProcessor,
            IMfAggrTagRelQueryProcessor         mfAggrTagRelQueryProcessor,
            IAddMfAggrTagRelQueryProcessor      addMfAggrTagRelQueryProcessor,
            IDeleteMfAggrTagRelQueryProcessor   deleteMfAggrTagRelQueryProcessor,
            ITransactionScopeBuilder            transactionScopeBuilder
            )
        {
            this.mfAggrAccountQueryProcessor            = mfAggrAccountQueryProcessor;
            this.mfAggrSubAccountQueryProcessor         = mfAggrSubAccountQueryProcessor;
            this.addMfAggrAccountQueryProcessor         = addMfAggrAccountQueryProcessor;
            this.addMfAggrSubAccountQueryProcessor      = addMfAggrSubAccountQueryProcessor;
            this.deleteMfAggrAccountQueryProcessor      = deleteMfAggrAccountQueryProcessor;
            this.deleteMfAggrSubAccountQueryProcessor   = deleteMfAggrSubAccountQueryProcessor;
            this.mfAggrTagRelQueryProcessor             = mfAggrTagRelQueryProcessor;
            this.addMfAggrTagRelQueryProcessor          = addMfAggrTagRelQueryProcessor;
            this.deleteMfAggrTagRelQueryProcessor       = deleteMfAggrTagRelQueryProcessor;
            this.transactionScopeBuilder                = transactionScopeBuilder;
        }

        public async Task<IEnumerable<MfAggrAccount>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            var subs = (await mfAggrSubAccountQueryProcessor.GetAsync(token)).ToArray();
            var relations = (await mfAggrTagRelQueryProcessor.GetAsync(token)).ToArray();
            var subIds = new SortedList<long, long[]>(relations.GroupBy(x => x.SubAccountId).ToDictionary(x => x.Key, x => x.Select(y => y.TagId).ToArray()));
            foreach (var sub in subs)
                if (subIds.ContainsKey(sub.Id)) sub.TagIds = subIds[sub.Id];

            var subDic = subs
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.ToArray());
            var accounts = (await mfAggrAccountQueryProcessor.GetAsync(token)).ToArray();
            foreach (var account in accounts)
            {
                if (subDic.ContainsKey(account.Id))
                    account.SubAccounts = subDic[account.Id];
                else
                    account.SubAccounts = new MfAggrSubAccount[] { };
            }
            return accounts;
        }

        public async Task<int> SaveAsync(IEnumerable<MfAggrAccount> accounts, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = 0;
                var dbAccounts      = (await mfAggrAccountQueryProcessor.GetAsync(token)).ToArray();
                var dbSubAcocounts  = (await mfAggrSubAccountQueryProcessor.GetAsync(token)).ToArray();

                foreach (var account in accounts)
                {
                    await addMfAggrAccountQueryProcessor.AddAsync(account, token);
                    result++;
                }

                foreach (var sub in accounts.SelectMany(x => x.SubAccounts))
                {
                    await addMfAggrSubAccountQueryProcessor.AddAsync(sub, token);

                    await deleteMfAggrTagRelQueryProcessor.DeleteAsync(sub.Id, null, token);
                    foreach (var id in sub.TagIds)
                        await addMfAggrTagRelQueryProcessor.AddAsync(sub.Id, id, token);
                }

                var accountIds = new HashSet<long>(accounts.Select(x => x.Id).ToArray());
                foreach (var account in dbAccounts.Where(x => !accountIds.Contains(x.Id)))
                    await deleteMfAggrAccountQueryProcessor.DeleteAsync(account.Id, token);

                var subAccountIds = new HashSet<long>(accounts.SelectMany(x => x.SubAccounts).Select(y => y.Id).ToArray());
                foreach (var sub in dbSubAcocounts.Where(x => !subAccountIds.Contains(x.Id)))
                    await deleteMfAggrSubAccountQueryProcessor.DeleteAsync(sub.Id, token);

                scope.Complete();
                return result;
            }
        }
    }
}
