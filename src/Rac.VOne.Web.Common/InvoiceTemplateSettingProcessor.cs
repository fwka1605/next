using System.Collections.Generic;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class InvoiceTemplateSettingProcessor : IInvoiceTemplateSettingProcessor
    {
        private readonly IInvoiceTemplateSettingQueryProcessor invoiceTemplateSettingQueryProcessor;
        private readonly IByCompanyGetEntitiesQueryProcessor<InvoiceTemplateSetting> getInvoiceTemplateSettingQueryProcessor;
        private readonly IMasterGetByCodeQueryProcessor<InvoiceTemplateSetting> getInvoiceTemplateSettingByCodeQueryProcessor;
        private readonly IAddInvoiceTemplateSettingQueryProcessor addInvoiceTemplateSettingQueryProcessor;
        private readonly IDeleteIdenticalEntityQueryProcessor<InvoiceTemplateSetting> deleteInvoiceTemplateSettingQueryProcessor;

        public InvoiceTemplateSettingProcessor(
            IInvoiceTemplateSettingQueryProcessor invoiceTemplateSettingQueryProcessor,
            IByCompanyGetEntitiesQueryProcessor<InvoiceTemplateSetting> getInvoiceTemplateSettingQueryProcessor,
            IMasterGetByCodeQueryProcessor<InvoiceTemplateSetting> getInvoiceTemplateSettingByCodeQueryProcessor,
            IAddInvoiceTemplateSettingQueryProcessor addInvoiceTemplateSettingQueryProcessor,
            IDeleteIdenticalEntityQueryProcessor<InvoiceTemplateSetting> deleteInvoiceTemplateSettingQueryProcessor
            )
        {
            this.invoiceTemplateSettingQueryProcessor = invoiceTemplateSettingQueryProcessor;
            this.getInvoiceTemplateSettingQueryProcessor = getInvoiceTemplateSettingQueryProcessor;
            this.getInvoiceTemplateSettingByCodeQueryProcessor = getInvoiceTemplateSettingByCodeQueryProcessor;
            this.addInvoiceTemplateSettingQueryProcessor = addInvoiceTemplateSettingQueryProcessor;
            this.deleteInvoiceTemplateSettingQueryProcessor = deleteInvoiceTemplateSettingQueryProcessor;
        }

        public async Task<bool> ExistCollectCategoryAsync(int CollectCategoryId, CancellationToken token = default(CancellationToken))
            => await invoiceTemplateSettingQueryProcessor.ExistCollectCategoryAsync(CollectCategoryId, token);

        public async Task<InvoiceTemplateSetting> GetByCodeAsync(int CompanyId, string Code, CancellationToken token = default(CancellationToken))
            => await getInvoiceTemplateSettingByCodeQueryProcessor.GetByCodeAsync(CompanyId, Code, token);

        public async Task<IEnumerable<InvoiceTemplateSetting>> GetItemsAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await getInvoiceTemplateSettingQueryProcessor.GetItemsAsync(CompanyId, token);

        public async Task<InvoiceTemplateSetting> SaveAsync(InvoiceTemplateSetting InvoiceTemplateSetting, CancellationToken token = default(CancellationToken))
            => await addInvoiceTemplateSettingQueryProcessor.SaveAsync(InvoiceTemplateSetting, token);

        public async Task<int> DeleteAsync(int Id, CancellationToken token = default(CancellationToken))
            => await deleteInvoiceTemplateSettingQueryProcessor.DeleteAsync(Id, token);

    }
}
