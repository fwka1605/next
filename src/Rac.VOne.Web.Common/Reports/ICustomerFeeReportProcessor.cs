using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common.Reports
{
    /// <summary>得意先手数料マスター 帳票</summary>
    public interface ICustomerFeeReportProcessor
    {
        Task<byte[]> GetAsync(CustomerFeeSearch option, CancellationToken token = default(CancellationToken));
    }
}
