using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IClosingSettingProcessor
    {
        Task<ClosingSetting> SaveAsync(ClosingSetting setting, CancellationToken token = default(CancellationToken));

        Task<ClosingSetting> GetAsync(int companyId, CancellationToken token = default(CancellationToken));
    }
}
