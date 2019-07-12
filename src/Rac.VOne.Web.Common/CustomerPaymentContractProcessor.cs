using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class CustomerPaymentContractProcessor : ICustomerPaymentContractProcessor
    {
        private readonly ICustomerPaymentContractQueryProcessor customerPaymentContractQueryProcessor;
        private readonly IAddCustomerPaymentContractQueryProcessor addCustomerPaymentContractQueryProcessor;
        private readonly IDeleteCustomerPaymentContractQueryProcessor deleteCustomerPaymentContractQueryProcessor;

        public CustomerPaymentContractProcessor(
          ICustomerPaymentContractQueryProcessor customerPaymentContractQueryProcessor,
          IAddCustomerPaymentContractQueryProcessor addCustomerPaymentContractQueryProcessor,
          IDeleteCustomerPaymentContractQueryProcessor deleteCustomerPaymentContractQueryProcessor)
        {
            this.customerPaymentContractQueryProcessor = customerPaymentContractQueryProcessor;
            this.addCustomerPaymentContractQueryProcessor = addCustomerPaymentContractQueryProcessor;
            this.deleteCustomerPaymentContractQueryProcessor = deleteCustomerPaymentContractQueryProcessor;
        }

        public async Task<CustomerPaymentContract> SaveAsync(CustomerPaymentContract contract, CancellationToken token = default(CancellationToken))
            => await addCustomerPaymentContractQueryProcessor.SaveAsync(contract, token);

        public async Task<int> DeleteAsync(int CustomerId, CancellationToken token = default(CancellationToken))
            => await deleteCustomerPaymentContractQueryProcessor.DeleteAsync(CustomerId, token);

        public async Task<bool> ExistCategoryAsync(int Id, CancellationToken token = default(CancellationToken))
            => await customerPaymentContractQueryProcessor.ExistCategoryAsync(Id, token);

        public async Task<IEnumerable<CustomerPaymentContract>> GetAsync(IEnumerable<int> ids, CancellationToken token = default(CancellationToken))
            => await customerPaymentContractQueryProcessor.GetAsync(ids, token);



    }
}
