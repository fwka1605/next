using Rac.VOne.Web.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IInvoiceNumberSettingProcessor
    {
        Task<InvoiceNumberSetting> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<InvoiceNumberSetting> SaveAsync(InvoiceNumberSetting InvoiceNumberSetting, CancellationToken token = default(CancellationToken));
    }
}
