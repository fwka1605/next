using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class InvoiceCommonSettingProcessor : IInvoiceCommonSettingProcessor
    {
        private readonly IByCompanyGetEntityQueryProcessor<InvoiceCommonSetting> getInvoiceCommonSettingQueryProcessor;
        private readonly IAddInvoiceCommonSettingQueryProcessor addInvoiceCommonSettingQueryProcessor;
        private readonly IAddCategoryQueryProcessor addCategoryQueryProcessor;
        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        public InvoiceCommonSettingProcessor(
            IByCompanyGetEntityQueryProcessor<InvoiceCommonSetting> getInvoiceCommonSettingQueryProcessor,
            IAddInvoiceCommonSettingQueryProcessor addInvoiceCommonSettingQueryProcessor,
            IAddCategoryQueryProcessor addCategoryQueryProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.getInvoiceCommonSettingQueryProcessor = getInvoiceCommonSettingQueryProcessor;
            this.addInvoiceCommonSettingQueryProcessor = addInvoiceCommonSettingQueryProcessor;
            this.addCategoryQueryProcessor = addCategoryQueryProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<InvoiceCommonSetting> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await getInvoiceCommonSettingQueryProcessor.GetAsync(CompanyId, token);

        public async Task<InvoiceCommonSetting> SaveAsync(InvoiceCommonSetting seting, CancellationToken token = default(CancellationToken))
            => await addInvoiceCommonSettingQueryProcessor.SaveAsync(seting, token);

    }
}
