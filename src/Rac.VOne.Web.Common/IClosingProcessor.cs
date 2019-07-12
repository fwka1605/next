using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IClosingProcessor
    {
        Task<ClosingInformation> GetClosingInformationAsync(int companyId, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ClosingHistory>> GetClosingHistoryAsync(int companyId, CancellationToken token = default(CancellationToken));
        Task<Closing> SaveAsync(Closing closing, CancellationToken token = default(CancellationToken));
        Task<int> DeleteAsync(int companyId, CancellationToken token = default(CancellationToken));
    }
}
