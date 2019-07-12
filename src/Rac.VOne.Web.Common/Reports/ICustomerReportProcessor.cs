using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>得意先マスター 帳票</summary>
    public interface ICustomerReportProcessor
    {
        Task<byte[]> GetAsync(CustomerSearch option, CancellationToken token = default(CancellationToken));
    }
}
