using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddInvoiceCommonSettingQueryProcessor
    {
        Task<InvoiceCommonSetting> SaveAsync(InvoiceCommonSetting setting, CancellationToken token = default(CancellationToken));
    }
}
