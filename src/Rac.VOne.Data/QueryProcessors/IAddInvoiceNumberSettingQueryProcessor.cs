using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddInvoiceNumberSettingQueryProcessor
    {
        Task<InvoiceNumberSetting> SaveAsync(InvoiceNumberSetting setting, CancellationToken token = default(CancellationToken));
    }
}
