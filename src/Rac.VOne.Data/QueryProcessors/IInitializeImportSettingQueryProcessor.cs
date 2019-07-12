using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IInitializeImportSettingQueryProcessor
    {
        Task<int> InitialzieAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken));
    }
}
