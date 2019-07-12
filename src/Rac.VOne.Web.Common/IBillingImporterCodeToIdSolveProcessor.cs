using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public interface IBillingImporterCodeToIdSolveProcessor
    {
        Task SolveAsync(int CompanyId, IEnumerable<BillingImport> billingImport, CancellationToken token);
    }
}
