using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IWebApiSettingProcessor
    {
        Task<WebApiSetting> GetByIdAsync(int companyId, int apiTypeId, CancellationToken token = default(CancellationToken));
        Task<int> SaveAsync(WebApiSetting setting, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int companyId, int? apiTypeId, CancellationToken token = default(CancellationToken));
    }
}
