using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public interface IDataMaintenanceProcessor
    {
        Task<int> DeleteDataAsync(DateTime date, CancellationToken token = default(CancellationToken));
    }
}
