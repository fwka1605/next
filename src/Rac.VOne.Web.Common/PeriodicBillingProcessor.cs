using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class PeriodicBillingProcessor : IPeriodicBillingProcesssor
    {
        private readonly IBillingSaveForInputProcessor billingSaveForInputProcessor;

        private readonly IAddPeriodicBillingCreatedQueryProcessor addPeriodicBillingCreatedQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Customer> customerQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<HolidayCalendar> holidayCalendarQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;


        public PeriodicBillingProcessor(
            IBillingSaveForInputProcessor billingSaveForInputProcessor,
            IAddPeriodicBillingCreatedQueryProcessor addPeriodicBillingCreatedQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<Customer> customerQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<HolidayCalendar> holidayCalendarQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingSaveForInputProcessor = billingSaveForInputProcessor;
            this.addPeriodicBillingCreatedQueryProcessor = addPeriodicBillingCreatedQueryProcessor;
            this.customerQueryProcessor = customerQueryProcessor;
            this.holidayCalendarQueryProcessor = holidayCalendarQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Billing>> CreateAsync(IEnumerable<PeriodicBillingSetting> settings, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<Billing>();
                var first = settings.First();
                var companyId = first.CompanyId;
                var customers = (await customerQueryProcessor
                    .GetByIdsAsync(settings.Select(x => x.CustomerId).Distinct().ToArray(), token))
                    .ToDictionary(x => x.Id);
                var holidays = (await holidayCalendarQueryProcessor.GetItemsAsync(companyId, token)).ToArray();
                foreach (var setting in settings)
                {
                    var billings = setting.GetBillings(customers[setting.CustomerId], holidays).ToArray();
                    result.AddRange((await billingSaveForInputProcessor.SaveAsync(billings, token)).ToArray());
                    await addPeriodicBillingCreatedQueryProcessor.SaveAsync(new PeriodicBillingCreated {
                        PeriodicBillingSettingId    = setting.Id,
                        CreateYearMonth             = setting.BaseDate,
                        CreateBy                    = setting.UpdateBy,
                        UpdateBy                    = setting.UpdateBy,
                    }, token);
                }

                scope.Complete();

                return result;
            }
        }
    }
}
