using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>ログインユーザーマスター 帳票</summary>
    public interface ILoginUserReportProcessor
    {
        Task<byte[]> GetAsync(int companyId, CancellationToken token = default(CancellationToken));
    }
}
