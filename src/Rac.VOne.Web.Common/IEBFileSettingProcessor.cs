using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IEBFileSettingProcessor
    {
        Task<IEnumerable<EBFileSetting>> GetAsync(EBFileSettingSearch option, CancellationToken token = default(CancellationToken));

        Task<EBFileSetting> SaveAsync(EBFileSetting setting, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int id, CancellationToken token = default(CancellationToken));
        Task<int> UpdateIsUseableAsync(int CompanyId, int LoginUserId, IEnumerable<int>ids, CancellationToken token = default(CancellationToken));
    }
}
