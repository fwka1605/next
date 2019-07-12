using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Common
{
    public class CustomerFeeProcessor:ICustomerFeeProcessor
    {
        private readonly ICustomerFeeQueryProcessor customerFeeQueryProcessor;
        private readonly IAddCustomerFeeQueryProcessor addCustomerFeeQueryProcessor;
        private readonly IDeleteCustomerFeeQueryProcessor deleteCustomerFeeQueryProcessor;
        private readonly IMasterGetByCodesQueryProcessor<Currency> currencyGetByCodesQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyIdQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CustomerFeeProcessor(
            ICustomerFeeQueryProcessor customerFeeQueryProcessor,
            IAddCustomerFeeQueryProcessor addCustomerFeeQueryProcessor,
            IDeleteCustomerFeeQueryProcessor deleteCustomerFeeQueryProcessor,
            IMasterGetByCodesQueryProcessor<Currency> currencyGetByCodesQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlByCompanyIdQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.customerFeeQueryProcessor          = customerFeeQueryProcessor;
            this.addCustomerFeeQueryProcessor       = addCustomerFeeQueryProcessor;
            this.deleteCustomerFeeQueryProcessor    = deleteCustomerFeeQueryProcessor;
            this.currencyGetByCodesQueryProcessor   = currencyGetByCodesQueryProcessor;
            this.applicationControlByCompanyIdQueryProcessor = applicationControlByCompanyIdQueryProcessor;
            this.transactionScopeBuilder            = transactionScopeBuilder;
        }

        public async Task<IEnumerable<CustomerFee>> GetAsync(CustomerFeeSearch option, CancellationToken token = default(CancellationToken))
            => await customerFeeQueryProcessor.GetAsync(option, token);

        public async Task<IEnumerable<CustomerFee>> SaveAsync(IEnumerable<CustomerFee> fees, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<CustomerFee>();
                var fee1st = fees.First();
                var customerId = fee1st.CustomerId;
                var currencyId = fee1st.CurrencyId;
                foreach (var fee in fees.Where(f => f.Fee != null && f.Fee != f.NewFee))
                {
                    await deleteCustomerFeeQueryProcessor.DeleteAsync(new CustomerFeeSearch
                    {
                        CustomerId = customerId,
                        CurrencyId = currencyId,
                        Fee = fee.Fee.Value,
                    }, token);
                }

                foreach (var fee in fees.Where(f => f.Fee != f.NewFee && f.NewFee != 0))
                {
                    fee.Fee = fee.NewFee;
                    result.Add(await addCustomerFeeQueryProcessor.SaveAsync(fee, token));
                }

                scope.Complete();

                return result;
            }
        }

        //public async Task<int> DeleteAsync(CustomerFeeSearch option, CancellationToken token = default(CancellationToken))
        //    => await deleteCustomerFeeQueryProcessor.DeleteAsync(option, token);

        public async Task<ImportResult> ImportAsync(IEnumerable<CustomerFee> insert, IEnumerable<CustomerFee> update, IEnumerable<CustomerFee> delete, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var deleteCount = 0;
                var updateCount = 0;
                var insertCount = 0;

                var defaultCurrencyId = 0;
                var isNotUseForeignCurrency = true;
                if (insert.Any())
                {
                    var companyId = insert.First().CompanyId;
                    var appControl = await applicationControlByCompanyIdQueryProcessor.GetAsync(companyId, token);
                    isNotUseForeignCurrency = appControl.UseForeignCurrency == 0;

                    if (isNotUseForeignCurrency)
                        defaultCurrencyId = (await currencyGetByCodesQueryProcessor.GetByCodesAsync(companyId, new[] { DefaultCurrencyCode })).First().Id;
                }

                foreach (var x in delete)
                {
                    await deleteCustomerFeeQueryProcessor.DeleteAsync(new CustomerFeeSearch { CustomerId = x.CustomerId }, token);
                    deleteCount++;
                }

                foreach (var x in update)
                {
                    await addCustomerFeeQueryProcessor.SaveAsync(x);
                    ++updateCount;
                }

                foreach (var x in insert)
                {
                    if (isNotUseForeignCurrency) x.CurrencyId = defaultCurrencyId;
                    await addCustomerFeeQueryProcessor.SaveAsync(x);
                    ++insertCount;
                }

                scope.Complete();

                return new ImportResult {
                    ProcessResult   = new ProcessResult { Result = true ,},
                    InsertCount     = insertCount,
                    UpdateCount     = updateCount,
                    DeleteCount     = deleteCount,
                };
            }
        }
    }
}
