using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class InvoiceNumberSettingProcessor : IInvoiceNumberSettingProcessor
    {
        private readonly IByCompanyGetEntityQueryProcessor<InvoiceNumberSetting> getInvoiceNumberSettingQueryProcessor;
        private readonly IAddInvoiceNumberSettingQueryProcessor addInvoiceNumberSettingQueryProcessor;

        public InvoiceNumberSettingProcessor(
            IByCompanyGetEntityQueryProcessor<InvoiceNumberSetting> getInvoiceNumberSettingQueryProcessor,
            IAddInvoiceNumberSettingQueryProcessor addInvoiceNumberSettingQueryProcessor
            )
        {
            this.getInvoiceNumberSettingQueryProcessor = getInvoiceNumberSettingQueryProcessor;
            this.addInvoiceNumberSettingQueryProcessor = addInvoiceNumberSettingQueryProcessor;
        }

        public async Task<InvoiceNumberSetting> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken))
            => await getInvoiceNumberSettingQueryProcessor.GetAsync(CompanyId, token);

        public async Task<InvoiceNumberSetting> SaveAsync(InvoiceNumberSetting InvoiceNumberSetting, CancellationToken token = default(CancellationToken))
            => await addInvoiceNumberSettingQueryProcessor.SaveAsync(InvoiceNumberSetting, token);

    }
}
