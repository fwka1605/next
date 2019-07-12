using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Rac.VOne.Web.Common
{
   public class MFBillingProcessor : IMFBillingProcessor
    {
        private readonly IBillingProcessor billingProcessor;
        private readonly ICustomerProcessor customerProcessor;
        private readonly IStaffProcessor staffProcessor;
        private readonly IBillingSaveProcessor billingSaveProcessor;
        private readonly IMFBillingQueryProcessor mfBillingQueryProcessor;
        private readonly IAddMFBillingQueryProcessor addMFBillingQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<MFBilling> mfBillingByCompanyGetEntitiesQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MFBillingProcessor(IBillingProcessor billingProcessor,
            ICustomerProcessor customerProcessor,
            IStaffProcessor staffProcessor,
            IBillingSaveProcessor billingSaveProcessor,
            IMFBillingQueryProcessor mfBillingQueryProcessor,
            IAddMFBillingQueryProcessor addMFBillingQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<MFBilling> mfBillingByCompanyGetEntitiesQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingProcessor = billingProcessor;
            this.customerProcessor = customerProcessor;
            this.staffProcessor = staffProcessor;
            this.billingSaveProcessor = billingSaveProcessor;
            this.mfBillingQueryProcessor = mfBillingQueryProcessor;
            this.addMFBillingQueryProcessor = addMFBillingQueryProcessor;
            this.mfBillingByCompanyGetEntitiesQueryProcessor = mfBillingByCompanyGetEntitiesQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }


        /// <summary>登録</summary>
        /// <param name="source">MFC請求書 の ID（ハッシュ文字列）は、<see cref="Billing.CustomerKana"/>へ設定されているものとする</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Billing>> SaveAsync(MFBillingSource source, CancellationToken token = default(CancellationToken))
        {
            var billings        = source.Billings;
            var customers       = source.Customers;
            var newCustomers = new List<Customer>();
            var result = new List<Billing>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var c in customers)
                    newCustomers.Add(await customerProcessor.SaveAsync(c, token: token));

                var staffs = (await staffProcessor.GetAsync(new StaffSearch {
                    Ids = newCustomers.Select(x => x.StaffId).Distinct().ToArray()
                }, token)).ToDictionary(x => x.Id);

                foreach (var billing in billings)
                {
                    if (billing.CustomerId == 0)
                    {
                        var customer    = newCustomers.Where(x => x.Code == billing.CustomerCode).FirstOrDefault();
                        var staff       = staffs[customer.StaffId];

                        billing.CustomerId          = customer.Id;
                        billing.StaffId             = customer.StaffId;
                        billing.CollectCategoryId   = customer.CollectCategoryId;
                        billing.DepartmentId        = staff.DepartmentId;
                    }
                    var savedBilling = await billingSaveProcessor.SaveAsync(billing, token);
                    var mfId = billing.CustomerKana;
                    var mfBilling = new MFBilling
                    {
                        BillingId   = savedBilling.Id,
                        CompanyId   = savedBilling.CompanyId,
                        Id          = mfId
                    };
                    await addMFBillingQueryProcessor.SaveAsync(mfBilling, token);
                    result.Add(savedBilling);
                }
                scope.Complete();
            }
            return result;
        }

        public Task<IEnumerable<MFBilling>> GetAsync(MFBillingSource source, CancellationToken token = default(CancellationToken))
            => mfBillingQueryProcessor.GetItems(source, token);

    }
}
