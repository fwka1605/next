using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>得意先台帳 帳票</summary>
    public interface ICustomerRegisterReportProcessor
    {
        Task<byte[]> GetAsync(CustomerSearch option, CancellationToken token = default(CancellationToken));
    }
}
