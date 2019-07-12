using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IInvoiceCommonSettingProcessor
    {
        Task<InvoiceCommonSetting> GetAsync(int CompanyId, CancellationToken token = default(CancellationToken));
        Task<InvoiceCommonSetting> SaveAsync(InvoiceCommonSetting InvoiceCommonSetting, CancellationToken token = default(CancellationToken));
    }
}
