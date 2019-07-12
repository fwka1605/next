using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICollationOrderQueryProcessor
    {
        Task<int> InitializeAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken));
    }
}
