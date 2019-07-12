using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddCustomerPaymentContractQueryProcessor
    {
        Task<CustomerPaymentContract> SaveAsync(CustomerPaymentContract contract, CancellationToken token = default(CancellationToken));

    }
}
