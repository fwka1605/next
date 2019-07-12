using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>請求部門マスター 帳票</summary>
    public interface IDepartmentReportProcessor
    {
        Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken));
    }
}
