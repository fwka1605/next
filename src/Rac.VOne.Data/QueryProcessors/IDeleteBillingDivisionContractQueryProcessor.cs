﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IDeleteBillingDivisionContractQueryProcessor
    {
        Task<int> DeleteWithBillingIdAsync(long BillingId, CancellationToken token = default(CancellationToken));
    }
}
