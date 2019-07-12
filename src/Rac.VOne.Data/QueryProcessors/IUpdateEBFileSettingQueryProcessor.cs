using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateEBFileSettingQueryProcessor
    {
        Task<int> UpdateIsUseableAsync(int companyId, int loginUserId, IEnumerable<int> ids, CancellationToken token = default(CancellationToken));
    }
}
