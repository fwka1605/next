using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;

namespace Rac.VOne.Web.Service.Master
{
    public class CategoryMaster : ICategoryMaster
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly ILogger logger;

        public CategoryMaster(
            IAuthorizationProcessor authorizationProcessor,
            ICategoryProcessor categoryProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.categoryProcessor = categoryProcessor;
            logger = logManager.GetLogger(typeof(CategoryMaster));
        }

        public async Task<CategoriesResult> GetAsync(string SessionKey, int[] Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await categoryProcessor.GetAsync(new CategorySearch { Ids = Id }, token)).ToList();
                return new CategoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Categories = result,
                };
            }, logger);
        }

        public async Task<CategoriesResult> GetItemsAsync(string SessionKey, CategorySearch CategorySearch)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await categoryProcessor.GetAsync(CategorySearch, token)).ToList();
                return new CategoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Categories = result,
                };
            }, logger);
        }

        public async Task<CategoriesResult> GetByCodeAsync(string SessionKey, int CompanyId, int CategoryType, string[] Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await categoryProcessor.GetAsync(new CategorySearch {
                    CompanyId = CompanyId,
                    CategoryType = CategoryType,
                    Codes = Code,
                }, token)).ToList();

                return new CategoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Categories = result,
                };
            }, logger);
        }

        public async Task<CategoriesResult> GetInvoiceCollectCategoriesAsync(string SessionKey, int CompanyId, int CategoryType)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await categoryProcessor.GetAsync(new CategorySearch {
                    CompanyId    = CompanyId,
                    CategoryType = CategoryType,
                }, token)).ToList();

                return new CategoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Categories = result,
                };
            }, logger);
        }

        public async Task<CategoryResult> SaveAsync(string SessionKey, Category Category)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await categoryProcessor.SaveAsync(Category, token);
                return new CategoryResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Category = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await categoryProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistAccountTitleAsync(string SessionKey, int AccountTitleId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await categoryProcessor.ExistAccountTitleAsync(AccountTitleId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistPaymentAgencyAsync(string SessionKey, int PaymentAgencyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await categoryProcessor.ExistPaymentAgencyAsync(PaymentAgencyId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }
    }
}
