using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IWebApiSettingQueryProcessor
    {
        Task<WebApiSetting> GetByIdAsync(int companyId, int apiTypeId, CancellationToken token = default(CancellationToken));
    }
}
