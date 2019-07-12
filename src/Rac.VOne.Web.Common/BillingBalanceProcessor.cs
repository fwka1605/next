using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class BillingBalanceProcessor : IBillingBalanceProcessor
    {
        private readonly IBillingBalanceQueryProcessor billingBalanceQueryProcessor;
        private readonly IDeleteBillingBalanceQueryProcessor deleteBillingBalanceQueryProcessor;
        private readonly IAddBillingBalanceQueryProcessor addBillingBalanceQueryProcessor;

        public BillingBalanceProcessor(
            IBillingBalanceQueryProcessor billingBalanceQueryProcessor,
            IDeleteBillingBalanceQueryProcessor deleteBillingBalanceQueryProcessor,
            IAddBillingBalanceQueryProcessor addBillingBalanceQueryProcessor
            )
        {
            this.billingBalanceQueryProcessor = billingBalanceQueryProcessor;
            this.deleteBillingBalanceQueryProcessor = deleteBillingBalanceQueryProcessor;
            this.addBillingBalanceQueryProcessor = addBillingBalanceQueryProcessor;
        }

        public Task<DateTime?> GetLastCarryOverAtAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => billingBalanceQueryProcessor.GetLastCarryOverAtAsync(CompanyId);

        public Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => deleteBillingBalanceQueryProcessor.DeleteAsync(CompanyId);

        public Task<BillingBalance> SaveAsync(BillingBalance BillingBalance, CancellationToken token = default(CancellationToken))
            => addBillingBalanceQueryProcessor.SaveAsync(BillingBalance);

        public Task<IEnumerable<BillingBalance>> GetBillingBalancesAsync(int CompanyId, DateTime? LastCarryOverAt, DateTime CarryOverAt, CancellationToken token = default(CancellationToken))
            => billingBalanceQueryProcessor.GetBillingBalancesAsync(CompanyId, LastCarryOverAt, CarryOverAt);

        public Task<IEnumerable<BillingBalance>> GetLastBillingBalanceAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, CancellationToken token = default(CancellationToken))
            => billingBalanceQueryProcessor.GetLastBillingBalanceAsync(CompanyId, CurrencyId, CustomerId, StaffId, DepartmentId);

        public Task<decimal> GetBillingAmountAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, DateTime CarryOverAt, DateTime? LastCarryOverAt, CancellationToken token = default(CancellationToken))
        {
            return billingBalanceQueryProcessor.GetBillingAmountAsync(CompanyId, CurrencyId, CustomerId,
                StaffId, DepartmentId, CarryOverAt, LastCarryOverAt);
        }

        public Task<decimal> GetReceiptAmountAsync(int CompanyId, int CurrencyId, int CustomerId, int StaffId, int DepartmentId, DateTime CarryOverAt, DateTime? LastCarryOverAt, CancellationToken token = default(CancellationToken))
        {
            return billingBalanceQueryProcessor.GetReceiptAmountAsync(CompanyId, CurrencyId, CustomerId,
                StaffId, DepartmentId, CarryOverAt, LastCarryOverAt);
        }

        public Task<IEnumerable<BillingBalance>> RestoreBillingBalanceAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            return addBillingBalanceQueryProcessor.RestoreBillingBalanceAsync(CompanyId);
        }

    }
}
