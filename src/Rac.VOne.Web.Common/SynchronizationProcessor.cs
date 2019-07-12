using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common.TypeMapping;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Web.Common
{
    public class SynchronizationProcessor : ISynchronizationProcessor
    {
        private readonly ISynchronizationQueryProcessor synchronizationQueryProcessor;

        public SynchronizationProcessor(ISynchronizationQueryProcessor synchronizationQueryProcessor)
        {
            this.synchronizationQueryProcessor = synchronizationQueryProcessor;
        }

        public async Task<IEnumerable<Entity>> CheckCustomerAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<Customer, Entity>(UpdateAt);
        }

        public async Task<IEnumerable<Entity>> CheckCompanyAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<Company, Entity>(UpdateAt);
        }


        public async Task<IEnumerable<Entity>> CheckDepartmentAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<Department, Entity>(UpdateAt);
        }


        public async Task<IEnumerable<Entity>> CheckStaffAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<Staff, Entity>(UpdateAt);
        }


        public async Task<IEnumerable<Entity>> CheckLoginUserAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<LoginUser, Entity>(UpdateAt);
        }


        public async Task<IEnumerable<Entity>> CheckAccountTitleAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<AccountTitle, Entity>(UpdateAt);
        }


        public async Task<IEnumerable<Entity>> CheckBankAccountAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<BankAccount, Entity>(UpdateAt);
        }


        public async Task<IEnumerable<Transaction>> CheckBillingAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<Billing, Transaction>(UpdateAt);
        }


        public async Task<IEnumerable<Transaction>> CheckReceiptAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<Receipt, Transaction>(UpdateAt);
        }


        public async Task<IEnumerable<Transaction>> CheckMatchingHeaderAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<MatchingHeader, Transaction>(UpdateAt);
        }


        public async Task<IEnumerable<Transaction>> CheckMatchingAsync(DateTime UpdateAt, CancellationToken token = default(CancellationToken))
        {
            return await synchronizationQueryProcessor.CheckAsync<Matching, Transaction>(UpdateAt);
        }
    }
}
