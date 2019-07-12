using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IAddMFBillingQueryProcessor
    {
        Task<MFBilling> SaveAsync(MFBilling billing, CancellationToken token = default(CancellationToken));
    }
}
