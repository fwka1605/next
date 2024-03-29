﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>請求残高年齢表 帳票</summary>
    public interface IBillingAgingListReportProcessor
    {
        Task<byte[]> GetAsync(BillingAgingListSearch option, IProgressNotifier notifier = null, CancellationToken token = default(CancellationToken));
    }
}
