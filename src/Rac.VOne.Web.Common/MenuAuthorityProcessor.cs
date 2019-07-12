using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class MenuAuthorityProcessor : IMenuAuthorityProcessor
    {
        private readonly IAddMenuAuthorityQueryProcessor addMenuAuthorityQueryProcessor;
        private readonly IDeleteByCompanyQueryProcessor<MenuAuthority> deleteMenuAuthorityQueryProcessor;
        private readonly IMenuAuthorityQueryProcessor menuAuthorityQueryProcessor;
        private readonly IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public MenuAuthorityProcessor(
            IAddMenuAuthorityQueryProcessor addMenuAuthorityQueryProcessor,
            IDeleteByCompanyQueryProcessor<MenuAuthority> deleteMenuAuthorityQueryProcessor,
            IMenuAuthorityQueryProcessor menuAuthorityQueryProcessor,
            IByCompanyGetEntityQueryProcessor<ApplicationControl> applicationControlQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.addMenuAuthorityQueryProcessor = addMenuAuthorityQueryProcessor;
            this.deleteMenuAuthorityQueryProcessor = deleteMenuAuthorityQueryProcessor;
            this.menuAuthorityQueryProcessor = menuAuthorityQueryProcessor;
            this.applicationControlQueryProcessor = applicationControlQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<int> DeleteAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await deleteMenuAuthorityQueryProcessor.DeleteAsync(CompanyId, token);

        public async Task<IEnumerable<MenuAuthority>> GetAsync(MenuAuthoritySearch option, CancellationToken token = default(CancellationToken))
            => await menuAuthorityQueryProcessor.GetAsync(option, token);

        public async Task<IEnumerable<MenuAuthority>> SaveAsync(IEnumerable<MenuAuthority> menus, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new List<MenuAuthority>();
                foreach (var menu in menus)
                    result.Add(await addMenuAuthorityQueryProcessor.SaveAsync(menu, token));

                scope.Complete();

                return result;
            }
        }
    }
}
