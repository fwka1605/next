using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddInvoiceTemplateSettingQueryProcessor
    {
        Task<InvoiceTemplateSetting> SaveAsync(InvoiceTemplateSetting InvoiceTemplateSetting, CancellationToken token = default(CancellationToken));
    }
}
