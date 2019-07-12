using Rac.VOne.Common.Security;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class CompanyProcessor : ICompanyProcessor
    {
        private readonly ICompanyQueryProcessor companyQueryProcessor;
        private readonly IAddCompanyQueryProcessor addCompanyQueryProcessor;
        private readonly IDeleteCompanyQueryProcessor deleteCompanyQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CompanyProcessor(
            ICompanyQueryProcessor companyQueryProcessor,
            IAddCompanyQueryProcessor addCompanyQueryProcessor,
            IDeleteCompanyQueryProcessor deleteCompanyQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.companyQueryProcessor          = companyQueryProcessor;
            this.addCompanyQueryProcessor       = addCompanyQueryProcessor;
            this.deleteCompanyQueryProcessor    = deleteCompanyQueryProcessor;
            this.transactionScopeBuilder        = transactionScopeBuilder;
        }

        public async Task<IEnumerable<Company>> GetAsync(CompanySearch option, CancellationToken token = default(CancellationToken))
            => await companyQueryProcessor.GetAsync(option, token);

        public async Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = await deleteCompanyQueryProcessor.DeleteAsync(CompanyId, token);
                if (result != 0) scope.Complete();
                return result;
            }
        }

        public async Task<Company> SaveAsync(Company company, CancellationToken token = default(CancellationToken))
            => await addCompanyQueryProcessor.SaveAsync(company, token);

    }
}
