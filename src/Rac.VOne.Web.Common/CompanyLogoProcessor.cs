using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class CompanyLogoProcessor :
        ICompanyLogoProcessor
    {
        private readonly IAddCompanyLogoQueryProcessor addCompanyLogoQueryProcessor;
        private readonly IDeleteCompanyLogoQueryProcessor deleteCompanyLogoQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<CompanyLogo> deleteCompanyLogoByCompanyQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<CompanyLogo> companyLogoGetByCompanyIdQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CompanyLogoProcessor(
            IAddCompanyLogoQueryProcessor addCompanyLogoQueryProcessor,
            IDeleteCompanyLogoQueryProcessor deleteCompanyLogoQueryProcessor,
            IDeleteByCompanyQueryProcessor<CompanyLogo> deleteCompanyLogoByCompanyQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<CompanyLogo> companyLogoGetByCompanyIdQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.addCompanyLogoQueryProcessor = addCompanyLogoQueryProcessor;
            this.deleteCompanyLogoQueryProcessor = deleteCompanyLogoQueryProcessor;
            this.deleteCompanyLogoByCompanyQueryProcessor = deleteCompanyLogoByCompanyQueryProcessor;
            this.companyLogoGetByCompanyIdQueryProcessor = companyLogoGetByCompanyIdQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<CompanyLogo>> GetAsync(int companyId, CancellationToken token = default(CancellationToken))
            => await companyLogoGetByCompanyIdQueryProcessor.GetItemsAsync(companyId, token);

        public async Task<IEnumerable<CompanyLogo>> SaveAsync(IEnumerable<CompanyLogo> logos, CancellationToken token = default(CancellationToken))
        {
            var result = new List<CompanyLogo>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var logo in logos)
                    result.Add(await addCompanyLogoQueryProcessor.SaveAsync(logo, token));
                scope.Complete();
            }
            return result;
        }

        public async Task<int> DeleteByCompanyIdAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await deleteCompanyLogoByCompanyQueryProcessor.DeleteAsync(CompanyId, token);

        public async Task<int> DeleteAsync(IEnumerable<CompanyLogo> CompanyLogos, CancellationToken token = default(CancellationToken))
        {
            var result = 0;
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var companyLogo in CompanyLogos)
                    result += await deleteCompanyLogoQueryProcessor.DeleteAsync(companyLogo.CompanyId, companyLogo.LogoType, token);
                scope.Complete();
            }
            return result;
        }
    }
}
