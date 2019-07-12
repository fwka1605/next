﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IBillingAgingListQueryProcessor
    {
        Task<IEnumerable<BillingAgingList>> GetAsync(BillingAgingListSearch searchOption,
            IProgressNotifier notifier = null,
            CancellationToken token = default(CancellationToken));

        Task<IEnumerable<BillingAgingListDetail>> GetDetailsAsync(BillingAgingListSearch SearchOption, CancellationToken token);


    }
}