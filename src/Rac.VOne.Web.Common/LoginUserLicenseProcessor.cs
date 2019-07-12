using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;

namespace Rac.VOne.Web.Common
{
    public class LoginUserLicenseProcessor : ILoginUserLicenseProcessor
    {
        private readonly IByCompanyGetEntitiesQueryProcessor<LoginUserLicense> getLoginUserLicenseQueryProcessor;
        private readonly ILoginUserLicenseQueryProcessor loginUserLicenseQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public LoginUserLicenseProcessor(
            IByCompanyGetEntitiesQueryProcessor<LoginUserLicense> getLoginUserLicenseQueryProcessor,
            ILoginUserLicenseQueryProcessor loginUserLicenseQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.getLoginUserLicenseQueryProcessor = getLoginUserLicenseQueryProcessor;
            this.loginUserLicenseQueryProcessor = loginUserLicenseQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<LoginUserLicense>> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
             => await getLoginUserLicenseQueryProcessor.GetItemsAsync(CompanyId, token);

        public async Task<IEnumerable<LoginUserLicense>> SaveAsync(IEnumerable<LoginUserLicense> licenses, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<LoginUserLicense>();
                foreach (var license in licenses)
                    result.Add(await loginUserLicenseQueryProcessor.SaveAsync(license, token));

                scope.Complete();

                return result;
            }
        }

    }
}
