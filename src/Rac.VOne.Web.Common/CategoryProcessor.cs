using System;
using System.Collections.Generic;
using System.Linq;
using Rac.VOne.Web.Models;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class CategoryProcessor : ICategoryProcessor
    {
        private readonly ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor;
        private readonly ICategoriesQueryProcessor categoriesQueryProcessor;
        private readonly IAddCategoryQueryProcessor addCategoryQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<Category> categoryDeleteIdenticalQueryProcessor;
        private readonly IAccountTitleQueryProcessor accountTitleByIdQueryProcessor;
        private readonly ITaxClassQueryProcessor taxClassQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> identicalGetByIdsQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<Category> categoryIdenticalEntityGetByIdsQueryProcessor;
        private readonly IIdenticalEntityGetByIdsQueryProcessor<AccountTitle> accountTitleIdenticalEntityGetByIdsQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public CategoryProcessor(
            ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor,
            ICategoriesQueryProcessor categoriesQueryProcessor,
            IAddCategoryQueryProcessor addCategoryQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<Category> categoryDeleteIdenticalQueryProcessor,
            IAccountTitleQueryProcessor accountTitleByIdQueryProcessor,
            ITaxClassQueryProcessor taxClassQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<PaymentAgency> identicalGetByIdsQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<Category> categoryIdenticalEntityGetByIdsQueryProcessor,
            IIdenticalEntityGetByIdsQueryProcessor<AccountTitle> accountTitleIdenticalEntityGetByIdsQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.categoryByCodeQueryProcessor = categoryByCodeQueryProcessor;
            this.categoriesQueryProcessor = categoriesQueryProcessor;
            this.addCategoryQueryProcessor = addCategoryQueryProcessor;
            this.categoryDeleteIdenticalQueryProcessor = categoryDeleteIdenticalQueryProcessor;
            this.accountTitleByIdQueryProcessor = accountTitleByIdQueryProcessor;
            this.taxClassQueryProcessor = taxClassQueryProcessor;
            this.identicalGetByIdsQueryProcessor = identicalGetByIdsQueryProcessor;
            this.categoryIdenticalEntityGetByIdsQueryProcessor = categoryIdenticalEntityGetByIdsQueryProcessor;
            this.accountTitleIdenticalEntityGetByIdsQueryProcessor = accountTitleIdenticalEntityGetByIdsQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<Category> SaveAsync(Category category, CancellationToken token = default(CancellationToken))
            => await addCategoryQueryProcessor.SaveAsync(category, token);

        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await categoryDeleteIdenticalQueryProcessor.DeleteAsync(Id, token);

        public async Task<bool> ExistAccountTitleAsync(int AccountTitleId, CancellationToken token = default(CancellationToken))
            => await categoriesQueryProcessor.ExistAccountTitleAsync(AccountTitleId, token);

        public async Task<bool> ExistPaymentAgencyAsync(int PaymentAgencyId, CancellationToken token = default(CancellationToken))
            => await categoriesQueryProcessor.ExistPaymentAgencyAsync(PaymentAgencyId, token);

        public async Task<IEnumerable<Category>> GetAsync(CategorySearch option, CancellationToken token = default(CancellationToken))
            => await categoryByCodeQueryProcessor.GetAsync(option, token);

        public async Task<IEnumerable<Category>> SaveItemsAsync(IEnumerable<Category> categories, CancellationToken token = default(CancellationToken))
        {
            var result = new List<Category>();
            using (var scope = transactionScopeBuilder.Create())
            {
                foreach (var category in categories)
                    result.Add(await addCategoryQueryProcessor.SaveAsync(category, token));
            }
            return result;
        }
    }
}
