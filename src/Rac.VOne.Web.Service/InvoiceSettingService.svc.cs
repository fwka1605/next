using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using Rac.VOne.Common.Logging;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    public class InvoiceSettingService : IInvoiceSettingService
    {
        private readonly IAuthorizationProcessor authorizationProcessor;
        private readonly IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor;
        private readonly IInvoiceNumberHistoryProcessor invoiceNumberHistoryProcessor;
        private readonly IInvoiceNumberSettingProcessor invoiceNumberSettingProcessor;
        private readonly IInvoiceTemplateSettingProcessor invoiceTemplateSettingProcessor;
        private readonly ICategoryProcessor categoryProcessor;
        private readonly ILogger logger;

        public InvoiceSettingService(
            IAuthorizationProcessor authorizationProcessor,
            IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor,
            IInvoiceNumberHistoryProcessor invoiceNumberHistoryProcessor,
            IInvoiceNumberSettingProcessor invoiceNumberSettingProcessor,
            IInvoiceTemplateSettingProcessor invoiceTemplateSettingProcessor,
            ICategoryProcessor categoryProcessor,
            ILogManager logManager
            )
        {
            this.authorizationProcessor = authorizationProcessor;
            this.invoiceCommonSettingProcessor = invoiceCommonSettingProcessor;
            this.invoiceNumberHistoryProcessor = invoiceNumberHistoryProcessor;
            this.invoiceNumberSettingProcessor = invoiceNumberSettingProcessor;
            this.invoiceTemplateSettingProcessor = invoiceTemplateSettingProcessor;
            this.categoryProcessor = categoryProcessor;
            logger = logManager.GetLogger(typeof(InvoiceSettingService));
        }

        public async Task<InvoiceCommonSettingResult> GetInvoiceCommonSettingAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceCommonSettingProcessor.GetAsync(CompanyId, token);
                return new InvoiceCommonSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceCommonSetting = result,
                };
            }, logger);
        }

        public async Task<InvoiceCommonSettingResult> SaveInvoiceCommonSettingAsync(string SessionKey, InvoiceCommonSetting InvoiceCommonSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceCommonSettingProcessor.SaveAsync(InvoiceCommonSetting, token);
                return new InvoiceCommonSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceCommonSetting = result,
                };
            }, logger);
        }

        public async Task<CategoriesResult> UpdateCollectCategoryAsync(string SessionKey, IEnumerable<Category> CollectCategories)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await categoryProcessor.SaveItemsAsync(CollectCategories)).ToList();
                return new CategoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Categories = result,
                };
            }, logger);
        }

        public async Task<InvoiceNumberHistoriesResult> GetInvoiceNumberHistoriesAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceNumberHistoryProcessor.GetItemsAsync(CompanyId, token);
                return new InvoiceNumberHistoriesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceNumberHistories = result.ToList(),
                };
            }, logger);
        }

        public async Task<InvoiceNumberHistoryResult> SaveInvoiceNumberHistoryAsync(string SessionKey, InvoiceNumberHistory InvoiceNumberHistory)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceNumberHistoryProcessor.SaveAsync(InvoiceNumberHistory, token);
                return new InvoiceNumberHistoryResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceNumberHistory = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteInvoiceNumberHistoriesAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceNumberHistoryProcessor.DeleteAsync(CompanyId, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }

        public async Task<InvoiceNumberSettingResult> GetInvoiceNumberSettingAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceNumberSettingProcessor.GetAsync(CompanyId, token);
                return new InvoiceNumberSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceNumberSetting = result,
                };
            }, logger);
        }

        public async Task<InvoiceNumberSettingResult> SaveInvoiceNumberSettingAsync(string SessionKey, InvoiceNumberSetting InvoiceNumberSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceNumberSettingProcessor.SaveAsync(InvoiceNumberSetting, token);
                return new InvoiceNumberSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceNumberSetting = result,
                };
            }, logger);
        }

        public async Task<ExistResult> ExistCollectCategoryAtTemplateAsync(string SessionKey, int CollectCategoryId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceTemplateSettingProcessor.ExistCollectCategoryAsync(CollectCategoryId, token);
                return new ExistResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Exist = result,
                };
            }, logger);
        }

        public async Task<InvoiceTemplateSettingResult> GetInvoiceTemplateSettingByCodeAsync(string SessionKey, int CompanyId, string Code)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceTemplateSettingProcessor.GetByCodeAsync(CompanyId, Code, token);
                return new InvoiceTemplateSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceTemplateSetting = result,
                };
            }, logger);
        }

        public async Task<InvoiceTemplateSettingsResult> GetInvoiceTemplateSettingsAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = (await invoiceTemplateSettingProcessor.GetItemsAsync(CompanyId, token)).ToList();
                return new InvoiceTemplateSettingsResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceTemplateSettings = result,
                };
            }, logger);
        }

        public async Task<InvoiceTemplateSettingResult> SaveInvoiceTemplateSettingAsync(string SessionKey, InvoiceTemplateSetting InvoiceTemplateSetting)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceTemplateSettingProcessor.SaveAsync(InvoiceTemplateSetting, token);
                return new InvoiceTemplateSettingResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    InvoiceTemplateSetting = result,
                };
            }, logger);
        }

        public async Task<CountResult> DeleteInvoiceTemplateSettingAsync(string SessionKey, int Id)
        {
            return await authorizationProcessor.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await invoiceTemplateSettingProcessor.DeleteAsync(Id, token);
                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = result,
                };
            }, logger);
        }



    }
}
