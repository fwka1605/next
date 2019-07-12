using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>入金部門・請求部門関連マスター 帳票</summary>
    public interface ISectionWithDepartmentReportProcessor
    {
        Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken));
    }
}
